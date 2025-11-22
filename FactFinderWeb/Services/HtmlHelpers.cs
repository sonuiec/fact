using System.Globalization;

namespace FactFinderWeb.Services
{
    public static class HtmlHelpers
    {
        public static string FormatDecimal(this decimal? value)
        {
            if (!value.HasValue)
                return string.Empty;

            // Indian number formatting
            var indianFormat = new CultureInfo("en-IN", false).NumberFormat;
            indianFormat.NumberGroupSizes = new[] { 3, 2 };

            decimal val = value.Value;

            // Custom rounding logic with negative support
            long result;
            if (val >= 0)
            {
                result = (val - Math.Floor(val)) >= 0.5m
                    ? (long)Math.Floor(val) + 1
                    : (long)Math.Floor(val);
            }
            else
            {
                // For negative numbers: -123.51 → -124, -123.49 → -123
                result = (Math.Ceiling(val) - val) >= 0.5m
                    ? (long)Math.Ceiling(val) - 1
                    : (long)Math.Ceiling(val);
            }

            return result.ToString("N0", indianFormat);
        }


    }
}
