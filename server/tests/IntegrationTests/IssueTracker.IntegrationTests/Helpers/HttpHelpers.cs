using Newtonsoft.Json;
using System.Text;

namespace IssueTracker.ApplicationTests.Helpers
{
    public static class HttpHelpers
    {
        public static T GetParsedBody<T>(HttpResponseMessage response)
        {
            var respBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(respBody);
        }

        public static HttpContent CreatetHttpContent(object body, Encoding encoding = null, string mediaType = "application/json")
        {
          return new StringContent( JsonConvert.SerializeObject(body), encoding ?? Encoding.UTF8, mediaType);
        }

        public static string ToStringContentType(HttpResponseMessage response)
        {
            return response.Content.Headers.ContentType?.ToString();
        }
    }
}
