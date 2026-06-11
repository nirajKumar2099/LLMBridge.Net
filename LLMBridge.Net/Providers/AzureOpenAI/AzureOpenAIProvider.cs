using System.Text.Json;
using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Models;

namespace LLMBridge.Net.Providers.AzureOpenAI;

public sealed class AzureOpenAIProvider : BaseProvider, ILLMProvider
{
    private readonly LLMOptions _options;

    public AzureOpenAIProvider(LLMOptions options)
    {
        _options = options;
    }

    public async Task<LLMResponse> GenerateAsync(
        LLMRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ValidateOptions();

            var endpoint =
                $"{_options.Endpoint}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version=2024-10-21";

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add(
                "api-key",
                _options.ApiKey);

            var payload = new
            {
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = request.Prompt
                    }
                },
                temperature = request.Options?.Temperature ?? 0.7,
                max_tokens = request.Options?.MaxTokens ?? 500
            };

            var content = CreateJsonContent(payload);

            var response =
                await HttpClient.PostAsync(
                    endpoint,
                    content,
                    cancellationToken);

            await EnsureSuccessAsync(response);

            var json =
                await response.Content.ReadAsStringAsync(cancellationToken);

            using var doc = JsonDocument.Parse(json);

            var answer =
                doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

            int? promptTokens = null;
            int? completionTokens = null;
            int? totalTokens = null;

            if (doc.RootElement.TryGetProperty("usage", out var usage))
            {
                promptTokens =
                    usage.GetProperty("prompt_tokens").GetInt32();

                completionTokens =
                    usage.GetProperty("completion_tokens").GetInt32();

                totalTokens =
                    usage.GetProperty("total_tokens").GetInt32();
            }

            return new LLMResponse
            {
                Success = true,
                Content = answer ?? string.Empty,
                PromptTokens = promptTokens,
                CompletionTokens = completionTokens,
                TotalTokens = totalTokens
            };
        }
        catch (Exception ex)
        {
            return new LLMResponse
            {
                Success = false,
                Content = string.Empty,
                ErrorMessage = ex.Message
            };
        }
    }

    private void ValidateOptions()
    {
        if (string.IsNullOrWhiteSpace(_options.Endpoint))
            throw new ArgumentException("Azure OpenAI Endpoint is required.");

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new ArgumentException("Azure OpenAI ApiKey is required.");

        if (string.IsNullOrWhiteSpace(_options.DeploymentName))
            throw new ArgumentException("Azure OpenAI DeploymentName is required.");
    }
}