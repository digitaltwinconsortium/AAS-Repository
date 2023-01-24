
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class ProductCarbonFootprintService : ADXDataService
    {
        private Timer _timer;

        private readonly ILogger _logger;

        public ProductCarbonFootprintService(ILoggerFactory logger, AASXPackageService packageService)
        : base(packageService)
        {
            _logger = logger.CreateLogger("ProductCarbonFootprintService");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CALCULATE_PCF")))
            {
                _timer = new Timer(GeneratePCFAAS);
                _timer.Change(15000, 15000);
            }
        }

        public new void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            base.Dispose();
        }

        private string GetADTHistoryTableName()
        {
            string adtEndpoint = Environment.GetEnvironmentVariable("ADT_HOSTNAME");
            if (!string.IsNullOrEmpty(adtEndpoint))
            {
                string[] adtEndpointParts = adtEndpoint.Split('.');
                string region = string.Empty;
                switch (adtEndpointParts[2])
                {
                    case "eus": region = "eastus"; break;
                    case "eus2": region = "eastus2"; break;
                    case "scus": region = "southcentralus"; break;
                    case "wus2": region = "westus2"; break;
                    case "wus3": region = "westus3"; break;
                    case "aue": region = "australiaeast"; break;
                    case "sea": region = "southeastasia"; break;
                    case "neu": region = "northeurope"; break;
                    case "swc": region = "swedencentral"; break;
                    case "uks": region = "uksouth"; break;
                    case "weu": region = "westeurope"; break;
                    case "cus": region = "centralus"; break;
                    case "san": region = "southafricanorth"; break;
                    case "ci": region = "centralindia"; break;
                    case "ea": region = "eastasia"; break;
                    case "jpe": region = "japaneast"; break;
                    case "kac": region = "koreacentral"; break;
                    case "cc": region = "canadacentral"; break;
                    case "frc": region = "francecentral"; break;
                    case "gwc": region = "germanywestcentral"; break;
                    case "noe": region = "norwayeast"; break;
                    case "swn": region = "switzerlandnorth"; break;
                    case "uaen": region = "uaenorth"; break;
                    case "brs": region = "brazilsouth"; break;
                    case "qtc": region = "qatarcentral"; break;
                    default: throw new Exception("Could not determine region of ADT instance!");
                }

                return "adt_dh_" + adtEndpointParts[0].Replace('-', '_') + "_" + region;
            }

            throw new Exception("Environment variable ADT_HOSTNAME not defined!");
        }

        private void GeneratePCFAAS(object state)
        {
            // we have two production lines in the manufacturing ontologies production line simulation and they are connected like so:
            // assembly -> test -> packaging
            GeneratePCFAASForProductionLine("Munich", "48.1375", "11.575", 6);
            GeneratePCFAASForProductionLine("Seattle", "47.609722", "-122.333056", 10);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private void GeneratePCFAASForProductionLine(string productionLineName, string latitude, string longitude, int idealCycleTime)
        {
            try
            {
                // first of all, retrieve carbon intensity for the location of the production line
                CarbonIntensityQueryResult currentCarbonIntensity = WattTime.GetCarbonIntensity(latitude, longitude).GetAwaiter().GetResult();
                if ((currentCarbonIntensity != null) && (currentCarbonIntensity.data.Length > 0))
                {
                    // check if a new product was produced (last machine in the production line, i.e. packaging, is in state 2 ("done") with a passed QA)
                    // and get the products serial number and energy consumption at that time
                    ConcurrentDictionary<string, object> latestProductProduced = ADXQueryForSpecificValue("packaging", productionLineName, "Status", 2);
                    ConcurrentDictionary<string, object> serialNumberResult = ADXQueryForSpecificTime("packaging", productionLineName, "ProductSerialNumber", ((DateTime)latestProductProduced["SourceTimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);
                    double serialNumber = (double)serialNumberResult["OPCUANodeValue"];

                    ConcurrentDictionary<string, object> timeItWasProducedPackaging = ADXQueryForSpecificValue("packaging", productionLineName, "ProductSerialNumber", serialNumber);
                    ConcurrentDictionary<string, object> energyPackaging = ADXQueryForSpecificTime("packaging", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedPackaging["SourceTimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                    // check each other machine for the time when the product with this serial number was in the machine and get its energy comsumption at that time
                    ConcurrentDictionary<string, object> timeItWasProducedTest = ADXQueryForSpecificValue("test", productionLineName, "ProductSerialNumber", serialNumber);
                    ConcurrentDictionary<string, object> energyTest = ADXQueryForSpecificTime("test", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedTest["SourceTimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                    ConcurrentDictionary<string, object> timeItWasProducedAssembly = ADXQueryForSpecificValue("assembly", productionLineName, "ProductSerialNumber", serialNumber);
                    ConcurrentDictionary<string, object> energyAssembly = ADXQueryForSpecificTime("assembly", productionLineName, "EnergyConsumption", ((DateTime)timeItWasProducedAssembly["SourceTimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss"), idealCycleTime);

                    // calculate the total energy consumption for the product by summing up all the machines' energy consumptions (in KWh), divide by 3600 to get seconds and multiply by the ideal cycle time (which is in seconds)
                    double energyTotal = ((double)energyAssembly["OPCUANodeValue"] + (double)energyTest["OPCUANodeValue"] + (double)energyPackaging["OPCUANodeValue"]) / 3600 * idealCycleTime;

                    // finally calculate the scope 2 product carbon footprint by multiplying the full energy consumption by the current carbon intensity
                    double pcf = energyTotal * currentCarbonIntensity.data[0].intensity.actual;

                    // TODO: Generate and persist AAS with serial number and calculated PCF
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private ConcurrentDictionary<string, object> ADXQueryForSpecificValue(string stationName, string productionLineName, string valueToQuery, double desiredValue)
        {
            string query =
                "let dataHistoryTable = " + GetADTHistoryTableName() + ";\r\n" +
                "let dtId = toscalar(dataHistoryTable\r\n" +
                "| where TimeStamp > now(-1h)\r\n" +
                "| where Key == 'equipmentID'\r\n" +
                "| where Value has \"" + stationName + "\"\r\n" +
                "| where Value  has \"" + productionLineName + "\"\r\n" +
                "| where Value has \"" + valueToQuery + "\"\r\n" +
                "| project Id);\r\n" +
                "let intermediateTable = dataHistoryTable\r\n" +
                "| where TimeStamp > now(-1h)\r\n" +
                "| where Id == dtId;\r\n" +
                "intermediateTable\r\n" +
                "| where isnotnull(SourceTimeStamp)\r\n" +
                "| join kind=innerunique intermediateTable on $left.TimeStamp == $right.TimeStamp\r\n" +
                "| where Key1 == \"OPCUADisplayName\"\r\n" +
                "| distinct SourceTimeStamp, OPCUANodeValue = toint(Value)" +
                "| where OPCUANodeValue == " + desiredValue.ToString() +
                "| sort by SourceTimeStamp desc";

            ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
            RunADXQuery(query, values);

            return values;
        }

        private ConcurrentDictionary<string, object> ADXQueryForSpecificTime(string stationName, string productionLineName, string valueToQuery, string timeToQuery, int idealCycleTime)
        {
            string query =
                "let dataHistoryTable = " + GetADTHistoryTableName() + ";\r\n" +
                "let dtId = toscalar(dataHistoryTable\r\n" +
                "| where TimeStamp > now(-1h)\r\n" +
                "| where Key == 'equipmentID'\r\n" +
                "| where Value has \"" + stationName + "\"\r\n" +
                "| where Value  has \"" + productionLineName + "\"\r\n" +
                "| where Value has \"" + valueToQuery + "\"\r\n" +
                "| project Id);\r\n" +
                "let intermediateTable = dataHistoryTable\r\n" +
                "| where TimeStamp > now(-1h)\r\n" +
                "| where Id == dtId;\r\n" +
                "intermediateTable\r\n" +
                "| where isnotnull(SourceTimeStamp)\r\n" +
                "| join kind=innerunique intermediateTable on $left.TimeStamp == $right.TimeStamp\r\n" +
                "| where Key1 == \"OPCUADisplayName\"\r\n" +
                "| distinct SourceTimeStamp, OPCUANodeValue = todouble(Value)" +
                "| where around(SourceTimeStamp, datetime(" + timeToQuery + "), " + idealCycleTime.ToString() + "s)" +
                "| sort by SourceTimeStamp desc";

            ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
            RunADXQuery(query, values);

            return values;
        }
    }
}
