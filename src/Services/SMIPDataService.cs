
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AdminShell
{
    /// <summary>
    /// Support for accessing the SMIP service via GraphQL. In support of the
    /// accompanying product carbon footprint calculation example.
    /// </summary>
    public class SMIPDataService : IDisposable
    {
        private string _instanceGraphQLEndpoint = string.Empty;
        private string _clientId = string.Empty;
        private string _clientPassword = string.Empty;
        private string _userName = string.Empty;
        private string _role = string.Empty;
        private string _currentBearerToken = string.Empty;

        public SMIPDataService()
        {
            _instanceGraphQLEndpoint = Environment.GetEnvironmentVariable("SMIP_GRAPHQL_ENDPOINT_URL");
            _clientId = Environment.GetEnvironmentVariable("SMIP_CLIENT_ID");
            _clientPassword = Environment.GetEnvironmentVariable("SMIP_CLIENT_PASSWORD");
            _userName = Environment.GetEnvironmentVariable("SMIP_USERNAME");
            _role = Environment.GetEnvironmentVariable("SMIP_CLIENT_ROLE");
            _currentBearerToken = Environment.GetEnvironmentVariable("SMIP_BEARER_TOKEN");

            // optionally login
            if (!string.IsNullOrEmpty(_instanceGraphQLEndpoint) && string.IsNullOrEmpty(_currentBearerToken))
            {
                string newToken = GetBearerToken().GetAwaiter().GetResult();
                if (newToken != null)
                {
                    _currentBearerToken = newToken;
                }
            }
        }

        public void Dispose()
        {
        }

        public string RunSMIPQuery(string query)
        {
            string smpResponse = string.Empty;
            try
            {
                smpResponse = PerformGraphQLRequest(query, _instanceGraphQLEndpoint, _currentBearerToken).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("forbidden") || ex.Message.ToLower().Contains("unauthorized") || ex.Message.ToLower().Contains("badrequest"))
                {
                    Debug.WriteLine("Bearer Token expired! Attempting to retrieve a new bearer token.");

                    // re-authorize
                    _currentBearerToken = GetBearerToken().GetAwaiter().GetResult();

                    // re-try our data request, using the updated bearer token
                    smpResponse = PerformGraphQLRequest(query, _instanceGraphQLEndpoint, _currentBearerToken).GetAwaiter().GetResult();
                }
                else
                {
                    Console.WriteLine("RunSMIPQuery: " + ex.Message);
                }
            }

            return smpResponse;
        }

        private async Task<string> PerformGraphQLRequest(string content, string endPoint, string bearerToken)
        {
            var graphQLClient = new GraphQLHttpClient(endPoint, new NewtonsoftJsonSerializer());

            GraphQLRequest dataRequest = new GraphQLRequest() { Query = content };
            graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            GraphQLResponse<JObject> dataResponse = await graphQLClient.SendQueryAsync<JObject>(dataRequest).ConfigureAwait(false);
            JObject data = dataResponse.Data;

            return data.ToString(Formatting.Indented);
        }

        private async Task<string> GetBearerToken()
        {
            try
            {
                var graphQLClient = new GraphQLHttpClient(_instanceGraphQLEndpoint, new NewtonsoftJsonSerializer());

                // Step 1: Request a challenge
                string authRequestQuery = @$"
                mutation authRequest {{
                  authenticationRequest(input: {{authenticator: ""{_clientId}"", role: ""{_role}"", userName: ""{_userName}""}}) {{
                    jwtRequest {{
                      challenge
                      message
                    }}
                  }}
                }}";

                GraphQLRequest authRequest = new GraphQLRequest() { Query = authRequestQuery };
                GraphQLResponse<JObject> authResponse = await graphQLClient.SendQueryAsync<JObject>(authRequest).ConfigureAwait(false);
                string challenge = authResponse.Data["authenticationRequest"]["jwtRequest"]["challenge"].Value<string>();
                Debug.WriteLine($"Challenge received: {challenge}");

                // Step 2: Get token
                var authValidationQuery = @$"
                mutation authValidation {{
                  authenticationValidation(input: {{authenticator: ""{_clientId}"", signedChallenge: ""{challenge}|{_clientPassword}""}}) {{
                    jwtClaim
                  }}
                }}";

                GraphQLRequest validationRequest = new GraphQLRequest() { Query = authValidationQuery };
                GraphQLResponse<JObject> validationQLResponse = await graphQLClient.SendQueryAsync<JObject>(validationRequest).ConfigureAwait(false);

                return validationQLResponse.Data["authenticationValidation"]["jwtClaim"].Value<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetBearerToken: " + ex.Message);
                return null;
            }
        }
    }
}
