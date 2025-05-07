using System.Text.Json;
using System.Text.Json.Serialization;

namespace NaviriaAPI.Helpers
{
    public class PolymorphicJsonConverter<TBase> : JsonConverter<TBase> where TBase : class
    {
        private readonly string _typeProperty;
        private readonly Dictionary<string, Type> _typeMap;

        public PolymorphicJsonConverter(string typeProperty, Dictionary<string, Type> typeMap)
        {
            _typeProperty = typeProperty;
            _typeMap = typeMap;
        }

        public override TBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty(_typeProperty, out var typeProp))
                throw new JsonException($"Missing '{_typeProperty}' property");

            var discriminator = typeProp.GetString();
            if (discriminator == null || !_typeMap.TryGetValue(discriminator, out var targetType))
                throw new JsonException($"Unknown {_typeProperty}: {discriminator}");

            return (TBase?)JsonSerializer.Deserialize(root.GetRawText(), targetType, options);
        }

        public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}