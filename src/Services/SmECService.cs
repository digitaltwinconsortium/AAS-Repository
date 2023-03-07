
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class SmECService : ADXDataService
    {
        private Timer _queryTimer;
        private ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();

        private readonly ILogger _logger;

        public SmECService(ILoggerFactory logger, AASXPackageService packageService)
        : base(packageService)
        {
            _logger = logger.CreateLogger("CarbonReportingService");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SMEC_REPORTING")))
            {
                RunADXQuery("AdtPropertyEvents | where Key == 'equipmentID' | distinct tostring(Value)", _values, true);

                CreateSMEValues(_values);

                _values.Clear();

                _queryTimer = new Timer(RunQuerys);

                string adxQueryInterval = Environment.GetEnvironmentVariable("ADX_QUERY_INTERVAL");
                if (adxQueryInterval != null && int.TryParse(adxQueryInterval, out int interval))
                {
                    _queryTimer.Change(interval, interval);
                }
                else
                {
                    // default to 15s interval
                    _queryTimer.Change(5000, 150000);
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
            // read the row from our OPC UA telemetry table
            foreach (Property prop in _dataPoints)
            {
                RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', '" + prop.IdShort + "')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);

                UpdateSMEValues();

                _values.Clear();
            }

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private void CreateSMEValues(ConcurrentDictionary<string, object> values)
        {
            // retrieve our OperationalData Submodel
            foreach (AssetAdministrationShellEnvironment env in _packageService.Packages.Values)
            {
                foreach (Submodel sm in env.Submodels)
                {
                    if (sm.IdShort == "OperationalData")
                    {
                        List<string> keys = values.Keys.ToList();
                        keys.Sort();
                        foreach (string dataItem in keys)
                        {
                            // create a wrapper and submodel element per data item
                            Property sme = new() { IdShort = dataItem };
                            _dataPoints.Add(sme);
                            sm.SubmodelElements.Add(new SubmodelElementWrapper() { SubmodelElement = sme });
                        }
                    }
                }
            }
        }

        private void UpdateSMEValues()
        {
            foreach (Property prop in _dataPoints)
            {
                try
                {
                    if (_values["OPCUADisplayName"].ToString().Contains(prop.IdShort))
                    {
                        prop.Value = _values["OPCUANodeValue"].ToString();
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
