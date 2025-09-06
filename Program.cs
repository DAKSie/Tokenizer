namespace Tokenizer {
    class Program {
        static void Main() {
            List<string> testCases = new List<string>();

            testCases.Add("The brave hero fights the dragon in the dark forest.");
            testCases.Add("A wizard discovers a cave while the moon rises.");
            // testCases.Add("The knight who rides a horse rescues the princess and then finds the treasure.");
            testCases.Add("The brave hero fights the dragon in the dark forest.");
            testCases.Add("The the the the the knight");

            Console.WriteLine("Phase 1:");

            PhaseOne(testCases);
            
            Console.WriteLine("Phase 2:");

            PhaseTwo(testCases);
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

        public static void PhaseTwo(List<string> testCases) {
            Deriver dr = new Deriver();
            foreach (var test in testCases) {
                Console.WriteLine($"Test case: {test}");
                var derivation = dr.Derive(test);
                foreach (var step in derivation)
                Console.WriteLine("=> " + step);
            }
        }
    }
}
