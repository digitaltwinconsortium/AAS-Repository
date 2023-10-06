
namespace AdminShell
{
    using Kusto.Data;
    using Kusto.Data.Common;
    using Kusto.Data.Net.Client;
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Diagnostics;

    public class ADXDataService : IDisposable
    {
        private ICslQueryProvider _queryProvider = null;

        public ADXDataService()
        {
            // connect to ADX cluster
            string adxClusterName = Environment.GetEnvironmentVariable("ADX_HOST");
            string adxDBName = Environment.GetEnvironmentVariable("ADX_DB");
            string aadAppKey = Environment.GetEnvironmentVariable("AAD_APPLICATION_KEY");
            string aadAppID = Environment.GetEnvironmentVariable("AAD_APPLICATION_ID");
            string aadTenant = Environment.GetEnvironmentVariable("AAD_TENANT");

            if (!string.IsNullOrEmpty(adxClusterName) && !string.IsNullOrEmpty(adxDBName) && !string.IsNullOrEmpty(aadAppID))
            {
                KustoConnectionStringBuilder connectionString;
                if (!string.IsNullOrEmpty(aadAppKey) && !string.IsNullOrEmpty(aadTenant))
                {
                    connectionString = new KustoConnectionStringBuilder(adxClusterName.Replace("https://", string.Empty), adxDBName).WithAadApplicationKeyAuthentication(aadAppID, aadAppKey, aadTenant);
                }
                else
                {
                    connectionString = new KustoConnectionStringBuilder(adxClusterName, adxDBName).WithAadUserManagedIdentity(aadAppID);
                }

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

        public void RunADXQuery(string query, ConcurrentDictionary<string, object> values, bool allowMultiRow = false)
        {
            ClientRequestProperties clientRequestProperties = new ClientRequestProperties()
            {
                ClientRequestId = Guid.NewGuid().ToString()
            };

            try
            {
                if (_queryProvider != null)
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
                                        if (!allowMultiRow)
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
                                        else
                                        {
                                            if (values.ContainsKey(reader.GetValue(i).ToString()))
                                            {
                                                values[reader.GetValue(i).ToString()] = reader.GetValue(i);
                                            }
                                            else
                                            {
                                                values.TryAdd(reader.GetValue(i).ToString(), reader.GetValue(i));
                                            }
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
