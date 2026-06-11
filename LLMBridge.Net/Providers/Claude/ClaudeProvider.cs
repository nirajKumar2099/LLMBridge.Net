using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Models;

namespace LLMBridge.Net.Providers.Anthropic;

public sealed class ClaudeProvider : BaseProvider, ILLMProvider
{
    private readonly LLMOptions _options;

    public ClaudeProvider(LLMOptions options)
    {
        _options = options;
    }

    public async Task<LLMResponse> GenerateAsync(
        LLMRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new ArgumentException("Anthropic ApiKey is required.");

            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add(
                "x-api-key",
                _options.ApiKey);

            httpClient.DefaultRequestHeaders.Add(
                "anthropic-version",
                "2023-06-01");

            var payload = new
            {
                model = request.Model,
                max_tokens = request.Options?.MaxTokens ?? 1024,
                temperature = request.Options?.Temperature ?? 0.7,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = request.Prompt
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(
                "https://api.anthropic.com/v1/messages",
                content,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(cancellationToken);

            response.EnsureSuccessStatusCode();

            using var document =
                JsonDocument.Parse(responseBody);

            var text =
                document.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();

            int? inputTokens = null;
            int? outputTokens = null;

            if (document.RootElement.TryGetProperty("usage", out var usage))
            {
                inputTokens = usage.GetProperty("input_tokens").GetInt32();
                outputTokens = usage.GetProperty("output_tokens").GetInt32();
            }

            return new LLMResponse
            {
                Success = true,
                Content = text ?? string.Empty,
                PromptTokens = inputTokens,
                CompletionTokens = outputTokens,
                TotalTokens = (inputTokens ?? 0) + (outputTokens ?? 0)
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(ex);
        }
    }

    private LLMResponse CreateErrorResponse(Exception ex)
    {
        throw new NotImplementedException();
    }
}