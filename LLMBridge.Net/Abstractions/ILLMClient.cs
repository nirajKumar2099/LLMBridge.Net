using LLMBridge.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Abstractions
{
    public interface ILLMClient
    {
        Task<LLMResponse> GenerateAsync(LLMRequest request, CancellationToken cancellationToken = default);
    }
}
