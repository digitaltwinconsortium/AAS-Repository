
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
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
            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'id_detected')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'mated')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'locked')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'blocked')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'current')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'voltage')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            RunADXQuery("let intermediateTable = AdtPropertyEvents | where Id == toscalar(GetDigitalTwinIdForUANode('', '', 'temperature')); intermediateTable | where isnotnull(SourceTimeStamp) | join intermediateTable on $left.TimeStamp == $right.TimeStamp | where Key1 == 'equipmentID' | project SourceTimeStamp, OPCUANodeValue = tostring(Value), OPCUADisplayName = Value1 | top 1 by SourceTimeStamp desc", _values);
            UpdateSMEValues();
            _values.Clear();

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private void UpdateSMEValues()
        {
            foreach (SubmodelElement sme in _dataPoints)
            {
                if (sme is Property prop)
                {
                    prop.Value = GetValue(sme).ToString();
                }
            }
        }

        public string GetValue(SubmodelElement sme)
        {
            try
            {
                foreach (Qualifier q in sme.Qualifiers)
                {
                    if (q.Type == "ADX")
                    {
                        if (_values["OPCUADisplayName"].ToString().Contains(q.Value))
                        {
                            return _values["OPCUANodeValue"].ToString();
                        }
                    }
                }

                // if we can't find it, simply return 0
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return string.Empty;
            }
        }
    }
}
