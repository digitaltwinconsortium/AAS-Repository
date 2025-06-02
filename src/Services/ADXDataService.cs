
using Azure.Identity;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;

namespace AdminShell
{
    /// <summary>
    /// ADXDataService - Uses Kusto libraries to 
    /// -- Connect to an Azure Data Explorer (ADX) cluster.
    /// -- Run Kusto queries (KQL) against the data stored in ADX.
    /// </summary>
    public class ADXDataService : IDisposable
    {
        private ICslQueryProvider _queryProvider = null;

        public ADXDataService()
        {
            // connect to ADX cluster
            string adxClusterName = Environment.GetEnvironmentVariable("ADX_HOST");
            string adxDBName = Environment.GetEnvironmentVariable("ADX_DB");
            string aadAppID = Environment.GetEnvironmentVariable("AAD_APPLICATION_ID");

            if (!string.IsNullOrEmpty(adxClusterName) && !string.IsNullOrEmpty(adxDBName))
            {
                KustoConnectionStringBuilder connectionString;
                if (string.IsNullOrEmpty(aadAppID))
                {
                    connectionString = new KustoConnectionStringBuilder(adxClusterName, adxDBName)
                        .WithAadAzureTokenCredentialsAuthentication(new DefaultAzureCredential());
                }
                else
                {
                    connectionString = new KustoConnectionStringBuilder(adxClusterName, adxDBName)
                        .WithAadUserManagedIdentity(aadAppID);
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
                Console.WriteLine("RunADXQuery: " + ex.Message);
            }
        }
    }
}
