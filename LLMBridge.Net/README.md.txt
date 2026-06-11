# LLMBridge.Net

A unified .NET SDK for OpenAI, Azure OpenAI, Gemini, Claude, and future LLM providers.

LLMBridge.Net provides a single, consistent API for interacting with multiple Large Language Model (LLM) providers without vendor lock-in.

## Features

* Unified API across providers
* OpenAI support
* Azure OpenAI support
* Google Gemini support
* Anthropic Claude support
* Provider switching with minimal code changes
* Common request and response models
* Token usage tracking
* Async-first design
* .NET 8+ compatible

## Installation

```bash
dotnet add package LLMBridge.Net
```

## Quick Start

```csharp
using LLMBridge.Net.Client;
using LLMBridge.Net.Enums;
using LLMBridge.Net.Models;

var options = new LLMOptions
{
    Provider = LLMProviderType.Gemini,
    ApiKey = "YOUR_API_KEY"
};

var client = new LLMClient(options);

var response = await client.GenerateAsync(
    new LLMRequest
    {
        Model = "gemini-2.5-flash",
        Prompt = "Explain SOLID Principles"
    });

Console.WriteLine(response.Content);
```

## Supported Providers

| Provider     | Status    |
| ------------ | --------- |
| OpenAI       | Supported |
| Azure OpenAI | Supported |
| Gemini       | Supported |
| Claude       | Supported |

## Why LLMBridge.Net?

Without LLMBridge.Net:

```csharp
var openAiClient = new OpenAIClient(...);
var geminiClient = new GeminiClient(...);
var anthropicClient = new AnthropicClient(...);
```

With LLMBridge.Net:

```csharp
var client = new LLMClient(options);
var response = await client.GenerateAsync(request);
```

Only the provider changes.

## Response Model

```csharp
public class LLMResponse
{
    public bool Success { get; set; }
    public string Content { get; set; }
    public string? ErrorMessage { get; set; }

    public int? PromptTokens { get; set; }
    public int? CompletionTokens { get; set; }
    public int? TotalTokens { get; set; }
}
```

## Roadmap

### v1.0.0

* Text Generation
* Multi-Provider Support
* Unified Response Model

### v1.1.0

* Streaming
* Function Calling
* Tool Calling
* Embeddings

### v2.0.0

* RAG Helpers
* Vector Database Integrations
* Multi-Agent Support


