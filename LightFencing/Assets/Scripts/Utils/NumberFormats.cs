using System.Globalization;

namespace LightFencing.Utils
{
    public class NumberFormats
    {
        public static NumberFormatInfo StandardNumberFormat = null;

        static NumberFormats()
        {
            CultureInfo info = CultureInfo.InvariantCulture;
            StandardNumberFormat = (NumberFormatInfo)info.NumberFormat.Clone();
            StandardNumberFormat.NumberDecimalSeparator = ".";
            StandardNumberFormat.CurrencyDecimalSeparator = ",";
        }
    }
}