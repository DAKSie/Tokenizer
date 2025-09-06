namespace Tokenizer {
    public class Deriver {
        private readonly Dictionary<TokenType, List<List<(TokenType, string)>>> grammar;

        public Deriver() {
            grammar = new Dictionary<TokenType, List<List<(TokenType, string)>>>() {
                {
                    TokenType.Sentence, new List<List<(TokenType, string)>> {
                        new List<(TokenType, string)> {
                            (TokenType.NounPhrase, null),
                            (TokenType.VerbPhrase, null),
                            (TokenType.Extra, null),
                            (TokenType.Punctuation, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.NounPhrase, null),
                            (TokenType.VerbPhrase, null),
                            (TokenType.Punctuation, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Sentence, null),
                            (TokenType.Conjunction, null),
                            (TokenType.Sentence, null)
                        }
                    }
                },

                {
                    TokenType.NounPhrase, new List<List<(TokenType, string)>> {
                        new List<(TokenType, string)> {
                            (TokenType.Noun, null),
                            (TokenType.RelativeClause, null),
                            (TokenType.Verb, null),
                            (TokenType.NounPhrase, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Noun, null),
                            (TokenType.RelativeClause, null),
                            (TokenType.VerbPhrase, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Determiner, null),
                            (TokenType.Adjective, null),
                            (TokenType.Noun, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Determiner, null),
                            (TokenType.Noun, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Noun, null)
                        }
                    }
                },

                {
                    TokenType.VerbPhrase, new List<List<(TokenType, string)>> {
                        new List<(TokenType, string)> {
                            (TokenType.Verb, null),
                            (TokenType.NounPhrase, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Verb, null),
                            (TokenType.Location, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Verb, null),
                            (TokenType.Adverbial, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.Verb, null)
                        },
                        new List<(TokenType, string)> {
                            (TokenType.VerbPhrase, null),
                            (TokenType.Conjunction, null),
                            (TokenType.VerbPhrase, null)
                        }
                    }
                },

                {
                    TokenType.Extra, new List<List<(TokenType, string)>> {
                        new List<(TokenType, string)> { (TokenType.Location, null) },
                        new List<(TokenType, string)> { (TokenType.Condition, null) },
                        new List<(TokenType, string)> { (TokenType.Adverbial, null) }
                    }
                },

                // Lexical categories from CFG.cs → TokenType
                { TokenType.Determiner, CFG.Determiners.Select(w =>
                    new List<(TokenType, string)> { (TokenType.DeterminerWord, w) }).ToList()
                },
                { TokenType.Adjective, CFG.Adjectives.Select(w =>
                    new List<(TokenType, string)> { (TokenType.AdjectiveWord, w) }).ToList()
                },
                { TokenType.Noun, CFG.Nouns.Select(w =>
                    new List<(TokenType, string)> { (TokenType.NounWord, w) }).ToList()
                },
                { TokenType.Verb, CFG.Verbs.Select(w =>
                    new List<(TokenType, string)> { (TokenType.VerbWord, w) }).ToList()
                },
                { TokenType.Preposition, CFG.Prepositions.Select(w =>
                    new List<(TokenType, string)> { (TokenType.PrepositionWord, w) }).ToList()
                },
                { TokenType.Adverbial, CFG.Adverbials.Select(w =>
                    new List<(TokenType, string)> { (TokenType.AdverbWord, w) }).ToList()
                },
                { TokenType.Location, CFG.Locations.Select(w =>
                    new List<(TokenType, string)> { (TokenType.LocationWord, w) }).ToList()
                },
                { TokenType.Condition, CFG.Conditions.Select(w =>
                    new List<(TokenType, string)> { (TokenType.ConditionWord, w) }).ToList()
                },


                { TokenType.Conjunction,
                    CFG.Conjunctions.Select(w =>
                        new List<(TokenType, string)> { (TokenType.Conjunction, w) }
                    ).ToList()
                    .Concat(new List<List<(TokenType, string)>> {
                        new List<(TokenType, string)> { (TokenType.Conjunction, "and"), (TokenType.Adverbial, "then") }
                    }).ToList()
                },


                { TokenType.Punctuation, CFG.Punctuation.Select(w =>
                    new List<(TokenType, string)> { (TokenType.Punctuation, w) }).ToList()
                }
            };
        }

        public List<string> Derive(string input) {
            var tokens = Tokenizer.Tokenize(input); // ✅ use your Tokenizer
            var derivation = new List<string>();

            var current = new List<(TokenType, string)> { (TokenType.Sentence, null) };
            derivation.Add(Format(current));

            if (Expand(current, tokens, derivation, 0, 200))
                return derivation;

            return new List<string> { "No derivation found." };
        }

        private bool Expand(List<(TokenType, string)> sentential, List<Token> tokens, List<string> derivation, int depth, int maxDepth) {
            if (depth > maxDepth) return false;

            // ✅ base case: all terminals + exact match
            if (sentential.All(s => s.Item2 != null)) {
                if (sentential.Select(s => s.Item2)
                    .SequenceEqual(tokens.Select(t => t.Value), StringComparer.OrdinalIgnoreCase))
                    return true;
                return false;
            }

            // Expand leftmost nonterminal
            int idx = sentential.FindIndex(s => s.Item2 == null);
            var (nonTerm, _) = sentential[idx];

            if (!grammar.ContainsKey(nonTerm))
                return false;

            foreach (var production in grammar[nonTerm]) {
                var next = new List<(TokenType, string)>();
                next.AddRange(sentential.Take(idx));
                next.AddRange(production);
                next.AddRange(sentential.Skip(idx + 1));

                if (!PrefixMatches(next, tokens))
                    continue;

                derivation.Add(Format(next));

                if (Expand(next, tokens, derivation, depth + 1, maxDepth))
                    return true;

                derivation.RemoveAt(derivation.Count - 1);
            }

            return false;
        }

        private bool PrefixMatches(List<(TokenType, string)> sentential, List<Token> tokens) {
            var generated = sentential
                .Where(s => s.Item2 != null)
                .Select(s => s.Item2)
                .ToList();

            if (generated.Count > tokens.Count) return false;

            for (int i = 0; i < generated.Count; i++) {
                if (!string.Equals(generated[i], tokens[i].Value, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }

        private string Format(List<(TokenType, string)> symbols) {
            return string.Join(" ", symbols.Select(s =>
                s.Item2 == null
                    ? $"<{s.Item1}>"  // still show non-terminals like <Sentence>
                    : s.Item2         // only show the word, e.g. "The"
            ));
        }
    }
}

