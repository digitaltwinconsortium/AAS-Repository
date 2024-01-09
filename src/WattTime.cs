
namespace AdminShell
{
    using Kusto.Cloud.Platform.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class WattTime
    {
        public static async Task<CarbonIntensityQueryResult> GetCarbonIntensity(string latitude, string longitude)
        {
            string watttimeUser = Environment.GetEnvironmentVariable("WATTTIME_USER");
            if (!string.IsNullOrEmpty(watttimeUser))
            {
                try
                {
                    // read current carbon intensity from the WattTime service at https://api2.watttime.org/v2
                    HttpClient webClient = new HttpClient();

                    // login
                    string watttimePassword = Environment.GetEnvironmentVariable("WATTTIME_PASSWORD");
                    webClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(watttimeUser + ":" + watttimePassword)));

                    HttpResponseMessage response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api.watttime.org/login")).ConfigureAwait(false);
                    string token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    token = token.TrimStart("{\"token\":\"").TrimEnd("\"}");

                    // now use bearer token returned previously
                    webClient.DefaultRequestHeaders.Remove("Authorization");
                    webClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // determine grid region
                    response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api.watttime.org/v3/region-from-loc?latitude=" + latitude + "&longitude=" + longitude + "&signal_type=co2_moer")).ConfigureAwait(false);
                    RegionQueryResult regionResult = JsonConvert.DeserializeObject<RegionQueryResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                    // get the carbon intensity
                    response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api.watttime.org/v3/forecast?region=" + regionResult.region + "&signal_type=co2_moer&horizon_hours=0")).ConfigureAwait(false);
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    WattTimeQueryResult result = JsonConvert.DeserializeObject<WattTimeQueryResult>(content);

                    CarbonIntensityQueryResult intensity = new()
                    {
                        data = new CarbonData[1]
                    };

                    // convert from lbs/MWh to g/KWh
                    intensity.data[0] = new CarbonData
                    {
                        intensity = new CarbonIntensity()
                        {
                            actual = (int)(result.data[0].value * 453.592f / 1000.0f)
                        }
                    };

                    return intensity;
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    return null;
                }
            }

            return null;
        }
    }
}
