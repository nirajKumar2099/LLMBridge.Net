using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Models
{
    public sealed class LLMRequest
    {
        public string Prompt { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public GenerationOptions? Options { get; set; }
    }
}
