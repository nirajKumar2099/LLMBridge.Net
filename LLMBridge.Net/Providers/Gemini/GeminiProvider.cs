using System.Net.Http.Json;
using System.Text.Json;
using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Models;

namespace LLMBridge.Net.Providers.Gemini;

public sealed class GeminiProvider : BaseProvider, ILLMProvider
{
    private readonly LLMOptions _options;
    private readonly HttpClient _httpClient;

    public GeminiProvider(LLMOptions options)
    {
        _options = options;
        _httpClient = new HttpClient();
    }

    public async Task<LLMResponse> GenerateAsync(
        LLMRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{request.Model}:generateContent?key={_options.ApiKey}";

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = request.Prompt
                            }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                url,
                body,
                cancellationToken);

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(json);

            var content =
                doc.RootElement
                   .GetProperty("candidates")[0]
                   .GetProperty("content")
                   .GetProperty("parts")[0]
                   .GetProperty("text")
                   .GetString();

            return new LLMResponse
            {
                Success = true,
                Content = content ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            return new LLMResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}