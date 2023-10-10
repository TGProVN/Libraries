using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TGProVN.Extension.Serialization.JsonConverters;

/// <summary>
/// The new Json.NET doesn't support Timespan at this time
/// https://github.com/dotnet/corefx/issues/38641
/// </summary>
public class TimespanJsonConverter : JsonConverter<TimeSpan>
{
    /// <summary>
    /// Format: Days.Hours:Minutes:Seconds:Milliseconds
    /// </summary>
    private const string TIME_SPAN_FORMAT_STRING = @"d\.hh\:mm\:ss\:FFF";

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (string.IsNullOrWhiteSpace(s)) {
            return TimeSpan.Zero;
        }

        if (!TimeSpan.TryParseExact(s, TIME_SPAN_FORMAT_STRING, null, out var parsedTimeSpan)) {
            throw new FormatException(
                $"Input timespan is not in an expected format : expected {Regex.Unescape(TIME_SPAN_FORMAT_STRING)}. Please retrieve this key as a string and parse manually.");
        }

        return parsedTimeSpan;
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        var timespanFormatted = $"{value.ToString(TIME_SPAN_FORMAT_STRING)}";
        writer.WriteStringValue(timespanFormatted);
    }
}