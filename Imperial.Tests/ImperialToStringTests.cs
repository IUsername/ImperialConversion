using NUnit.Framework;

namespace Imperial.Tests
{
    [TestFixture]
    public class ImperialToStringTests
    {
        [Test]
        public void ApproximateZeroValue()
        {
            double given = -0.000001;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("~ 0'"));
        }

        [Test]
        public void DecimalValueToApproximateFeetInchesAndFractionalInches()
        {
            double given = -5.515626d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("~ -5' 6 3/16\""));
        }

        [Test]
        public void DecimalValueToApproximateFeetInchesAndFractionalInchesOf64()
        {
            double given = 3.52d;
            string value = Conversion.ToFeetInches(given, 64);
            Assert.That(value, Is.EqualTo("~ 3' 6 15/64\""));
        }

        [Test]
        public void DecimalValueToApproximateFeetInchesAndFractionalInchesOf128()
        {
            double given = 3.52d;
            string value = Conversion.ToFeetInches(given, 128);
            Assert.That(value, Is.EqualTo("~ 3' 6 31/128\""));
        }

        [Test]
        public void DecimalValueToFeetInchesAndFractionalInchesOf1000()
        {
            double given = 3.52d;
            string value = Conversion.ToFeetInches(given, 1000);
            Assert.That(value, Is.EqualTo("3' 6 6/25\""));
        }

        [Test]
        public void DecimalValueToFeetAndWholeInches()
        {
            double given = 3.5d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("3' 6\""));
        }

        [Test]
        public void DecimalValueToFeetEpsilon()
        {
            double given = 3.9999999d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("4'"));
        }

        [Test]
        public void DecimalValueToFeetInchesAndFractionalInches()
        {
            double given = 3.515625d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("3' 6 3/16\""));
        }

        [Test]
        public void DecimalValueToFeetInchesAndFractionalInchesOf64()
        {
            double given = 101.51953125d;
            string value = Conversion.ToFeetInches(given, 64);
            Assert.That(value, Is.EqualTo("101' 6 15/64\""));
        }

        [Test]
        public void DecimalValueToFeetInchesAndFractionalInchesReduced()
        {
            double given = 3.52083d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("~ 3' 6 1/4\""));
        }

        [Test]
        public void DecimalValueToFeetInchesRoundUp()
        {
            double given = -3.5832d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("~ -3' 7\""));
        }

        [Test]
        public void DecimalValueToFeetRoundUp()
        {
            double given = -3.99999d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("~ -4'"));
        }

        [Test]
        public void NegativeDecimalValueToFeetAndWholeInches()
        {
            double given = -8.75d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("-8' 9\""));
        }

        [Test]
        public void NegativeWholeFeetToString()
        {
            double given = -12.0d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("-12'"));
        }

        [Test]
        public void WholeValueToWholeFeet()
        {
            double given = 3.0d;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("3'"));
        }

        [Test]
        public void ZeroValue()
        {
            double given = 0.00000001;
            string value = Conversion.ToFeetInches(given);
            Assert.That(value, Is.EqualTo("0'"));
        }
    }
}