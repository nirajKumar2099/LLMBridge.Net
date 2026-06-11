using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Models;
using LLMBridge.Net.Providers.OpenAI.Dtos;

namespace LLMBridge.Net.Providers.OpenAI
{
    public sealed class OpenAIProvider :
        BaseProvider,
        ILLMProvider
    {
        private readonly LLMOptions _options;

        private const string Endpoint = "https://api.openai.com/v1/chat/completions";

        public OpenAIProvider(LLMOptions options)
        {
            _options = options;
        }

        public async Task<LLMResponse> GenerateAsync(LLMRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                SetBearerToken(_options.ApiKey);

                var payload = new OpenAIRequest
                    {
                        Model = request.Model,

                        Temperature = request.Options?.Temperature,

                        MaxTokens = request.Options?.MaxTokens,

                        Messages =
                            [
                                new OpenAIMessage
                                {
                                    Role = "user",
                                    Content = request.Prompt
                                }
                            ]
                        };

                var content = CreateJsonContent(payload);


                var response = await HttpClient.PostAsync(Endpoint, content, cancellationToken);

                await EnsureSuccessAsync(response);

                var openAiResponse = await DeserializeAsync<OpenAIResponse>(response);

                return new LLMResponse
                {
                    Success = true,

                    Content =
                        openAiResponse?
                            .Choices?
                            .FirstOrDefault()?
                            .Message?
                            .Content
                        ?? string.Empty,

                    PromptTokens =
                        openAiResponse?.Usage?.PromptTokens,

                    CompletionTokens =
                        openAiResponse?.Usage?.CompletionTokens,

                    TotalTokens =
                        openAiResponse?.Usage?.TotalTokens
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
}