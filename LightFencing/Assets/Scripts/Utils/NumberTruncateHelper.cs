using System;
using UnityEngine;

namespace LightFencing.Utils
{
    /// <summary>
    /// Helper class to truncate different types of numbers
    /// </summary>
    public static class NumberTruncateHelper
    {
        /// <summary>
        /// Truncates given numerical object. Works for decimals, floats and doubles
        /// </summary>
        public static object TruncateValue(object value, int decimalPlaces)
        {
            switch (value)
            {
                case decimal decimalValue:
                    var roundedDecimal = Math.Round(decimalValue, decimalPlaces);
                    return IsWholeValue(roundedDecimal) ? Convert.ToInt64(roundedDecimal) : (object)roundedDecimal;
                case float floatValue:
                    var roundedFloat = Math.Round(floatValue, decimalPlaces);
                    return IsWholeValue(roundedFloat) ? Convert.ToInt64(roundedFloat) : (object)roundedFloat;
                case double doubleValue:
                    var roundedDouble = Math.Round(doubleValue, decimalPlaces);
                    return IsWholeValue(roundedDouble) ? Convert.ToInt64(roundedDouble) : (object)roundedDouble;
            }
            Debug.LogWarning("Trying to truncate invalid value");
            return -1;
        }

        public static bool IsWholeValue(object value)
        {
            return value switch
            {
                decimal decimalValue => IsWholeValue(decimalValue),
                float floatValue => IsWholeValue(floatValue),
                double doubleValue => IsWholeValue(doubleValue),
                _ => false
            };
        }

        public static bool IsWholeValue(decimal decimalValue)
        {
            int precision = (decimal.GetBits(decimalValue)[3] >> 16) & 0x000000FF;
            return precision == 0;
        }

        public static bool IsWholeValue(double doubleValue)
        {
            return doubleValue == Math.Truncate(doubleValue);
        }

        public static bool IsWholeValue(float floatValue)
        {
            return floatValue == Math.Truncate(floatValue);
        }
    }
}