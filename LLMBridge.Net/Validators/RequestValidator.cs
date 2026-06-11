using LLMBridge.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Validators
{
    internal static class RequestValidator
    {

        public static void Validate(LLMRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                throw new ArgumentException(
                    "Prompt cannot be empty.",
                    nameof(request.Prompt));
            }

            if (string.IsNullOrWhiteSpace(request.Model))
            {
                throw new ArgumentException(
                    "Model cannot be empty.",
                    nameof(request.Model));
            }

            if (request.Options?.Temperature is < 0 or > 2)
            {
                throw new ArgumentException(
                    "Temperature must be between 0 and 2.");
            }

            if (request.Options?.MaxTokens <= 0)
            {
                throw new ArgumentException(
                    "MaxTokens must be greater than 0.");
            }
        }
    }
}
