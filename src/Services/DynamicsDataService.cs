using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        private string _environmentId = string.Empty;

        public DynamicsDataService()
        {
            _client = new();

            _instanceEndpoint = Environment.GetEnvironmentVariable("DYNAMICS_ENDPOINT_URL");
            _clientId = Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_ID");
            _clientPassword = Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_PASSWORD");
            _tenantId = Environment.GetEnvironmentVariable("DYNAMICS_TENANT_ID");
            _environmentId = Environment.GetEnvironmentVariable("DYNAMICS_ENVIRONMENT_ID");

            if (!string.IsNullOrEmpty(_instanceEndpoint))
            {
                Authorize().GetAwaiter().GetResult();
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

        private async Task Authorize()
        {
            try
            {
                // Step 1: Get Entra token
                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_clientId)
                    .WithClientSecret(_clientPassword)
                    .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token"))
                    .Build();

                AuthenticationResult result = await app.AcquireTokenForClient(["0cdb527f-a8d1-4bf8-9436-b352c68682b2/.default"]).ExecuteAsync().ConfigureAwait(false);
                Debug.WriteLine($"Token: {result.AccessToken}");

                // Step 2: Get bearer token
                HttpResponseMessage response = _client.Send(
                    new HttpRequestMessage(
                        HttpMethod.Post,
                        "https://securityservice.operations365.dynamics.com/token") {
                            Content = new StringContent(
                                "{ \"grant_type\": \"client_credentials\", \"client_assertion_type\": \"aad_app\", \"client_assertion\": \"" + result.AccessToken + "\", \"scope\": \"https://traceabilityservice.operations365.dynamics.com/.default\", \"context\": \"" + _environmentId + "\", \"context_type\": \"finops-env\" }",
                                Encoding.UTF8,
                                "application/json"
                            )
                        }
                );

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                DynamicsBearerTokenResponse tokenResponse = JsonConvert.DeserializeObject<DynamicsBearerTokenResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.accessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Authorize: " + ex.Message);
            }
        }

        public async Task<DynamicsQueryResponse> RunDynamicsQuery(DynamicsQuery query)
        {
            if (!string.IsNullOrEmpty(_instanceEndpoint))
            {
                try
                {
                    string url = _instanceEndpoint + "/api/environments/" + _environmentId + "/traces/Query";
                    HttpResponseMessage response = _client.Send(
                        new HttpRequestMessage(
                            HttpMethod.Post,
                            url
                            )
                        {
                            Content = new StringContent(
                                    JsonConvert.SerializeObject(query),
                                    Encoding.UTF8,
                                    "application/json"
                            )
                        }
                    );

                    if ((response.StatusCode == HttpStatusCode.Unauthorized) || (response.StatusCode == HttpStatusCode.Forbidden))
                    {
                        Debug.WriteLine("Bearer Token expired! Attempting to retrieve a new barer token.");

                        // re-authorize
                        await Authorize().ConfigureAwait(false);

                        // re-try our data request, using the updated bearer token
                        response = _client.Send(
                            new HttpRequestMessage(
                                HttpMethod.Post,
                                _instanceEndpoint + "/api/environment/" + _environmentId + "/traces/Query")
                            {
                                Content = new StringContent(
                                        JsonConvert.SerializeObject(query),
                                        Encoding.UTF8,
                                        "application/json"
                                )
                            }
                        );
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response.StatusCode.ToString());
                    }

                    return JsonConvert.DeserializeObject<DynamicsQueryResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("RunDynamicsQuery: " + ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
