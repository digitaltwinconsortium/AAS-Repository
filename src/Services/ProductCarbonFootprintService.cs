
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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
                string[] adtEndpointParts = adtEndpoint.Replace("https://", string.Empty).Split('.');
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

                    // we set scope 1 emissions to a fixed quantity of 1gCO2
                    float scope1Emissions = 1.0f;

                    // finally calculate the scope 2 product carbon footprint by multiplying the full energy consumption by the current carbon intensity
                    float scope2Emissions = (float)energyTotal * currentCarbonIntensity.data[0].intensity.actual;

                    // get scope 3 emission data from our supply chain, in this case the Carbon Reporting AAS from GE
                    float scope3Emissions = 0.0f;
                    try
                    {
                        scope3Emissions = CarbonReportingClient.ReadCarbonReportFromRemoteAAS("https://carbonreportingge.azurewebsites.net/", "0", "Energy_model_harmonized") / 1000;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Accessing Carbon Reporting AAS failed with: " + ex.Message);
                    }

                    // finally calculate our PCF
                    float pcf = scope1Emissions + scope2Emissions + scope3Emissions;

                    // persist AAS with serial number and calculated PCF
                    GenerateAASXFile(productionLineName + serialNumber.ToString(), pcf, scope1Emissions, scope2Emissions, scope3Emissions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void GenerateAASXFile(string serialNumber, float pcf, float scope1Emissions, float scope2Emissions, float scope3Emissions)
        {
            string newKey = "ProductCarbonFootprint" + "_" + serialNumber.ToString() + ".aasx";
            if (_packageService.Packages.ContainsKey(newKey))
            {
                // AASX already exists
                return;
            }

            // make a copy of our template
            _packageService.Load("ProductCarbonFootprint.template", newKey);

            // set serial number
            SubmodelElement serialNumberSME = FindSME(_packageService.Packages[newKey].Submodels[0].SubmodelElements, new Identifier("www.company.com/ids/cd/9544_4082_7091_8596"));
            ((Property)serialNumberSME).Value = serialNumber;

            // access pcf Submodel Element Collection
            SubmodelElement pcfSMEC = FindSME(_packageService.Packages[newKey].Submodels[1].SubmodelElements, new Identifier("0173-1#01-AHE716#001"));

            // set pcfTotal
            SubmodelElement pcfTotalSME = FindSME(pcfSMEC, new Identifier("0173-1#02-ABG855#001"));
            ((Property)pcfTotalSME).Value = pcf.ToString();

            // set scope 1
            SubmodelElement scope1SME = FindSME(pcfSMEC, new Identifier("www.example.com/ids/sm/Scope1Emissions"));
            ((Property)scope1SME).Value = scope1Emissions.ToString();

            // set scope 2
            SubmodelElement scope2SME = FindSME(pcfSMEC, new Identifier("www.example.com/ids/sm/Scope2Emissions"));
            ((Property)scope2SME).Value = scope2Emissions.ToString();

            // set scope 3
            SubmodelElement scope3SME = FindSME(pcfSMEC, new Identifier("www.example.com/ids/sm/Scope3Emissions"));
            ((Property)scope3SME).Value = scope3Emissions.ToString();

            // persist
            _packageService.Save(newKey);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
        }

        private SubmodelElement FindSME(SubmodelElement smeInput, Identifier semId)
        {
            if (smeInput is SubmodelElementCollection collection)
            {
                foreach (SubmodelElementWrapper smew in collection.Value)
                {
                    if (smew.SubmodelElement.SemanticId != null)
                    {
                        if (smew.SubmodelElement.SemanticId.Matches(semId))
                        {
                            return smew.SubmodelElement;
                        }
                    }
                }
            }

            return null;
        }

        private SubmodelElement FindSME(List<SubmodelElementWrapper> smewc, Identifier semId)
        {
            foreach (SubmodelElementWrapper smew in smewc)
            {
                if (smew.SubmodelElement.SemanticId != null)
                {
                    if (smew.SubmodelElement.SemanticId.Matches(semId))
                    {
                        return smew.SubmodelElement;
                    }
                }
            }

            return null;
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
