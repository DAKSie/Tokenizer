namespace Tokenizer {
    public class Parser {
        private readonly List<Token> tokens;
        private int position = 0;

        private static Dictionary<TokenType, List<List<TokenType>>> GrammarRules;

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
            InitGrammar();
        }

        // ---------------------------
        // Grammar initialization
        // ---------------------------
        private void InitGrammar() {
            GrammarRules = new Dictionary<TokenType, List<List<TokenType>>>();

            // Story ::= Sentence
            GrammarRules[TokenType.Story] = new() {
                new List<TokenType> { TokenType.Sentence }
            };

            // Sentence ::= SimpleSentence
            GrammarRules[TokenType.Sentence] = new() {
                new List<TokenType> { TokenType.SimpleSentence }
            };

            // SimpleSentence ::= Subject VerbPhrase Extra
            GrammarRules[TokenType.SimpleSentence] = new() {
                new List<TokenType> { TokenType.Subject, TokenType.VerbPhrase, TokenType.Extra }
            };

            // Subject ::= NounPhrase
            GrammarRules[TokenType.Subject] = new() {
                new List<TokenType> { TokenType.NounPhrase }
            };

            // NounPhrase ::= Determiner AdjectiveList Noun | AdjectiveList Noun | Noun
            GrammarRules[TokenType.NounPhrase] = new() {
                new List<TokenType> { TokenType.Determiner, TokenType.AdjectiveList, TokenType.Noun },
                new List<TokenType> { TokenType.AdjectiveList, TokenType.Noun },
                new List<TokenType> { TokenType.Noun }
            };

            // AdjectiveList ::= Adjective AdjectiveList | ε
            GrammarRules[TokenType.AdjectiveList] = new() {
                new List<TokenType> { TokenType.Adjective, TokenType.AdjectiveList },
                new List<TokenType>() // epsilon
            };

            // VerbPhrase ::= Verb Object Extra | Verb Object | Verb
            GrammarRules[TokenType.VerbPhrase] = new() {
                new List<TokenType> { TokenType.Verb, TokenType.Object, TokenType.Extra },
                new List<TokenType> { TokenType.Verb, TokenType.Object },
                new List<TokenType> { TokenType.Verb }
            };

            // Object ::= NounPhrase
            GrammarRules[TokenType.Object] = new() {
                new List<TokenType> { TokenType.NounPhrase }
            };

            // Extra ::= ε | Adverbial
            GrammarRules[TokenType.Extra] = new() {
                new List<TokenType>(), // epsilon
                new List<TokenType> { TokenType.Adverbial }
            };
        }

        // ---------------------------
        // Parser Logic
        // ---------------------------
        private bool IsNonTerminal(TokenType t) => GrammarRules.ContainsKey(t);

        public void Derive() {
            var current = new List<object> { TokenType.Story };
            PrintStep(current);

            while (current.Any(s => s is TokenType t && IsNonTerminal(t))) {
                int idx = current.FindIndex(s => s is TokenType t && IsNonTerminal(t));
                var nonTerminal = (TokenType)current[idx];

                // lookahead token
                var lookahead = position < tokens.Count ? tokens[position].Value : null;

                // choose production
                var production = GrammarRules[nonTerminal]
                    .FirstOrDefault(rule => RuleMatches(rule, lookahead))
                    ?? new List<TokenType>();

                // expand
                current.RemoveAt(idx);
                if (production.Count > 0)
                    current.InsertRange(idx, production.Cast<object>());

                // replace terminals if matched
                for (int i = idx; i < current.Count; i++) {
                    if (current[i] is TokenType t && IsTerminal(t)) {
                        if (position < tokens.Count && MatchesCFG(t, tokens[position].Value)) {
                            current[i] = $"\"{tokens[position].Value}\"";
                            position++;
                        }
                    }
                }

                PrintStep(current);
            }
        }

        private bool RuleMatches(List<TokenType> production, string lookahead) {
            if (production.Count == 0) return true; // epsilon
            var first = production[0];
            if (IsTerminal(first)) {
                return lookahead != null && MatchesCFG(first, lookahead);
            }
            return true; // assume match for nonterminals
        }

        private bool IsTerminal(TokenType type) {
            return type == TokenType.Determiner ||
                   type == TokenType.Adjective ||
                   type == TokenType.Noun ||
                   type == TokenType.Verb ||
                   type == TokenType.Preposition ||
                   type == TokenType.Adverbial ||
                   type == TokenType.Location ||
                   type == TokenType.Condition ||
                   type == TokenType.RelativeClause ||
                   type == TokenType.Conjunction ||
                   type == TokenType.Punctuation;
        }

        private bool MatchesCFG(TokenType type, string value) {
            return type switch {
                TokenType.Determiner => CFG.Determiners.Contains(value),
                TokenType.Adjective => CFG.Adjectives.Contains(value),
                TokenType.Noun => CFG.Nouns.Contains(value),
                TokenType.Verb => CFG.Verbs.Contains(value),
                TokenType.Preposition => CFG.Prepositions.Contains(value),
                TokenType.Adverbial => CFG.Adverbials.Contains(value),
                TokenType.Location => CFG.Locations.Contains(value),
                TokenType.Condition => CFG.Conditions.Contains(value),
                TokenType.RelativeClause => CFG.RelativeClauses.Contains(value),
                TokenType.Conjunction => CFG.Conjunctions.Contains(value),
                TokenType.Punctuation => CFG.Punctuation.Contains(value),
                _ => false
            };
        }

        private void PrintStep(List<object> sequence) {
            Console.WriteLine("=> " + string.Join(" ", sequence.Select(s =>
                s is TokenType t ? $"<{t}>" : s.ToString()
            )));
        }
    }
}
