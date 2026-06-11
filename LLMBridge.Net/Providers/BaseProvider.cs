using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LLMBridge.Net.Exceptions;


namespace LLMBridge.Net.Providers
{
    public abstract class BaseProvider
    {
        protected readonly HttpClient HttpClient;
        protected readonly JsonSerializerOptions JsonOptions;

        protected BaseProvider()
        {
            HttpClient = new HttpClient();

            JsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected void SetBearerToken(string apiKey)
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        protected StringContent CreateJsonContent(object payload)
        {
            var json = JsonSerializer.Serialize(
                payload,
                JsonOptions);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<string> ReadResponseContentAsync(
            HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task EnsureSuccessAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var error =
                await response.Content.ReadAsStringAsync();

            throw new LLMBridgeException(
                $"API Error ({(int)response.StatusCode}) : {error}");
        }

        protected async Task<T?> DeserializeAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json,JsonOptions);
        }
    }
}