using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartMeter.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle string date: "2025-10-29"
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                    throw new JsonException("Empty date string.");

                if (DateOnly.TryParse(value, out var result))
                    return result;

                if (DateOnly.TryParseExact(value, Format, out result))
                    return result;

                throw new JsonException($"Invalid date format: {value}. Expected format: {Format}");
            }

            // Handle object date: { "year": 2025, "month": 10, "day": 29 }
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    var root = document.RootElement;

                    int year = root.TryGetProperty("year", out var y) ? y.GetInt32() : 0;
                    int month = root.TryGetProperty("month", out var m) ? m.GetInt32() : 0;
                    int day = root.TryGetProperty("day", out var d) ? d.GetInt32() : 0;

                    // Validate before constructing DateOnly
                    if (year <= 0 || month <= 0 || day <= 0)
                        throw new JsonException($"Invalid date parts (year={year}, month={month}, day={day}).");

                    try
                    {
                        return new DateOnly(year, month, day);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new JsonException($"Invalid date values (year={year}, month={month}, day={day}).");
                    }
                }
            }

            // Handle nulls or unexpected tokens
            if (reader.TokenType == JsonTokenType.Null)
                throw new JsonException("Date value cannot be null.");

            throw new JsonException($"Unexpected token {reader.TokenType} when parsing DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
