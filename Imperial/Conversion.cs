using System;
using System.Linq;

namespace Imperial
{
    public static class Conversion
    {
        public static bool TryParseFeet(string value, out double feet)
        {
            var result = ToFeet(value);
            feet = result;
            return !double.IsNaN(result);
        }

        public static double ToFeet(string value)
        {
            value = ToLowerAndTrimWhitespaces(value);
            value = ReplaceFeetWithF(value);
            value = ReplaceInchesWithI(value);
            value = AddMissingParts(value);

            var values = SplitIntoNonEmptyParts(value, 'f');
            var length = values.Length;
            if (length > 0 && length < 3)
            {
                var feet = ToDouble(values[0]);
                if (length == 1)
                {
                    return feet;
                }
                var inchesPart = SplitIntoNonEmptyParts(values[1], 'i');
                var inches = ToDoubleWithFractions(inchesPart[0]);
                return feet + (inches/12d);
            }
            return double.NaN;
        }

        private static string[] SplitIntoNonEmptyParts(string value, char separator)
        {
            return value.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        private static double ToDoubleWithFractions(string value)
        {
            if (value.IndexOf('/') == -1)
            {
                return ToDouble(value);
            }
            var wholeAndFraction = SplitIntoNonEmptyParts(value, ' ');
            if (wholeAndFraction.Length == 2)
            {
                var whole = (double) ToInteger(wholeAndFraction[0]);
                return whole + ToFraction(wholeAndFraction[1]);
            }
            return ToFraction(value);
        }

        private static double ToFraction(string value)
        {
            var split = SplitIntoNonEmptyParts(value, '/');
            if (split.Length == 2)
            {
                return ToInteger(split[0])/(double) ToInteger(split[1]);
            }
            return double.NaN;
        }

        private static int ToInteger(string value)
        {
            return int.Parse(value);
        }

        private static string AddMissingParts(string value)
        {
            if (value.IndexOf('f') == -1)
            {
                value = value.IndexOf('i') > 0 ? "0f" + value : value + "f";
            }
            else
            {
                value = RemoveMiddleDash(value);
            }
            return value;
        }

        private static string RemoveMiddleDash(string value)
        {
            if (value.IndexOf('-') > 0)
            {
                value = value.Replace("-", string.Empty);
            }
            return value;
        }

        private static string ReplaceInchesWithI(string value)
        {
            return value
                .Replace("inches", "i")
                .Replace("inch", "i")
                .Replace("in", "i")
                .Replace("\"", "i")
                .Replace("“", "i")
                .Replace("”", "i");
        }

        private static string ReplaceFeetWithF(string value)
        {
            return value
                .Replace("'", "f")
                .Replace("feet", "f")
                .Replace("foot", "f")
                .Replace("ft", "f")
                .Replace("‘", "f")
                .Replace("’", "f");
        }

        private static string ToLowerAndTrimWhitespaces(string value)
        {
            return value.Replace("~", "").Trim().ToLower();
        }

        private static double ToDouble(string value)
        {
            double result;
            return double.TryParse(value, out result) ? result : double.NaN;
        }

        private static int EuclideanGCD(int a, int b)
        {
            while (b != 0)
            {
                var m = a%b;
                a = b;
                b = m;
            }
            return a;
        }

        public static string ToFeetInches(double value)
        {
            return ToFeetInches(value, 16);
        }

        public static string ToFeetInches(double value, int smallestFractionalDenominator)
        {
            if (smallestFractionalDenominator < 1)
            {
                throw new InvalidOperationException("Smallest fractional denomiator must be 1 or greater.");
            }

            var positiveValue = Math.Abs(value);
            var feet = (int) Math.Floor(positiveValue);
            var fractionalInches = 12d*(positiveValue - feet);
            var inches = (int) fractionalInches;
            var info = new FeetInchesInfo
                       {
                           IsNegative = value < 0d,
                           Feet = feet,
                           Inches = inches
                       };

            fractionalInches = fractionalInches - inches;
            var denominator = smallestFractionalDenominator;
            var epsilon = 1d/denominator*0.0001d;
            if (!(fractionalInches > epsilon))
            {
                return info.ToString();
            }

            var numerator = (int) Math.Round(fractionalInches*denominator, MidpointRounding.AwayFromZero);
            if (Math.Abs(numerator/(double) denominator - fractionalInches) > epsilon)
            {
                info.IsApproximate = true;
            }
            var gcd = EuclideanGCD(numerator, denominator);
            if (gcd > 1)
            {
                numerator = numerator/gcd;
                denominator = denominator/gcd;
            }
            info.Numerator = numerator;
            info.Denominator = denominator;
            if (numerator != denominator)
            {
                return info.ToString();
            }

            info.Inches += 1;
            info.Numerator = 0;
            if (info.Inches != 12)
            {
                return info.ToString();
            }

            info.Inches = 0;
            info.Feet += 1;
            return info.ToString();
        }

        private sealed class FeetInchesInfo
        {
            public int Feet { get; set; }
            public int Inches { get; set; }
            public int Numerator { private get; set; }
            public int Denominator { private get; set; }
            public bool IsNegative { private get; set; }
            public bool IsApproximate { private get; set; }

            public override string ToString()
            {
                var result = string.Empty;
                if (IsApproximate)
                {
                    result = "~ " + result;
                }
                if (IsNegative)
                {
                    result += "-";
                }
                if (Feet > 0)
                {
                    result += string.Format("{0}'", Feet);
                }
                if (Inches > 0)
                {
                    if (result.Length > 0)
                    {
                        result += " ";
                    }
                    result += string.Format("{0}", Inches);
                    if (Numerator == 0)
                    {
                        result += "\"";
                    }
                }
                if (Numerator > 0)
                {
                    if (result.Length > 0)
                    {
                        result += " ";
                    }
                    result += string.Format("{0}/{1}\"", Numerator, Denominator);
                }
                if (Feet == 0 && Inches == 0 && Numerator == 0)
                {
                    result = "0'";
                    if (IsApproximate)
                    {
                        result = "~ " + result;
                    }
                }
                return result;
            }
        }
    }
}