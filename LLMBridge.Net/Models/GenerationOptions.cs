using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Models
{
    public sealed class GenerationOptions
    {
        public double? Temperature { get; set; }

        public int? MaxTokens { get; set; }

//        TopP
//        FrequencyPenalty
//        PresencePenalty
//        StopSequences
//        Seed
    }
}
