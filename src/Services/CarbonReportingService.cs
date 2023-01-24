
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;

    public class CarbonReportingService : ADXDataService
    {
        private Timer _queryTimer;
        private float _geCO2Footprint = 0.0f;
        private ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<string, object> _values1MinuteAgo = new ConcurrentDictionary<string, object>();
        private CarbonIntensityQueryResult _currentIntensity = null;

        private readonly ILogger _logger;

        public CarbonReportingService(ILoggerFactory logger, AASXPackageService packageService)
        : base(packageService)
        {
            _logger = logger.CreateLogger("CarbonReportingService");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CARBON_REPORTING")))
            {
                _queryTimer = new Timer(RunQuerys);

                string adxQueryInterval = Environment.GetEnvironmentVariable("ADX_QUERY_INTERVAL");
                if (adxQueryInterval != null && int.TryParse(adxQueryInterval, out int interval))
                {
                    _queryTimer.Change(interval, interval);
                }
                else
                {
                    // default to 5s interval
                    _queryTimer.Change(5000, 5000);
                }
            }
        }

        public new void Dispose()
        {
            if (_queryTimer != null)
            {
                _queryTimer.Dispose();
            }

            base.Dispose();
        }

        private async void RunQuerys(object state)
        {
            // read the row from our OPC UA telemetry table
            RunADXQuery("opcua_telemetry | top 1 by creationTimeUtc desc", _values);

            // read the row from our OPC UA telemetry table
            RunADXQuery("opcua_telemetry | where creationTimeUtc > (now() - 2m) | where creationTimeUtc < (now() - 1m) | top 1 by creationTimeUtc desc", _values1MinuteAgo);

            string latitude = Environment.GetEnvironmentVariable("WATTTIME_LATITUDE");
            string longitude = Environment.GetEnvironmentVariable("WATTTIME_LONGITUDE");
            await WattTime.GetCarbonIntensity(latitude, longitude).ConfigureAwait(false);

            // get CO2 foot print data from our supply chain, in this case the GE machine's AAS
            _geCO2Footprint = ReadCarbonReportFromRemoteAAS("https://carbonreportingge.azurewebsites.net/", "0", "Energy_model_harmonized");

            UpdateSMEValues();

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private void UpdateSMEValues()
        {
            foreach (SubmodelElement sme in _dataPoints)
            {
                if (sme is Property prop)
                {
                    prop.Value = GetCarbonReportingValue(sme).ToString();
                }
                if (sme is Blob blob)
                {
                    blob.Value = GetCarbonReportingValue(sme).ToString();
                }
            }
        }

        public double GetCarbonReportingValue(SubmodelElement sme)
        {
            try
            {
                foreach (Qualifier q in sme.Qualifiers)
                {
                    if (q.Type == "ADX")
                    {
                        if (_values.ContainsKey(q.Value))
                        {
                            return (double)_values[q.Value];
                        }
                        else
                        {
                            // calculate our other values
                            if (q.Value == "ActiveEnergy1minTotal" && _values.ContainsKey("ActiveEnergy") && _values1MinuteAgo.ContainsKey("ActiveEnergy"))
                            {
                                double activeEnergyNow = (double)_values["ActiveEnergy"];
                                double activeEnergy1MinuteAgo = (double)_values1MinuteAgo["ActiveEnergy"];
                                return activeEnergyNow - activeEnergy1MinuteAgo;
                            }

                            if (q.Value == "Co2EquivalentTotal" && _values.ContainsKey("ActiveEnergy"))
                            {
                                // Active Energy is in Wh
                                // Carbon Intensity is in gCo2/KWh
                                // Therefore, to get gCO2/Wh, we need to divide by 1000
                                if (_currentIntensity == null ||
                                    _currentIntensity.data.Length == 0 ||
                                    _currentIntensity.data[0] == null ||
                                    _currentIntensity.data[0].intensity == null)
                                {
                                    // the German carbon intensity average is 515gCO2/kWh
                                    return (double)_values["ActiveEnergy"] * 515 / 1000;
                                }
                                else
                                {
                                    return (double)_values["ActiveEnergy"] * _currentIntensity.data[0].intensity.actual / 1000;
                                }
                            }

                            if (q.Value == "Co2Equivalent1minTotal" && _values.ContainsKey("ActiveEnergy") && _values1MinuteAgo.ContainsKey("ActiveEnergy"))
                            {
                                if (_currentIntensity == null ||
                                    _currentIntensity.data.Length == 0 ||
                                    _currentIntensity.data[0] == null ||
                                    _currentIntensity.data[0].intensity == null)
                                {
                                    return ((double)_values["ActiveEnergy"] - (double)_values1MinuteAgo["ActiveEnergy"]) * 515 / 1000 + _geCO2Footprint;
                                }
                                else
                                {
                                    return ((double)_values["ActiveEnergy"] - (double)_values1MinuteAgo["ActiveEnergy"]) * _currentIntensity.data[0].intensity.actual / 1000 + _geCO2Footprint;
                                }
                            }
                        }

                        break;
                    }
                }

                // if we can't find it, simply return 0
                return 0.0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return 0.0;
            }
        }

        float ReadCarbonReportFromRemoteAAS(string aasServerAddress, string aasId, string smIdShort)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(aasServerAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Submodel submodel = null;
                var path = $"/aas/{aasId}/Submodels/{smIdShort}/complete";
                HttpResponseMessage response = client.GetAsync(path).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    using (TextReader reader = new StringReader(json))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        submodel = (Submodel)serializer.Deserialize(reader, typeof(Submodel));
                    }
                }

                // access electrical energy
                SubmodelElement smcEe = FindAllSMEs(submodel?.SubmodelElements, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/electrical-energy/1/0")).FirstOrDefault();

                // access CO2 per 1 minute
                foreach (SubmodelElement smcPhase in FindAllSMEs(smcEe, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/electrical-total/1/0")))
                {
                    // get Value
                    string value = ((Property)FindAllSMEs(smcPhase, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/co2-equivalent-per-1-min/1/0")).FirstOrDefault()).Value;
                    if (value != null && float.TryParse(value, out float f))
                    {
                        return f;
                    }
                    else
                    {
                        return 0.0f;
                    }
                }

                return 0.0f;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Accessing other AAS failed with: " + ex.Message);
                return 0.0f;
            }
        }

        private IEnumerable<SubmodelElement> FindAllSMEs(SubmodelElement smeInput, Identifier semId)
        {
            if (smeInput is SubmodelElementCollection collection)
                foreach (SubmodelElementWrapper smew in collection.Value)
                    if (smew.SubmodelElement.SemanticId != null)
                        if (smew.SubmodelElement.SemanticId.Matches(semId))
                            yield return smew.SubmodelElement;
        }

        private IEnumerable<SubmodelElement> FindAllSMEs(List<SubmodelElementWrapper> smewc, Identifier semId)
        {
            foreach (SubmodelElementWrapper smew in smewc)
                if (smew.SubmodelElement.SemanticId != null)
                    if (smew.SubmodelElement.SemanticId.Matches(semId))
                        yield return smew.SubmodelElement;
        }
    }
}
