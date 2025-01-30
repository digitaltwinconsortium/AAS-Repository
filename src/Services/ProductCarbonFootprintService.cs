using Newtonsoft.Json;
using SMIP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AdminShell
{
    public class ProductCarbonFootprintService : IDisposable
    {
        private Timer _timer;

        private readonly ADXDataService _adxDataService;
        private readonly SMIPDataService _smipDataService;
        private readonly AssetAdministrationShellEnvironmentService _envService;

        public ProductCarbonFootprintService(ADXDataService adxDataService, SMIPDataService smipDataService, AssetAdministrationShellEnvironmentService envService)
        {
            _adxDataService = adxDataService;
            _smipDataService = smipDataService;
            _envService = envService;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CALCULATE_PCF"))
             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CALCULATE_PCF_SMIP")))
            {
                _timer = new Timer(GeneratePCFAAS);

                string dataQueryInterval = Environment.GetEnvironmentVariable("DATA_QUERY_INTERVAL");
                if (dataQueryInterval != null && int.TryParse(dataQueryInterval, out int interval))
                {
                    _timer.Change(interval, interval);
                }
                else
                {
                    // default to 15s interval
                    _timer.Change(15000, 15000);
                }
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private void GeneratePCFAAS(object state)
        {
            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CALCULATE_PCF_SMIP")))
                {
                    // we have a single pulp & paper machine from North Carolina State Univeristy that produced a roll of paper over 3 days
                    GeneratePCFAASForSMIP("NCSU Pulp & Paper batch ", "35.787222", "-78.670556", "79078", new DateTime(2023, 10, 12, 0, 0, 0), new DateTime(2023, 10, 14, 23, 59, 59));
                }

                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CALCULATE_PCF")))
                {
                    // we have two production lines in the manufacturing ontologies production line simulation and they are connected like so:
                    // assembly -> test -> packaging
                    GeneratePCFAASForProductionLine("Munich", "48.1375", "11.575", 6);
                    GeneratePCFAASForProductionLine("Seattle", "47.609722", "-122.333056", 10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GeneratePCFAAS: " + ex.Message);
            }
        }

        private void GeneratePCFAASForSMIP(string name, string latitude, string longitude, string productionLineID, DateTime batchCycleStart, DateTime batchCycleEnd)
        {
            try
            {
                string productionLineQuery = $@"{{
                  places(filter: {{partOfId: {{equalTo: ""{productionLineID}""}}}}) {{
                    id
                    displayName
                    equipment {{
                      displayName
                      id
                      attributes {{
                        displayName
                        id
                        dataType
                      }}
                    }}
                  }}
                }}";

                string productionLineQueryResponse = _smipDataService.RunSMIPQuery(productionLineQuery);
                AssetHierarchy assetHierarchy = JsonConvert.DeserializeObject<AssetHierarchy>(productionLineQueryResponse);

                // extract all equipment with power consumption
                double totalEnergyConsumption = 0.0f;
                foreach (AssetHierarchy.Place place in assetHierarchy.places)
                {
                    foreach (AssetHierarchy.Equipment equipment in place.equipment)
                    {
                        foreach (AssetHierarchy.Attribute attribute in equipment.attributes)
                        {
                            if (attribute.displayName == "Power")
                            {
                                // get time-series for the batch
                                string timeseriesQuery = $@"{{
                                  getRawHistoryDataWithSampling(
                                    maxSamples: 0,
                                    ids: [""{attribute.id}""],
                                    startTime: ""{batchCycleStart.ToString("yyyy-MM-dd HH:mm:ss+00")}"",
                                    endTime: ""{batchCycleEnd.ToString("yyyy-MM-dd HH:mm:ss+00")}""
                                  ) {{
    		                        id
                                    floatvalue
                                    ts
                                  }}
                                }}";

                                string timeseriesQueryResponse = _smipDataService.RunSMIPQuery(timeseriesQuery);
                                TimeSeries equipmentHistory = JsonConvert.DeserializeObject<TimeSeries>(timeseriesQueryResponse);

                                // capture cycle times (they are dynamic) and energy consumption during a cycle
                                DateTime cycleStart = DateTime.MinValue;
                                DateTime cycleEnd = DateTime.MinValue;
                                DateTime previousSampleTimeStamp = DateTime.MinValue;
                                double previousSamplePower = 0.0f;
                                bool inCycle = false;
                                double cycleEnergyConsumption = 0.0f;
                                foreach (TimeSeries.GetRawHistoryDataWithSampling equipmentHistoryItem in equipmentHistory.getRawHistoryDataWithSampling)
                                {
                                    if (!inCycle && ((equipmentHistoryItem.floatvalue == 0) || (equipmentHistoryItem.floatvalue == null)))
                                    {
                                        // skip ahead until we find a value that is not zero
                                        continue;
                                    }

                                    if (!inCycle && (equipmentHistoryItem.floatvalue != 0) && (equipmentHistoryItem.floatvalue != null))
                                    {
                                        // beginning of cycle
                                        inCycle = true;
                                        cycleStart = equipmentHistoryItem.ts;
                                        previousSampleTimeStamp = equipmentHistoryItem.ts;
                                    }

                                    if (inCycle && ((equipmentHistoryItem.floatvalue == 0) || (equipmentHistoryItem.floatvalue == null)))
                                    {
                                        // end of cycle
                                        inCycle = false;
                                        cycleEnd = equipmentHistoryItem.ts;

                                        // update energy consumption
                                        double secondsSinceLastSample = (equipmentHistoryItem.ts - previousSampleTimeStamp).TotalSeconds;
                                        cycleEnergyConsumption += (secondsSinceLastSample * previousSamplePower);

                                        // reset
                                        previousSampleTimeStamp = DateTime.MinValue;
                                        previousSamplePower = 0.0f;
                                    }

                                    if (inCycle && (equipmentHistoryItem.floatvalue != 0) && (equipmentHistoryItem.floatvalue != null))
                                    {
                                        Debug.Assert(previousSampleTimeStamp != DateTime.MinValue);

                                        // update energy consumption
                                        double secondsSinceLastSample = (equipmentHistoryItem.ts - previousSampleTimeStamp).TotalSeconds;
                                        cycleEnergyConsumption += (secondsSinceLastSample * previousSamplePower);

                                        previousSampleTimeStamp = equipmentHistoryItem.ts;
                                        previousSamplePower = (double)equipmentHistoryItem.floatvalue;

                                        // skip ahead unitl we find a value that is zero
                                        continue;
                                    }

                                    // check if we have both start and end and capture
                                    if ((cycleStart != DateTime.MinValue) && (cycleEnd != DateTime.MinValue))
                                    {
                                        // convert from Ws to kWh
                                        cycleEnergyConsumption = cycleEnergyConsumption / 3600 / 1000;

                                        Debug.WriteLine("Found cycle for equipment " + equipment.displayName + " from " + cycleStart.ToString() + " to " + cycleEnd.ToString() + ", energy consumption " + cycleEnergyConsumption.ToString() + " kWh.");
                                        totalEnergyConsumption += cycleEnergyConsumption;

                                        // reset
                                        cycleStart = DateTime.MinValue;
                                        cycleEnd = DateTime.MinValue;
                                        cycleEnergyConsumption = 0.0f;
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine("Total energy consumption of batch: " + totalEnergyConsumption.ToString() + " kWh.");

                // first of all, retrieve carbon intensity for the location of the production line
                CarbonIntensityQueryResult currentCarbonIntensity = WattTimeClient.GetCarbonIntensity(latitude, longitude).GetAwaiter().GetResult();
                if ((currentCarbonIntensity != null) && (currentCarbonIntensity.data.Length > 0))
                {
                    // we set scope 1 emissions to a fixed quantity of 1000 gCO2-equivalent
                    float scope1Emissions = 1000.0f;

                    // finally calculate the scope 2 product carbon footprint by multiplying the full energy consumption by the current carbon intensity
                    float scope2Emissions = (float)totalEnergyConsumption * currentCarbonIntensity.data[0].intensity.actual;

                    // we calculate scope 3 emissions from published figures: https://pubs.acs.org/doi/full/10.1021/acssuschemeng.2c00840
                    // 1Kg of wood-based fluff pulp produces 1102 gCO2-equivalent
                    // NCSU uses 113 lbs of pulp in one batch, therefore:
                    // 113 * 453,59 / 1000 * 1102 = 56483,75 gCO2-equivalent
                    float scope3Emissions = 56483.75f;

                    // finally calculate our PCF
                    float pcf = scope1Emissions + scope2Emissions + scope3Emissions;

                    Debug.WriteLine("Total carbon intensity of batch: " + pcf.ToString() + " gCO2");

                    // persist AAS with serial number and calculated PCF
                    PersistAAS(name, 1, pcf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GeneratePCFAASForSMIP: " + ex.Message);
            }
        }

        private void GeneratePCFAASForProductionLine(string productionLineName, string latitude, string longitude, int idealCycleTime)
        {
            try
            {
                // first of all, retrieve carbon intensity for the location of the production line
                CarbonIntensityQueryResult currentCarbonIntensity = WattTimeClient.GetCarbonIntensity(latitude, longitude).GetAwaiter().GetResult();
                if ((currentCarbonIntensity != null) && (currentCarbonIntensity.data.Length > 0))
                {
                    // check if a new product was produced (last machine in the production line, i.e. packaging, is in state 2 ("done") with a passed QA)
                    // and get the products serial number and energy consumption at that time
                    ConcurrentDictionary<string, object> latestProductProduced = ADXQueryForSpecificValue("packaging", productionLineName, "Status", 2);
                    if ((latestProductProduced != null) && (latestProductProduced.Count > 0))
                    {
                        ConcurrentDictionary<string, object> serialNumberResult = ADXQueryForSpecificTime("packaging", productionLineName, "ProductSerialNumber", ((DateTime)latestProductProduced["Timestamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);
                        double serialNumber = (double)serialNumberResult["OPCUANodeValue"];

                        ConcurrentDictionary<string, object> timeItWasProducedPackaging = ADXQueryForSpecificValue("packaging", productionLineName, "ProductSerialNumber", serialNumber);
                        ConcurrentDictionary<string, object> energyPackaging = ADXQueryForSpecificTime("packaging", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedPackaging["Timestamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                        // check each other machine for the time when the product with this serial number was in the machine and get its energy comsumption at that time
                        ConcurrentDictionary<string, object> timeItWasProducedTest = ADXQueryForSpecificValue("test", productionLineName, "ProductSerialNumber", serialNumber);
                        ConcurrentDictionary<string, object> energyTest = ADXQueryForSpecificTime("test", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedTest["Timestamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                        ConcurrentDictionary<string, object> timeItWasProducedAssembly = ADXQueryForSpecificValue("assembly", productionLineName, "ProductSerialNumber", serialNumber);
                        ConcurrentDictionary<string, object> energyAssembly = ADXQueryForSpecificTime("assembly", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedAssembly["Timestamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                        // calculate the total energy consumption for the product by summing up all the machines' energy consumptions (in Ws), divide by 3600 to get seconds and multiply by the ideal cycle time (which is in seconds)
                        double energyTotal = ((double)energyAssembly["OPCUANodeValue"] + (double)energyTest["OPCUANodeValue"] + (double)energyPackaging["OPCUANodeValue"]) / 3600 * idealCycleTime;

                        // we set scope 1 emissions to 0
                        float scope1Emissions = 0.0f;

                        // finally calculate the scope 2 product carbon footprint by multiplying the full energy consumption by the current carbon intensity
                        float scope2Emissions = (float)energyTotal * currentCarbonIntensity.data[0].intensity.actual;

                        // we set scope 3 emissions to 0
                        float scope3Emissions = 0.0f;

                        // finally calculate our PCF
                        float pcf = scope1Emissions + scope2Emissions + scope3Emissions;

                        // persist AAS with serial number and calculated PCF
                        PersistAAS(productionLineName, serialNumber, pcf);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GeneratePCFAASForProductionLine: " + ex.Message);
            }
        }

        private void PersistAAS(string productionLineName, double serialNumber, float pcf)
        {
            string aasName = "CarbonFootprintAAS_" + productionLineName + "_" + serialNumber.ToString();
            string pathToAAS = "./NodeSets/CarbonFootprintAAS_" + aasName + ".NodeSet2.xml";

            SimpleServer server = (SimpleServer)Program.App.Server;
            NodesetFileNodeManager nodeManager = (NodesetFileNodeManager)server.CurrentInstance.NodeManager.NodeManagers[2];

            // write the values to a JSON file
            Dictionary<string, string> values = new()
            {
                { "i=10", pcf.ToString() }
            };
            string pathToAASValues = "./NodeSets/CarbonFootprintAAS_" + aasName + "_Values.json";
            System.IO.File.WriteAllText(pathToAASValues, JsonConvert.SerializeObject(values));

            // copy AAS Submodel Template for CO2 footprint to our Nodeset directory
            System.IO.File.Copy("./CarbonFootprintAAS.NodeSet2.xml", pathToAAS, true);

            // replace namespace in .nodeset XML file
            System.IO.File.WriteAllText(pathToAAS, System.IO.File.ReadAllText(pathToAAS).Replace("CarbonFootprintAAS", aasName));

            // add the namespace to the server
            nodeManager.AddNamespace(pathToAAS);

            // add the nodes to the server
            nodeManager.AddNodesFromNodesetXml(pathToAAS);

            // disconnect all client sessions
            OpcSessionHelper.Instance.DisconnectAll();

            Console.WriteLine("Persisted AAS for " + aasName + " with PCF " + pcf.ToString() + " gCO2.");
        }

        private ConcurrentDictionary<string, object> ADXQueryForSpecificValue(string stationName, string productionLineName, string valueToQuery, double desiredValue)
        {
            string query = "opcua_metadata_lkv\r\n"
                         + "| where Name contains \"" + stationName + "\"\r\n"
                         + "| where Name contains \"" + productionLineName + "\"\r\n"
                         + "| join kind = inner(opcua_telemetry\r\n"
                         + "    | where Name == \"" + valueToQuery + "\"\r\n"
                         + "    | where Timestamp > now(- 1h)\r\n"
                         + ") on DataSetWriterID\r\n"
                         + "| distinct Timestamp, OPCUANodeValue = todouble(Value)\r\n"
                         + "| sort by Timestamp desc";

            ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
            _adxDataService.RunADXQuery(query, values);

            return values;
        }

        private ConcurrentDictionary<string, object> ADXQueryForSpecificTime(string stationName, string productionLineName, string valueToQuery, string timeToQuery, int idealCycleTime)
        {
            string query = "opcua_metadata_lkv\r\n"
                         + "| where Name contains \"" + stationName + "\"\r\n"
                         + "| where Name contains \"" + productionLineName + "\"\r\n"
                         + "| join kind = inner(opcua_telemetry\r\n"
                         + "    | where Name == \"" + valueToQuery + "\"\r\n"
                         + "    | where Timestamp > now(- 1h)\r\n"
                         + ") on DataSetWriterID\r\n"
                         + "| distinct Timestamp, OPCUANodeValue = todouble(Value)\r\n"
                         + "| where around(Timestamp, datetime(" + timeToQuery + "), " + idealCycleTime.ToString() + "s)\r\n"
                         + "| sort by Timestamp desc";

            ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
            _adxDataService.RunADXQuery(query, values);

            return values;
        }
    }
}
