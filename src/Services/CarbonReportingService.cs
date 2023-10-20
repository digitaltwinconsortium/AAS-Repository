
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Threading;

    public class CarbonReportingService : IDisposable
    {
        private Timer _queryTimer;
        private float _geCO2Footprint = 0.0f;
        private ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();
        private ConcurrentDictionary<string, object> _values1MinuteAgo = new ConcurrentDictionary<string, object>();
        private CarbonIntensityQueryResult _currentIntensity = null;

        private readonly ILogger _logger;

        private readonly AASXPackageService _packageService;

        private readonly ADXDataService _adxDataService;

        public CarbonReportingService(ILoggerFactory logger, AASXPackageService packageService, ADXDataService adxDataService)
        {
            _logger = logger.CreateLogger("CarbonReportingService");

            _packageService = packageService;

            _adxDataService = adxDataService;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CARBON_REPORTING")))
            {
                _queryTimer = new Timer(RunQuerys);

                string dataQueryInterval = Environment.GetEnvironmentVariable("DATA_QUERY_INTERVAL");
                if (dataQueryInterval != null && int.TryParse(dataQueryInterval, out int interval))
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

        public void Dispose()
        {
            if (_queryTimer != null)
            {
                _queryTimer.Dispose();
            }
        }

        private async void RunQuerys(object state)
        {
            // read the row from our OPC UA telemetry table
            _adxDataService.RunADXQuery("opcua_telemetry | top 1 by creationTimeUtc desc", _values);

            // read the row from our OPC UA telemetry table
            _adxDataService.RunADXQuery("opcua_telemetry | where creationTimeUtc > (now() - 2m) | where creationTimeUtc < (now() - 1m) | top 1 by creationTimeUtc desc", _values1MinuteAgo);

            string latitude = Environment.GetEnvironmentVariable("WATTTIME_LATITUDE");
            string longitude = Environment.GetEnvironmentVariable("WATTTIME_LONGITUDE");
            await WattTime.GetCarbonIntensity(latitude, longitude).ConfigureAwait(false);

            // get CO2 foot print data from our supply chain, in this case the GE machine's AAS
            try
            {
                _geCO2Footprint = CarbonReportingClient.ReadCarbonReportFromRemoteAAS("https://carbonreportingge.azurewebsites.net/", "0", "Energy_model_harmonized");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Accessing Carbon Reporting AAS failed with: " + ex.Message);
            }

            UpdateSMEValues();

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private void UpdateSMEValues()
        {
            // retrieve our ADX-tagged data points from all loaded AASes
            foreach (AssetAdministrationShellEnvironment env in _packageService.Packages.Values)
            {
                foreach (Submodel sm in env.Submodels)
                {
                    foreach (SubmodelElementWrapper smew in sm.SubmodelElements)
                    {
                        CheckForADXDataPointInSME(smew.SubmodelElement);
                    }
                }
            }
        }

        protected void CheckForADXDataPointInSME(SubmodelElement sme)
        {
            // recurse if needed
            if (sme is SubmodelElementCollection collection)
            {
                if (collection.Value != null)
                {
                    foreach (SubmodelElementWrapper smew in collection.Value)
                    {
                        CheckForADXDataPointInSME(smew.SubmodelElement);
                    }
                }
            }

            foreach (Qualifier q in sme.Qualifiers)
            {
                if (q.Type == "ADX")
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
    }
}
