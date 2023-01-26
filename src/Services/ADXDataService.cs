
namespace AdminShell
{
    using Kusto.Data;
    using Kusto.Data.Common;
    using Kusto.Data.Net.Client;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    public class ADXDataService : IDisposable
    {
        private ICslQueryProvider _queryProvider = null;

        protected List<SubmodelElement> _dataPoints = new();

        protected readonly AASXPackageService _packageService;

        public ADXDataService(AASXPackageService packageService)
        {
            _packageService = packageService;

            // retrieve our ADX-tagged data points from all loaded AASes
            foreach (AssetAdministrationShellEnvironment env in packageService.Packages.Values)
            {
                foreach (Submodel sm in env.Submodels)
                {
                    foreach (SubmodelElementWrapper smew in sm.SubmodelElements)
                    {
                        CheckForADXDataPointInSME(smew.SubmodelElement);
                    }
                }
            }

            // connect to ADX cluster
            string adxClusterName = Environment.GetEnvironmentVariable("ADX_HOST");
            string adxDBName = Environment.GetEnvironmentVariable("ADX_DB");
            string aadAppKey = Environment.GetEnvironmentVariable("AAD_APPLICATION_KEY");
            string aadAppID = Environment.GetEnvironmentVariable("AAD_APPLICATION_ID");
            string aadTenant = Environment.GetEnvironmentVariable("AAD_TENANT");

            if (!string.IsNullOrEmpty(aadAppKey) && !string.IsNullOrEmpty(adxClusterName) && !string.IsNullOrEmpty(adxDBName) && !string.IsNullOrEmpty(aadAppID) && !string.IsNullOrEmpty(aadTenant))
            {
                KustoConnectionStringBuilder connectionString = new KustoConnectionStringBuilder(adxClusterName, adxDBName).WithAadApplicationKeyAuthentication(aadAppID, aadAppKey, aadTenant);
                _queryProvider = KustoClientFactory.CreateCslQueryProvider(connectionString);
            }
        }

        public void Dispose()
        {
            if (_queryProvider != null)
            {
                _queryProvider.Dispose();
                _queryProvider = null;
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
                if ((q.Type == "ADX") && !_dataPoints.Contains(sme))
                {
                    _dataPoints.Add(sme);
                    break;
                }
            }
        }

        protected void RunADXQuery(string query, ConcurrentDictionary<string, object> values)
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
    }
}
