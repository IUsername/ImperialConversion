using NUnit.Framework;

namespace Imperial.Tests
{
    [TestFixture]
    public class ImperialParsingTests
    {
        [Test]
        public void CanHandleFeetAndDecimalInches()
        {
            string given = "3' 1.5\"";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(3.125d));
        }

        [Test]
        public void CanHandleFractionalInches()
        {
            string given = "3' 3/8\"";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(3.03125d));
        }

        [Test]
        public void CanHandleImperfectFractionalInches()
        {
            string given = "75' 2 3/4\"";
            Assert.That(Conversion.ToFeet(given), Is.InRange(75.2291666666666d, 75.2291666666668d));
        }

        [Test]
        public void CanHandleInchesWithoutFeet()
        {
            string given = "3in";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(0.25d));
        }

        [Test]
        public void CanHandleNegativeFeet()
        {
            string given = "-8ft";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(-8d));
        }

        [Test]
        public void CanHandleNegativeInches()
        {
            string given = "-6 inches";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(-0.5d));
        }

        [Test]
        public void CanHandleWholeAndFractionalInches()
        {
            string given = "3' 1 1/2\"";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(3.125d));
        }

        [Test]
        public void CanParseDoubleQuoteAsInches()
        {
            string given = "6\"";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(0.5d));
        }

        [Test]
        public void CanParseFtAsFeet()
        {
            string given = "6.5ft";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(6.5d));
        }

        [Test]
        public void InvalidInputProducesNaN()
        {
            string given = "6.52e";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(double.NaN));
        }

        [Test]
        public void TryParseInvalidInputReturnsFalse()
        {
            string given = "6.52e";
            double value;
            bool result = Conversion.TryParseFeet(given, out value);
            Assert.That(result, Is.False);
            Assert.That(value, Is.EqualTo(double.NaN));
        }

        [Test]
        public void TryParseInvalidInputReturnsTrue()
        {
            string given = "6' 3/8\"";
            double value;
            bool result = Conversion.TryParseFeet(given, out value);
            Assert.That(result, Is.True);
            Assert.That(value, Is.EqualTo(6.03125d));
        }

        [Test]
        public void CanParseSingleAndDoubleQuotes()
        {
            string given = "6' 3\"";
            double value = Conversion.ToFeet(given);
            Assert.That(value, Is.EqualTo(6.25d));
        }

        [Test]
        public void HandlesNoUnitsAsFeet()
        {
            string given = "5.5";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(5.5d));
        }

        [Test]
        public void RemovesDashesInTheMiddle()
        {
            string given = "6'-6";
            Assert.That(Conversion.ToFeet(given), Is.EqualTo(6.5d));
        }
    }
}