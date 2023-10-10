using System.Text.Json;
using Microsoft.Extensions.Options;
using TGProVN.Extension.Serialization.Abstractions.Serializers;

namespace TGProVN.Extension.Serialization;

public class SystemTextJsonSerializer(IOptions<SystemTextJsonOptions> options) : IJsonSerializer
{
    private readonly JsonSerializerOptions _options = options.Value.JsonSerializerOptions;

    /// <inheritdoc/>
    public T? Deserialize<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data, _options);
    }

    /// <inheritdoc/>
    public string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, _options);
    }
}