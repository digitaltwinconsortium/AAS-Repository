using AdminShell;
using Kusto.Cloud.Platform.Utils;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace AasxDemonstration
{
    /// <summary>
    /// This class holds information and provides functions to "automate" the energy model
    /// used by the CESMII / LNI4.0 demonstrator.
    /// It consists of Properties, which shall be synchronized with Azure Data Explorer. It
    /// includes a time series (according the SM template spec) as well.
    /// </summary>
    public class EnergyModel
    {
        /// <summary>
        /// Base class for the source system and its context. Can be used to transport
        /// context and global status data w.r.t to the online connect to a source system
        /// </summary>
        public class SourceSystemBase {

            public static SourceSystemBase FactoryNewSystem(
                string sourceType,
                string sourceAddress,
                string user, string password,
                string credentials)
            {
                sourceType = ("" + sourceType).Trim().ToLower();

                if (sourceType == "azuredataexplorer")
                    return new SourceSystemAzureDataExplorer(sourceAddress, user, password, credentials);

                // no, default
                return new SourceSystemBase();
            }
        }

        /// <summary>
        /// Implements a source system, which gets data from Azure Data Explorer
        /// </summary>
        public class SourceSystemAzureDataExplorer : SourceSystemBase, IDisposable
        {
            private static ICslQueryProvider _queryProvider = null;
            private static ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();
            private static ConcurrentDictionary<string, object> _values1MinuteAgo = new ConcurrentDictionary<string, object>();
            private Timer _queryTimer = new Timer(RunQuerys, null, Timeout.Infinite, Timeout.Infinite);
            private static CarbonIntensityQueryResult _currentIntensity = null;
            private static float _geCO2Footprint = 0.0f;

            public double GetValue(string sourceID)
            {
                try
                {
                    if (_values.ContainsKey(sourceID))
                    {
                        // lookup
                        return (double)_values[sourceID];
                    }
                    else
                    {
                        // calculate our other values
                        if ((sourceID == "ActiveEnergy1minTotal") && _values.ContainsKey("ActiveEnergy") && _values1MinuteAgo.ContainsKey("ActiveEnergy"))
                        {
                            double activeEnergyNow = (double)_values["ActiveEnergy"];
                            double activeEnergy1MinuteAgo = (double)_values1MinuteAgo["ActiveEnergy"];
                            return activeEnergyNow - activeEnergy1MinuteAgo;
                        }

                        if ((sourceID == "Co2EquivalentTotal") && _values.ContainsKey("ActiveEnergy"))
                        {
                            // Active Energy is in Wh
                            // Carbon Intensity is in gCo2/KWh
                            // Therefore, to get gCO2/Wh, we need to divide by 1000
                            if ((_currentIntensity == null) ||
                                (_currentIntensity.data.Length == 0) ||
                                (_currentIntensity.data[0] == null) ||
                                (_currentIntensity.data[0].intensity == null))
                            {
                                // the German carbon intensity average is 515gCO2/kWh
                                return ((double)_values["ActiveEnergy"]) * 515 / 1000;
                            }
                            else
                            {
                                return ((double)_values["ActiveEnergy"]) * _currentIntensity.data[0].intensity.actual / 1000;
                            }
                        }

                        if ((sourceID == "Co2Equivalent1minTotal") && _values.ContainsKey("ActiveEnergy") && _values1MinuteAgo.ContainsKey("ActiveEnergy"))
                        {
                            if ((_currentIntensity == null) ||
                                (_currentIntensity.data.Length == 0) ||
                                (_currentIntensity.data[0] == null) ||
                                (_currentIntensity.data[0].intensity == null))
                            {
                                return ((((double)_values["ActiveEnergy"]) - ((double)_values1MinuteAgo["ActiveEnergy"])) * 515 / 1000) + _geCO2Footprint;
                            }
                            else
                            {
                                return ((((double)_values["ActiveEnergy"]) - ((double)_values1MinuteAgo["ActiveEnergy"])) * _currentIntensity.data[0].intensity.actual / 1000) + _geCO2Footprint;
                            }
                        }

                        // if we can't find it, simply return 0
                        return 0.0;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    return 0.0;
                }
            }

            static float ReadCO2Per1Minute(string aasServerAddress, string aasId, string smIdShort)
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
                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
                catch (Exception)
                {
                    return 0.0f;
                }
            }

            private static IEnumerable<SubmodelElement> FindAllSMEs(SubmodelElement smeInput, Identifier semId)
            {
                if (smeInput is SubmodelElementCollection collection)
                    foreach (SubmodelElement sme in collection.Value)
                        if (sme.SemanticId != null)
                            if (sme.SemanticId.Matches(semId))
                                yield return sme;
            }

            private static IEnumerable<SubmodelElement> FindAllSMEs(List<SubmodelElement> smec, Identifier semId)
            {
                foreach (SubmodelElement sme in smec)
                    if (sme.SemanticId != null)
                        if (sme.SemanticId.Matches(semId))
                            yield return sme;
            }

            private static async void RunQuerys(object state)
            {
                // read the row from our OPC UA telemetry table
                RunADXQuery("opcua_telemetry | top 1 by creationTimeUtc desc", _values);

                // read the row from our OPC UA telemetry table
                RunADXQuery("opcua_telemetry | where creationTimeUtc > (now() - 2m) | where creationTimeUtc < (now() - 1m) | top 1 by creationTimeUtc desc", _values1MinuteAgo);


                string watttimeUser = Environment.GetEnvironmentVariable("WATTTIME_USER");
                if (!string.IsNullOrEmpty(watttimeUser))
                {
                    try
                    {
                        // read current carbon intensity from the WattTime service at https://api2.watttime.org/v2
                        HttpClient webClient = new HttpClient();

                        // login
                        string watttimePassword = Environment.GetEnvironmentVariable("WATTTIME_PASSWORD");
                        webClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(watttimeUser + ":" + watttimePassword)));

                        HttpResponseMessage response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/login")).ConfigureAwait(false);
                        string token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        token = token.TrimStart("{\"token\":\"").TrimEnd("\"}");

                        // now use bearer token returned previously
                        webClient.DefaultRequestHeaders.Remove("Authorization");
                        webClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        // determine grid region
                        string watttimeLatitude = Environment.GetEnvironmentVariable("WATTTIME_LATITUDE");
                        string watttimeLongitude = Environment.GetEnvironmentVariable("WATTTIME_LONGITUDE");
                        response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/ba-from-loc?latitude=" + watttimeLatitude + "&longitude=" + watttimeLongitude)).ConfigureAwait(false);
                        RegionQueryResult region = JsonConvert.DeserializeObject<RegionQueryResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                        // get the carbon intensity
                        response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/index?ba=" + region.abbrev + "&style=moer")).ConfigureAwait(false);
                        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        WattTimeQueryResult result = JsonConvert.DeserializeObject<WattTimeQueryResult>(content);

                        _currentIntensity = new CarbonIntensityQueryResult()
                        {
                            data = new CarbonData[1]
                        };

                        // convert from lbs/MWh to g/KWh
                        _currentIntensity.data[0] = new CarbonData();
                        _currentIntensity.data[0].intensity = new CarbonIntensity()
                        {
                            actual = (int) (result.moer * 453.592f / 1000.0f)
                        };
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
                else
                {
                    try
                    {
                        // read current carbon intensity from the UK national energy grid's carbon intensity service at https://api.carbonintensity.org.uk/intensity
                        HttpClient webClient = new HttpClient();
                        HttpResponseMessage response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api.carbonintensity.org.uk/intensity")).ConfigureAwait(false);
                        _currentIntensity = JsonConvert.DeserializeObject<CarbonIntensityQueryResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }

                // get CO2 foot print data from our supply chain, in this case the GE machine's AAS
                _geCO2Footprint = ReadCO2Per1Minute("https://carbonreportingge.azurewebsites.net/", "0", "Energy_model_harmonized");
            }

            private static void RunADXQuery(string query, ConcurrentDictionary<string, object> values)
            {
                ClientRequestProperties clientRequestProperties = new ClientRequestProperties()
                {
                    ClientRequestId = Guid.NewGuid().ToString()
                };

                try
                {
                    using (IDataReader reader = _queryProvider.ExecuteQuery(query, clientRequestProperties))
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                try
                                {
                                    if (reader.GetValue(i) != null)
                                    {
                                        if (values.ContainsKey(reader.GetName(i)))
                                        {
                                            values[reader.GetName(i)] = reader.GetValue(i);
                                        }
                                        else
                                        {
                                            values.TryAdd(reader.GetName(i), reader.GetValue(i));
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);

                                    // ignore this field and move on
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            public SourceSystemAzureDataExplorer() : base()
            {
                // do nothing in default constructor
            }

            public SourceSystemAzureDataExplorer(
                string sourceAddress,
                string user, string tenant,
                string credentials)
                : base()
            {
                string key = Environment.GetEnvironmentVariable("ADX_PASSWORD");
                if (!string.IsNullOrEmpty(key))
                {
                    KustoConnectionStringBuilder connectionString = new KustoConnectionStringBuilder(sourceAddress, credentials).WithAadApplicationKeyAuthentication(user, key, tenant);
                    _queryProvider = KustoClientFactory.CreateCslQueryProvider(connectionString);

                    string queryInternval = Environment.GetEnvironmentVariable("ADX_QUERY_INTERVAL");
                    int interval = 5000;
                    if ((queryInternval != null) && int.TryParse(queryInternval, out interval))
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

                if (_queryProvider != null)
                {
                    _queryProvider.Dispose();
                    _queryProvider = null;
                }
            }
        }
    }
}
