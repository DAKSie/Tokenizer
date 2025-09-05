namespace Tokenizer {
    class Program {
        static void Main() {
            List<string> testCases = new List<string>();

            testCases.Add("The brave hero fights the dragon in the dark forest.");
            testCases.Add("A wizard discovers a cave while the moon rises.");
            testCases.Add("The knight who rides a horse rescues the princess and then finds the treasure.");

            Console.WriteLine("Phase 1:");

            PhaseOne(testCases);

            Console.WriteLine("Ambot wala nako kabalo HAHAHAHA");
        }

        public static void PhaseOne(List<string> testCases) {
            foreach (string input in testCases) {
                Console.WriteLine($"Test: {input}");
                var tokens = Tokenizer.Tokenize((input));

                Console.WriteLine("Tokens: ");

                foreach (var token in tokens) {
                    Console.WriteLine($" {token}");
                }
                Console.WriteLine();
            }
        }
    }
}
