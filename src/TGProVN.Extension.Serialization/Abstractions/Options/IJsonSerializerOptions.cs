using System.Text.Json;

namespace TGProVN.Extension.Serialization.Abstractions.Options;

public interface IJsonSerializerOptions
{
    /// <summary>
    /// Gets the JSON serializer options configured for the application.
    /// </summary>
    /// <remarks>
    /// These options can be used to customize the behavior of the JSON serializer, such as
    /// specifying naming policies, handling null values, or setting converters.
    /// See <see cref="System.Text.Json.JsonSerializerOptions"/> for more details.
    /// </remarks>
    public JsonSerializerOptions JsonSerializerOptions { get; }
}