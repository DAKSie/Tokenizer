namespace Tokenizer {
    /// <summary>
    /// The <c>Tokenizer</c> breaks raw input strings into a sequence of <see cref="Token"/>s.
    /// It uses lexical categories defined in <see cref="CFG"/> such as Noun, Verb, Location, etc.
    /// 
    /// Key features:
    /// - Uses the "longest match" principle for multi-word tokens (e.g., "in the dark forest").
    /// - Falls back to single-word classification if no multi-word match is found.
    /// - Extracts punctuation (.,!?), assigning them to <see cref="TokenType.Punctuation"/>.
    /// </summary>
    public static class Tokenizer {
        /// <summary>
        /// Converts a raw input string into a list of tokens.
        /// 
        /// Process:
        /// 1. Remove leading/trailing spaces.
        /// 2. Attempt multi-word matches first (Locations, Adverbials, Conditions, Verbs).
        ///    - Longest match is prioritized to avoid partial captures.
        ///    - Example: "searches for" → Verb (not "searches" + "for").
        /// 3. If no multi-word match exists, split into words and classify individually.
        /// 4. Handle punctuation as separate tokens.
        /// 
        /// Example:
        /// Input: "The mighty hero fights the dragon in the dark forest."
        /// Output:
        /// - "The" &lt;Determiner&gt;
        /// - "mighty" &lt;Adjective&gt;
        /// - "hero" &lt;Noun&gt;
        /// - "fights" &lt;Verb&gt;
        /// - "the" &lt;Determiner&gt;
        /// - "dragon" &lt;Noun&gt;
        /// - "in the dark forest" &lt;Location&gt;
        /// - "." &lt;Punctuation&gt;
        /// </summary>
        /// <param name="input">The input sentence or story string.</param>
        /// <returns>A list of <see cref="Token"/> objects in order.</returns>
        public static List<Token> Tokenize(string input) {
            var tokens = new List<Token>();
            string remaining = input.Trim();
            
            while (!string.IsNullOrEmpty(remaining)) {
                bool found = false;
                
                // --- Multi-word tokens (longest match first) ---
                foreach (var location in CFG.Locations.OrderByDescending(l => l.Length)) {
                    if (remaining.StartsWith(location, StringComparison.OrdinalIgnoreCase)) {
                        tokens.Add(new Token(location, TokenType.Location));
                        remaining = remaining[location.Length..].Trim();
                        found = true;
                        break;
                    }
                }
                if (found) continue;

                foreach (var adverbial in CFG.Adverbials.OrderByDescending(a => a.Length)) {
                    if (remaining.StartsWith(adverbial, StringComparison.OrdinalIgnoreCase)) {
                        tokens.Add(new Token(adverbial, TokenType.Adverbial));
                        remaining = remaining[adverbial.Length..].Trim();
                        found = true;
                        break;
                    }
                }
                if (found) continue;

                foreach (var condition in CFG.Conditions.OrderByDescending(c => c.Length)) {
                    if (remaining.StartsWith(condition, StringComparison.OrdinalIgnoreCase)) {
                        tokens.Add(new Token(condition, TokenType.Condition));
                        remaining = remaining[condition.Length..].Trim();
                        found = true;
                        break;
                    }
                }
                if (found) continue;

                foreach (var verb in CFG.Verbs.OrderByDescending(v => v.Length)) {
                    if (remaining.StartsWith(verb, StringComparison.OrdinalIgnoreCase)) {
                        tokens.Add(new Token(verb, TokenType.Verb));
                        remaining = remaining[verb.Length..].Trim();
                        found = true;
                        break;
                    }
                }
                if (found) continue;
                
                // --- Single-word token fallback ---
                var words = remaining.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string currentWord = words[0].Trim();
                
                TokenType type = GetTokenType(currentWord);
                tokens.Add(new Token(currentWord, type));
                
                remaining = words.Length > 1 ? words[1].Trim() : "";
                
                // --- Handle punctuation ---
                if (remaining.StartsWith(".") || remaining.StartsWith(",") || 
                    remaining.StartsWith("!") || remaining.StartsWith("?")) {
                    tokens.Add(new Token(remaining[0..1], TokenType.Punctuation));
                    remaining = remaining[1..].Trim();
                }
            }
            
            return tokens;
        }

        /// <summary>
        /// Classifies a single word into its <see cref="TokenType"/> using <see cref="CFG"/>.
        /// Returns <see cref="TokenType.Unknown"/> if the word does not match any category.
        /// </summary>
        /// <param name="word">The word to classify.</param>
        /// <returns>The detected <see cref="TokenType"/>.</returns>
        private static TokenType GetTokenType(string word) {
            if (CFG.Determiners.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Determiner;
            if (CFG.Adjectives.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Adjective;
            if (CFG.Nouns.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Noun;
            if (CFG.Verbs.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Verb;
            if (CFG.Prepositions.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Preposition;
            if (CFG.Adverbials.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Adverbial;
            if (CFG.Locations.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Location;
            if (CFG.Conditions.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Condition;
            if (CFG.RelativeClauses.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.RelativeClause;
            if (CFG.Conjunctions.Contains(word, StringComparer.OrdinalIgnoreCase)) return TokenType.Conjunction;
            
            return TokenType.Unknown;
        }
    }
}

