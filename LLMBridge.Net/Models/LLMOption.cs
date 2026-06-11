using LLMBridge.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Models
{
    public sealed class LLMOptions
    {
        public LLMProviderType Provider { get; set; }

        public string ApiKey { get; set; } = string.Empty;

        public string? Endpoint { get; set; }

        public string? DeploymentName { get; set; }
    }
}
