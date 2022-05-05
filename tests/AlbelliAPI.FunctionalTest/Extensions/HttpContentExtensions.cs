using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlbelliAPI.FunctionalTest.Extensions;

public static class HttpContentExtensions
{
    internal static async Task<T> GetAsync<T>(this HttpContent content)
    {
        var json = await content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json);
    }

    internal static Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string requestUri, T model)
    {
        var json = JsonSerializer.Serialize<T>(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return client.PostAsync(requestUri, content);
    }
}
