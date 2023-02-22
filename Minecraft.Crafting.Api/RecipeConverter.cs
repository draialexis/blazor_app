namespace Minecraft.Crafting.Api
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JsonConverter = Newtonsoft.Json.JsonConverter;
    using JsonSerializer = Newtonsoft.Json.JsonSerializer;

    public struct IngredientElement
    {
        public IngredientClass IngredientClass;
        public long? Integer;

        public static implicit operator IngredientElement(IngredientClass IngredientClass) => new IngredientElement { IngredientClass = IngredientClass };

        public static implicit operator IngredientElement(long Integer) => new IngredientElement { Integer = Integer };
    }

    public struct InShape
    {
        public IngredientClass IngredientClass;
        public long? Integer;

        public bool IsNull => IngredientClass == null && Integer == null;

        public static implicit operator InShape(IngredientClass IngredientClass) => new InShape { IngredientClass = IngredientClass };

        public static implicit operator InShape(long Integer) => new InShape { Integer = Integer };
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, Recipes[]> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class IngredientClass
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("metadata")]
        public long Metadata { get; set; }
    }

    public partial class Recipes
    {
        [JsonProperty("ingredients", NullValueHandling = NullValueHandling.Ignore)]
        public IngredientElement[] Ingredients { get; set; }

        [JsonProperty("inShape", NullValueHandling = NullValueHandling.Ignore)]
        public InShape[][] InShape { get; set; }

        [JsonProperty("outShape", NullValueHandling = NullValueHandling.Ignore)]
        public long?[][] OutShape { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Recipes
    {
        public static Dictionary<string, Recipes[]> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, Recipes[]>>(json, Converter.Settings);
    }

    public class Result
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("metadata")]
        public long Metadata { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                InShapeConverter.Singleton,
                IngredientElementConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IngredientElementConverter : JsonConverter
    {
        public static readonly IngredientElementConverter Singleton = new IngredientElementConverter();

        public override bool CanConvert(Type t) => t == typeof(IngredientElement) || t == typeof(IngredientElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new IngredientElement { Integer = integerValue };

                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<IngredientClass>(reader);
                    return new IngredientElement { IngredientClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type IngredientElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (IngredientElement)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.IngredientClass != null)
            {
                serializer.Serialize(writer, value.IngredientClass);
                return;
            }
            throw new Exception("Cannot marshal type IngredientElement");
        }
    }

    internal class InShapeConverter : JsonConverter
    {
        public static readonly InShapeConverter Singleton = new InShapeConverter();

        public override bool CanConvert(Type t) => t == typeof(InShape) || t == typeof(InShape?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return new InShape { };

                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new InShape { Integer = integerValue };

                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<IngredientClass>(reader);
                    return new InShape { IngredientClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type InShape");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (InShape)untypedValue;
            if (value.IsNull)
            {
                serializer.Serialize(writer, null);
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.IngredientClass != null)
            {
                serializer.Serialize(writer, value.IngredientClass);
                return;
            }
            throw new Exception("Cannot marshal type InShape");
        }
    }
}