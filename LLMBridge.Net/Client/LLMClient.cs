using LLMBridge.Net.Abstractions;
using LLMBridge.Net.Factory;
using LLMBridge.Net.Models;

namespace LLMBridge.Net.Client;

public sealed class LLMClient : ILLMClient
{
    private readonly ILLMProvider _provider;

    public LLMClient(LLMOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var factory = new LLMProviderFactory();

        _provider = factory.Create(options);
    }

    public async Task<LLMResponse> GenerateAsync(LLMRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Prompt))
            throw new ArgumentException(
                "Prompt is required.");

        if (string.IsNullOrWhiteSpace(request.Model))
            throw new ArgumentException(
                "Model is required.");

        return await _provider.GenerateAsync(request, cancellationToken);


    }
}