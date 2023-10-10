using System.Text.Json;
using TGProVN.Extension.Serialization.Abstractions.Options;

namespace TGProVN.Extension.Serialization;

public class SystemTextJsonOptions : IJsonSerializerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}