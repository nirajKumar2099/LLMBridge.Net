using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Enums;
using LLMBridge.Net.Models;
using LLMBridge.Net.Providers.Anthropic;
using LLMBridge.Net.Providers.AzureOpenAI;
using LLMBridge.Net.Providers.Gemini;
using LLMBridge.Net.Providers.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Factory
{
    public class LLMProviderFactory
    {
        public ILLMProvider Create(LLMOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            return options.Provider switch
            {
                LLMProviderType.OpenAI =>
                    new OpenAIProvider(options),

                LLMProviderType.Gemini =>
                    new GeminiProvider(options),

                LLMProviderType.Claude =>
                    new ClaudeProvider(options),

                LLMProviderType.AzureOpenAI =>
                    new AzureOpenAIProvider(options),

                _ => throw new NotSupportedException(
                    $"Provider {options.Provider} is not supported.")
            };
        }
    }
}
