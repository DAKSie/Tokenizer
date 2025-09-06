namespace Tokenizer {
    class Program {
        static void Main() {
            List<string> testCases = new List<string>();

            testCases.Add("The mighty hero fights the dragon in the dark forest.");
            testCases.Add("A brave old knight who rescues villager rides.");
            testCases.Add("The wizard discovers treasure in the cave.");
            testCases.Add("The princess holds the sword quickly.");
            testCases.Add("The brave knight protects the wizard quickly");

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Phase 1:");

            PhaseOne(testCases);
            
            Console.WriteLine("---------------------------------------------------------");
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
            foreach (var input in testCases) {
                var tokens = Tokenizer.Tokenize(input);
                var parser = new Parser(tokens);
                Console.WriteLine($"\n\nTest: {input}");
                parser.Derive();
            }
        }
    }
}
