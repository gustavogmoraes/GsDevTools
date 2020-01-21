using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GsDevTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GSDevTools
{
    public static class ServicoBitLy
    {
        public const string BearerToken = "97a43b343426a0ec2ea7e370416682d6a3b286f8";

        private const string BitLyBaseUrl = "https://api-ssl.bitly.com/v4/";

        private static HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BitLyBaseUrl),
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            return client;
        }

        public static string GetGroupId()
        {
            using (var httpClient = GetClient())
            {
                var result = JsonConvert.DeserializeObject<dynamic>(httpClient.GetAsync("groups").Result.Content.ReadAsStringAsync().Result);
                return result.groups[0].guid;
            }
        }

        public static string EncurteUrl(string url)
        {
            using (var httpClient = GetClient())
            {
                var response = httpClient.PostAsync("shorten", GetStringContent(new
                {
                    group_guid = GetGroupId(),
                    long_url = url
                })).Result;

                return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK
                    ? response.Content.GetObjectFromContent().link
                    : null;
            }
        }

        public static StringContent GetStringContent(this object obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj);

            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return contentString;
        }

        public static T GetObjectFromContent<T>(this HttpContent httpContent)
            where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(httpContent.ReadAsStringAsync().Result);
        }

        public static dynamic GetObjectFromContent(this HttpContent httpContent)
        {
            return JsonConvert.DeserializeObject<dynamic>(httpContent.ReadAsStringAsync().Result);
        }

        public static void TryShortLink(string clipBoardText)
        {
            if (string.IsNullOrEmpty(clipBoardText)) return;
            var trimmed = clipBoardText.Trim();
            if (trimmed.IsValidLink() && trimmed.Length > 100)
            {
                Program.InstanciaPrincipal.ShortLink(clipBoardText);
            }
        }

        private static bool IsValidLink(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && 
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
