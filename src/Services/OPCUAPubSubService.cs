
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class OPCUAPubSubService : IDisposable
    {
        private Timer _queryTimer;
        private ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();
        private List<string> _dataPoints = new List<string>();

        private readonly ILogger _logger;
        private readonly ADXDataService _adxDataService;
        private readonly AssetAdministrationShellEnvironmentService _envService;

        public OPCUAPubSubService(ILoggerFactory logger, ADXDataService adxDataService, AssetAdministrationShellEnvironmentService envService)
        {
            _logger = logger.CreateLogger("OPCUAPubSubService");
            _adxDataService = adxDataService;
            _envService = envService;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OPCUA_REPORTING")))
            {
                _adxDataService.RunADXQuery("AdtPropertyEvents | where Key == 'equipmentID' | distinct tostring(Value)", _values, true);

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

        public void Dispose()
        {
            if (_queryTimer != null)
            {
                _queryTimer.Dispose();
            }
        }

        private void RunQuerys(object state)
        {
            // stop timer while queries are executing
            _queryTimer.Change(Timeout.Infinite, Timeout.Infinite);

            // read the row from our OPC UA telemetry table
            foreach (string id in _dataPoints)
            {
                _adxDataService.RunADXQuery("opcua_metadata_lkv | where Name contains '" + id + "' | join kind = inner(opcua_telemetry) on DataSetWriterID | project Timestamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Name | top 1 by Timestamp desc", _values);

                UpdateSMEValues();

                _values.Clear();
            }

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
            string id = _envService.GetEnv().AssetAdministrationShells[0].Identification.Id;

            foreach (Submodel sm in _envService.GetEnv().Submodels)
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
                            // create a submodel element per data item
                            bool smeExists = false;
                            foreach (SubmodelElement existingSME in sm.SubmodelElements)
                            {
                                if (existingSME.IdShort == idShort)
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

                                sm.SubmodelElements.Add(sme);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateSMEValues()
        {
            string id = _envService.GetEnv().AssetAdministrationShells[0].Identification.Id;

            foreach (Submodel sm in _envService.GetEnv().Submodels)
            {
                if (sm.IdShort == "OperationalData")
                {
                    foreach (SubmodelElement sme in sm.SubmodelElements)
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

                            Property prop = (Property)sme;
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

        private void HandleHartingSmEC(Property prop)
        {
            if (prop.Value == "Object")
            {
                _logger.LogInformation("SmEC: Not plugged in!");

                foreach (Submodel sm in _envService.GetEnv().Submodels)
                {
                    if (sm.IdShort == "BOM")
                    {
                        foreach (SubmodelElement sme in ((Entity)sm.SubmodelElements[0]).Statements)
                        {
                            if (sme.IdShort.StartsWith("SmEC_"))
                            {
                                ((Entity)sm.SubmodelElements[0]).Statements.Remove(sme);
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
                foreach (Submodel sm in _envService.GetEnv().Submodels)
                {
                    if (sm.IdShort == "BOM")
                    {
                        foreach (SubmodelElement sme in ((Entity)sm.SubmodelElements[0]).Statements)
                        {
                            if (sme.IdShort == "SmEC")
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

                    foreach (Submodel sm in _envService.GetEnv().Submodels)
                    {
                        if (sm.IdShort == "BOM")
                        {
                            ((Entity)sm.SubmodelElements[0]).Statements.Add(sme);
                        }
                    }
                }
            }
        }
    }
}
