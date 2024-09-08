using Newtonsoft.Json;
using System;
using System.Globalization;
using static LightFencing.Utils.NumberTruncateHelper;

namespace LightFencing.Utils
{
    /// <summary>
    /// Converter that truncates trailing zeros if number is a whole value.
    /// Additionally, truncates decimal numbers to 3 decimal places.
    /// </summary>
    public class DecimalTruncateConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(float) || objectType == typeof(double);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(IsWholeValue(value)
                ? Convert.ToInt64(value).ToString(CultureInfo.InvariantCulture)
                : JsonConvert.ToString(value));
        }
    }
}