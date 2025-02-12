using Microsoft.Identity.Client;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdminShell
{
    public class DynamicsDataService : IDisposable
    {
        private HttpClient _client = null;

        private string _instanceEndpoint = string.Empty;
        private string _clientId = string.Empty;
        private string _clientPassword = string.Empty;
        private string _tenantId = string.Empty;
        private string _currentBearerToken = string.Empty;

        public DynamicsDataService()
        {
            _client = new();
            _instanceEndpoint = Environment.GetEnvironmentVariable("DYNAMICS_ENDPOINT_URL");
            _clientId = Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_ID");
            _clientPassword = Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_PASSWORD");
            _tenantId = Environment.GetEnvironmentVariable("DYNAMICS_TENANT");
            _currentBearerToken = Environment.GetEnvironmentVariable("DYNAMICS_BEARER_TOKEN");

            // optionally login
            if (!string.IsNullOrEmpty(_instanceEndpoint) && string.IsNullOrEmpty(_currentBearerToken))
            {
                string newToken = GetBearerToken(_instanceEndpoint).GetAwaiter().GetResult();
                if (newToken != null)
                {
                    _currentBearerToken = newToken;
                }
            }
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }

        private async Task<string> GetBearerToken(string endPoint)
        {
            try
            {

                // Step 1: Get Entra token
                string authority = $"https://login.microsoftonline.com/{_tenantId}";

                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_clientId)
                    .WithClientSecret(_clientPassword)
                    .WithAuthority(new Uri(authority))
                    .Build();

                string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
                AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                Debug.WriteLine($"Token: {result.AccessToken}");

                // Step 2: Get token
                // TODO
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void RunDynamicsQuery(string query, ConcurrentDictionary<string, object> values)
        {
            try
            {
                var response = _client.GetAsync(query).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var dataSet = new DataSet();
                    dataSet.ReadXml(new System.IO.StringReader(content));
                    if (dataSet.Tables.Count > 0)
                    {
                        var table = dataSet.Tables[0];
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                values.TryAdd(column.ColumnName, row[column]);
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
