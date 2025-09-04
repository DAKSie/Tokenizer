namespace Tokenizer
{
    class Program
    {
        static void Main()
        {
            List<string> testCases = new List<string>();
            testCases.Add("The brave hero fights the dragon in the dark forest.");
            testCases.Add("A wizard discovers a cave while the moon rises.");
            testCases.Add("The knight who rides a horse rescues the princess and then finds the treasure.");


            foreach (string input in testCases)
            {
                Console.WriteLine($"Test: {input}");
                var tokens = Tokenizer.Tokenize((input));
                Console.WriteLine("Tokens: ");
                foreach (var token in tokens)
                {
                    Console.WriteLine($" {token}");
                }
                Console.WriteLine();
            }
        }
    }
}
