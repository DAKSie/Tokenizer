namespace Tokenizer {
    public class Deriver {
        private readonly Dictionary<TokenType, List<List<(TokenType, string)>>> grammar;

        public Deriver() {
            grammar = new Dictionary<TokenType, List<List<(TokenType, string)>>>();

            // Sentence rules
            grammar.Add(TokenType.Sentence, new List<List<(TokenType, string)>> {
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
                // compound sentence
                new List<(TokenType, string)> {
                    (TokenType.Sentence, null),
                    (TokenType.Conjunction, null),
                    (TokenType.Sentence, null)
                }
            });

            // NounPhrase rules (include relative clause forms)
            grammar.Add(TokenType.NounPhrase, new List<List<(TokenType, string)>> {
                // "knight who rides a horse" -> Noun RelativeClause Verb NounPhrase
                new List<(TokenType, string)> {
                    (TokenType.Noun, null),
                    (TokenType.RelativeClause, null),
                    (TokenType.Verb, null),
                    (TokenType.NounPhrase, null)
                },
                // alternative relative clause pattern that allows a full verb phrase after 'who'
                new List<(TokenType, string)> {
                    (TokenType.Noun, null),
                    (TokenType.RelativeClause, null),
                    (TokenType.VerbPhrase, null)
                },
                // standard NPs
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
            });

            // VerbPhrase rules (include chained VPs and Verb + NP + Extra)
            grammar.Add(TokenType.VerbPhrase, new List<List<(TokenType, string)>> {
                new List<(TokenType, string)> {
                    (TokenType.Verb, null),
                    (TokenType.NounPhrase, null)
                },
                new List<(TokenType, string)> {
                    (TokenType.Verb, null),
                    (TokenType.NounPhrase, null),
                    (TokenType.Extra, null)
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
                // chained verb phrases: left side VP, then conjunction-like, then right side VP
                new List<(TokenType, string)> {
                    (TokenType.VerbPhrase, null),
                    (TokenType.Conjunction, null),
                    (TokenType.VerbPhrase, null)
                }
            });

            // Extra (location / condition / adverbial)
            grammar.Add(TokenType.Extra, new List<List<(TokenType, string)>> {
                new List<(TokenType, string)> { (TokenType.Location, null) },
                new List<(TokenType, string)> { (TokenType.Condition, null) },
                new List<(TokenType, string)> { (TokenType.Adverbial, null) }
            });

            // Lexical categories -> turn CFG entries into terminal productions
            grammar.Add(TokenType.Determiner, CFG.Determiners
                .Select(w => new List<(TokenType, string)>{ (TokenType.DeterminerWord, w) })
                .ToList());

            grammar.Add(TokenType.Adjective, CFG.Adjectives
                .Select(w => new List<(TokenType, string)>{ (TokenType.AdjectiveWord, w) })
                .ToList());

            grammar.Add(TokenType.Noun, CFG.Nouns
                .Select(w => new List<(TokenType, string)>{ (TokenType.NounWord, w) })
                .ToList());

            grammar.Add(TokenType.Verb, CFG.Verbs
                .Select(w => new List<(TokenType, string)>{ (TokenType.VerbWord, w) })
                .ToList());

            grammar.Add(TokenType.Preposition, CFG.Prepositions
                .Select(w => new List<(TokenType, string)>{ (TokenType.PrepositionWord, w) })
                .ToList());

            grammar.Add(TokenType.Adverbial, CFG.Adverbials
                .Select(w => new List<(TokenType, string)>{ (TokenType.AdverbWord, w) })
                .ToList());

            grammar.Add(TokenType.Location, CFG.Locations
                .Select(w => new List<(TokenType, string)>{ (TokenType.LocationWord, w) })
                .ToList());

            grammar.Add(TokenType.Condition, CFG.Conditions
                .Select(w => new List<(TokenType, string)>{ (TokenType.ConditionWord, w) })
                .ToList());

            // RELATIVE CLAUSE: lexical terminal(s)
            // Tokenizer produces TokenType.RelativeClause for "who" (and maybe others)
            grammar.Add(TokenType.RelativeClause, new List<List<(TokenType, string)>> {
                new List<(TokenType, string)> { (TokenType.RelativeClause, "who") }
            });

            // CONJUNCTIONS: build explicit productions for word and "word + adverbial" variants
            var conjProds = new List<List<(TokenType, string)>>();
            foreach (var w in CFG.Conjunctions) {
                // plain terminal (Conjunction, "and")
                conjProds.Add(new List<(TokenType, string)>{ (TokenType.Conjunction, w) });
                // allow "and then" / "and quickly" style as Conjunction followed by an adverbial nonterminal
                // This produces: (Conjunction,"and") (Adverbial,null)  -> later Adverbial expands to the actual adverbial tokens
                conjProds.Add(new List<(TokenType, string)>{ (TokenType.Conjunction, w), (TokenType.Adverbial, null) });
            }
            grammar.Add(TokenType.Conjunction, conjProds);

            // Punctuation
            grammar.Add(TokenType.Punctuation, CFG.Punctuation
                .Select(w => new List<(TokenType, string)>{ (TokenType.Punctuation, w) })
                .ToList());
        }

        public List<string> Derive(string input) {
            var tokens = Tokenizer.Tokenize(input); // use your Tokenizer
            var derivation = new List<string>();

            var current = new List<(TokenType, string)> { (TokenType.Sentence, null) };
            derivation.Add(Format(current));

            // visited set to avoid re-exploring identical sentential forms
            var visited = new HashSet<string>();
            // increase max depth for more complex sentences
            if (Expand(current, tokens, derivation, 0, 2000, visited))
                return derivation;

            return new List<string> { "No derivation found." };
        }

        // DFS with visited memoization + depth limit + prefix pruning
        private bool Expand(List<(TokenType, string)> sentential,
                            List<Token> tokens,
                            List<string> derivation,
                            int depth,
                            int maxDepth,
                            HashSet<string> visited) {
            if (depth > maxDepth) return false;

            // base case: all terminals -> compare token strings
            if (sentential.All(s => s.Item2 != null)) {
                if (sentential.Select(s => s.Item2)
                    .SequenceEqual(tokens.Select(t => t.Value), StringComparer.OrdinalIgnoreCase))
                    return true;
                return false;
            }

            // visited key (shape + terminals) - prevents revisiting identical states
            string key = KeyFor(sentential);
            if (visited.Contains(key)) return false;
            visited.Add(key);

            // leftmost nonterminal
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
                if (Expand(next, tokens, derivation, depth + 1, maxDepth, visited))
                    return true;
                derivation.RemoveAt(derivation.Count - 1);
            }

            return false;
        }

        // simple prefix check: all produced terminals must match token prefix
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
                    ? $"<{s.Item1}>"
                    : s.Item2 // only print the word
            ));
        }

        private string KeyFor(List<(TokenType, string)> sentential) {
            // stable serialization of a sentential form for visited set
            return string.Join("|", sentential.Select(s =>
                s.Item2 == null ? $"<{s.Item1}>" : $"{s.Item1}={s.Item2}"
            ));
        }
    }
}
