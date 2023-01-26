
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class CarbonReportingClient
    {
        public static float ReadCarbonReportFromRemoteAAS(string aasServerAddress, string aasId, string smIdShort)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(aasServerAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Submodel submodel = null;
            var path = $"/aas/{aasId}/Submodels/{smIdShort}/complete";
            HttpResponseMessage response = client.GetAsync(path).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                using (TextReader reader = new StringReader(json))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    submodel = (Submodel)serializer.Deserialize(reader, typeof(Submodel));
                }
            }

            // access electrical energy
            SubmodelElement smcEe = FindSME(submodel?.SubmodelElements, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/electrical-energy/1/0"));

            // access CO2 per 1 minute
            SubmodelElement smcPhase = FindSME(smcEe, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/electrical-total/1/0"));

            // get Value
            string value = ((Property)FindSME(smcPhase, new Identifier("https://admin-shell.io/sandbox/idta/carbon-reporting/cd/co2-equivalent/1/0"))).Value;
            if (value != null && float.TryParse(value, out float f))
            {
                return f;
            }
            else
            {
                return 0.0f;
            }
        }

        private static SubmodelElement FindSME(SubmodelElement smeInput, Identifier semId)
        {
            if (smeInput is SubmodelElementCollection collection)
            {
                foreach (SubmodelElementWrapper smew in collection.Value)
                {
                    if (smew.SubmodelElement.SemanticId != null)
                    {
                        if (smew.SubmodelElement.SemanticId.Matches(semId))
                        {
                            return smew.SubmodelElement;
                        }
                    }
                }
            }

            return null;
        }

        private static SubmodelElement FindSME(List<SubmodelElementWrapper> smewc, Identifier semId)
        {
            foreach (SubmodelElementWrapper smew in smewc)
            {
                if (smew.SubmodelElement.SemanticId != null)
                {
                    if (smew.SubmodelElement.SemanticId.Matches(semId))
                    {
                        return smew.SubmodelElement;
                    }
                }
            }

            return null;
        }
    }
}
