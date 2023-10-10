namespace TGProVN.Extension.Serialization.Abstractions.Serializers;

public interface IJsonSerializer
{
    /// <summary>
    /// Serializes an object to its JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>The JSON representation of the object.</returns>
    string Serialize<T>(T obj);

    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="text">The JSON string to deserialize.</param>
    /// <returns>The deserialized object, or <c>null</c> if the JSON string is empty or null.</returns>
    T? Deserialize<T>(string text);
}