namespace BankAdminSystem2.Services
{
    public static class MyReader
    {
        public static string ReadString_WithCondition(Predicate<string> condition, string errorMessage = "Invalid input, try again.")
        {
            Console.Write(">>> ");
            string input = Console.ReadLine() ?? "";
            while (!condition(input))
            {
                Console.WriteLine(errorMessage);
                Console.Write(">>> ");
                input = Console.ReadLine() ?? "";
            }
            return input;
        }
        public static double ReadDouble_WithCondition(Predicate<double> condition, string errorMessage = "Invalid input, try again.")
        {
            Console.Write(">>> ");
            double input = 0.0;
            while (!double.TryParse(Console.ReadLine(), out input) || !condition(input))
            {
                Console.WriteLine(errorMessage);
                Console.Write(">>> ");
            }
            return input;
        }
        public static int ReadInt_WithCondition(Predicate<int> condition, string errorMessage = "Invalid input, try again.")
        {
            Console.Write(">>> ");
            int input = 0;
            while (!int.TryParse(Console.ReadLine(), out input) || !condition(input))
            {
                Console.WriteLine(errorMessage);
                Console.Write(">>> ");
            }
            return input;
        }
        public static int ReadOption(int rangeStart, int rangeEnd)
        {
            if (rangeEnd < rangeStart)
                throw new ArgumentException("rangeEnd must be greater than rangeStart");
            Console.Write(">>> ");
            int input = 0;
            while (!int.TryParse(Console.ReadLine(), out input) || input < rangeStart || input > rangeEnd)
            {
                Console.WriteLine($"Invalid option, enter number between {rangeStart} and {rangeEnd}.");
                Console.Write(">>> ");
            }
            return input;
        }
    }
}
