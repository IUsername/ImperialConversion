namespace Imperial.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine("Enter a decimal number to convert it to feet and inches.");

            var input = System.Console.ReadLine();
            double feetDecimal;
            while (double.TryParse(input, out feetDecimal))
            {
                var output = Conversion.ToFeetInches(feetDecimal, 64);
                System.Console.WriteLine("Result: " + output);
                input = System.Console.ReadLine();
            }
        }
    }
}