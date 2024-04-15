using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TomatoGame.Service.Utils
{
    public class WebApiClient
    {
        public HttpClient CreateHttpClient()
        {
            ServicePointManager.SecurityProtocol |=
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var client = new HttpClient();
            // Set default request headers, if needed
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
