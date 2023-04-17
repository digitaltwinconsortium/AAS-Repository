
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class OPCUAPubSubService : ADXDataService
    {
        private Timer _queryTimer;
        private ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();

        private List<string> _dataPoints = new List<string>();

        private readonly ILogger _logger;

        public OPCUAPubSubService(ILoggerFactory logger, AASXPackageService packageService)
        : base(packageService)
        {
            _logger = logger.CreateLogger("OPCUAPubSubService");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OPCUA_REPORTING")))
            {
                RunADXQuery("AdtPropertyEvents | where Key == 'equipmentID' | distinct tostring(Value)", _values, true);

                CreateSMEs(_values);

                _values.Clear();

                _queryTimer = new Timer(RunQuerys);

                string adxQueryInterval = Environment.GetEnvironmentVariable("ADX_QUERY_INTERVAL");
                if (adxQueryInterval != null && int.TryParse(adxQueryInterval, out int interval))
                {
                    _queryTimer.Change(interval, interval);
                }
                else
                {
                    // default to 3s interval
                    _queryTimer.Change(3000, 3000);
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

        private void RunQuerys(object state)
        {
            // stop timer while queries are executing
            _queryTimer.Change(Timeout.Infinite, Timeout.Infinite);

            // read the row from our OPC UA telemetry table
            foreach (string id in _dataPoints)
            {
                RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', '" + id + "')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);

                UpdateSMEValues();

                _values.Clear();
            }

            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                _packageService.Save(package.Key);
            }

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);

            // restart the timer
            string adxQueryInterval = Environment.GetEnvironmentVariable("ADX_QUERY_INTERVAL");
            if (adxQueryInterval != null && int.TryParse(adxQueryInterval, out int interval))
            {
                _queryTimer.Change(interval, interval);
            }
            else
            {
                // default to 3s interval
                _queryTimer.Change(3000, 3000);
            }
        }

        private void CreateSMEs(ConcurrentDictionary<string, object> values)
        {
            // retrieve our OperationalData Submodel
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                string id = package.Value.AssetAdministrationShells[0].Identification.Id;

                foreach (Submodel sm in package.Value.Submodels)
                {
                    if (sm.IdShort == "OperationalData")
                    {
                        List<string> keys = values.Keys.ToList();
                        keys.Sort();
                        foreach (string dataItem in keys)
                        {
                            string lookup = id.Substring(id.LastIndexOf('/') + 1);
                            string[] lookupParts = lookup.Split('_');
                            string aasIDLookup = lookupParts[0];
                            if (lookupParts.Count() > 1)
                            {
                                aasIDLookup = lookupParts[1];
                            }

                            string[] dataItemParts = dataItem.Split(';');
                            string aasID = dataItemParts[0];
                            string idShort = dataItemParts[1];

                            if (aasIDLookup == aasID)
                            {
                                // create a wrapper and submodel element per data item
                                bool smeExists = false;
                                foreach (SubmodelElementWrapper existingSMEW in sm.SubmodelElements)
                                {
                                    if (existingSMEW.SubmodelElement.IdShort == idShort)
                                    {
                                        smeExists = true;
                                        break;
                                    }
                                }

                                _dataPoints.Add(dataItem);

                                if (!smeExists)
                                {
                                    Property sme = new()
                                    {
                                        IdShort = idShort,
                                        ValueType = "string"
                                    };

                                    SubmodelElementWrapper smew = new() { SubmodelElement = sme };

                                    sm.SubmodelElements.Add(smew);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateSMEValues()
        {
            foreach (AssetAdministrationShellEnvironment env in _packageService.Packages.Values)
            {
                string id = env.AssetAdministrationShells[0].Identification.Id;

                foreach (Submodel sm in env.Submodels)
                {
                    if (sm.IdShort == "OperationalData")
                    {
                        foreach (SubmodelElementWrapper smew in sm.SubmodelElements)
                        {
                            try
                            {
                                string lookup = id.Substring(id.LastIndexOf('/') + 1);
                                string[] lookupParts = lookup.Split('_');
                                string aasIDLookup = lookupParts[0];
                                if (lookupParts.Count() > 1)
                                {
                                    aasIDLookup = lookupParts[1];
                                }

                                string[] valueParts = _values["OPCUADisplayName"].ToString().Split(';');
                                string aasID = valueParts[0];
                                string idShort = valueParts[1];

                                Property prop = (Property)smew.SubmodelElement;
                                if ((prop.IdShort == idShort) && (aasIDLookup == aasID))
                                {
                                    prop.Value = _values["OPCUANodeValue"].ToString();

                                    // special case for Harting HMI demonstrator
                                    if ((aasID == "SmEC2") && (idShort == "id_detected"))
                                    {
                                        HandleHartingSmEC(prop);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void HandleHartingSmEC(Property prop)
        {
            if (prop.Value == "Object")
            {
                _logger.LogInformation("SmEC: Not plugged in!");

                foreach (Submodel sm in _packageService.Packages["/app/./gebhardt_agv.aasx"].Submodels)
                {
                    if (sm.IdShort == "BOM")
                    {
                        foreach (SubmodelElementWrapper smew in ((Entity)sm.SubmodelElements[0].SubmodelElement).Statements)
                        {
                            if (smew.SubmodelElement.IdShort.StartsWith("SmEC_"))
                            {
                                ((Entity)sm.SubmodelElements[0].SubmodelElement).Statements.Remove(smew);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                _logger.LogInformation("SmEC: Plugged in to " + prop.Value + "!");

                bool found = false;
                foreach (Submodel sm in _packageService.Packages["/app/./gebhardt_agv.aasx"].Submodels)
                {
                    if (sm.IdShort == "BOM")
                    {
                        foreach (SubmodelElementWrapper smew in ((Entity)sm.SubmodelElements[0].SubmodelElement).Statements)
                        {
                            if (smew.SubmodelElement.IdShort == "SmEC")
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (!found)
                {
                    Entity sme = new()
                    {
                        IdShort = "SmEC_" + prop.IdShort,
                        SemanticId = new SemanticId()
                    };

                    sme.SemanticId.Keys = new List<Key>();
                    sme.SemanticId.Type = KeyElements.GlobalReference;
                    sme.SemanticId.Keys.Add(new Key("GlobalReference", "https://admin-shell.io/idta/HierachicalStructures/Node/1/0"));

                    SubmodelElementWrapper smew = new() { SubmodelElement = sme };

                    foreach (Submodel sm in _packageService.Packages["/app/./gebhardt_agv.aasx"].Submodels)
                    {
                        if (sm.IdShort == "BOM")
                        {
                            ((Entity)sm.SubmodelElements[0].SubmodelElement).Statements.Add(smew);
                        }
                    }
                }
            }
        }
    }
}
