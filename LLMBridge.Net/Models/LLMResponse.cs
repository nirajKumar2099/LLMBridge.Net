using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Models;

public sealed class LLMResponse
{
    public bool Success { get; set; }

    public string Content { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public int? PromptTokens { get; set; }

    public int? CompletionTokens { get; set; }

    public int? TotalTokens { get; set; }
}