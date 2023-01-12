
namespace AdminShell
{
    using Kusto.Cloud.Platform.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProductCarbonFootprintService : ADXDataService
    {
        private Timer _timer = new Timer(CalculatePCF, null, Timeout.Infinite, Timeout.Infinite);
        private static ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();
        private static CarbonIntensityQueryResult _currentIntensity = null;

        public ProductCarbonFootprintService(AASXPackageService packageService)
        : base(packageService)
        {
            _timer.Change(15000, 15000);
        }

        public new void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            base.Dispose();
        }

        private static void CalculatePCF(object state)
        {
            // TODO: RunADXQuery("", _values);

            // TODO: await GetCarbonIntensity().ConfigureAwait(false);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
        }

        private static async Task GetCarbonIntensity(string latitude, string longitude)
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

                    HttpResponseMessage response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/login")).ConfigureAwait(false);
                    string token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    token = token.TrimStart("{\"token\":\"").TrimEnd("\"}");

                    // now use bearer token returned previously
                    webClient.DefaultRequestHeaders.Remove("Authorization");
                    webClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // determine grid region
                    response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/ba-from-loc?latitude=" + latitude + "&longitude=" + longitude)).ConfigureAwait(false);
                    RegionQueryResult region = JsonConvert.DeserializeObject<RegionQueryResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                    // get the carbon intensity
                    response = await webClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://api2.watttime.org/v2/index?ba=" + region.abbrev + "&style=moer")).ConfigureAwait(false);
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    WattTimeQueryResult result = JsonConvert.DeserializeObject<WattTimeQueryResult>(content);

                    _currentIntensity = new CarbonIntensityQueryResult()
                    {
                        data = new CarbonData[1]
                    };

                    // convert from lbs/MWh to g/KWh
                    _currentIntensity.data[0] = new CarbonData();
                    _currentIntensity.data[0].intensity = new CarbonIntensity()
                    {
                        actual = (int)(result.moer * 453.592f / 1000.0f)
                    };
                }
                catch (Exception)
                {
                    // do nothing
                }
            }
        }
    }
}
