using System.Text.Json.Serialization;

namespace LLMBridge.Net.Providers.OpenAI.Dtos
{
    public sealed class OpenAIResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; set; }

        [JsonPropertyName("usage")]
        public Usage? Usage { get; set; }
    }

    public sealed class Choice
    {
        [JsonPropertyName("message")]
        public Message? Message { get; set; }
    }

    public sealed class Message
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }

    public sealed class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}