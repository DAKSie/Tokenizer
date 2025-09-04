namespace Tokenizer {
    public static class Tokenizer {
        public static List<Token> Tokenize(string input) {
            var tokens = new List<Token>();
            var words = SplitInput(input);
            
            for (int i = 0; i < words.Count; i++) {
                string word = words[i];

                // Multiu Word Tokens (verbs, adverbials, locations, conditions)
                string multi = TryMatchMulti(words, i, CFG.Verbs);
                if (multi != null) {
                    tokens.Add(new Token(multi, TokenType.Verb));
                    i += multi.Split(' ').Length - 1;
                    continue;
                }
                multi = TryMatchMulti(words, i, CFG.Adverbials);
                if (multi != null) {
                    tokens.Add(new Token(multi, TokenType.Adverbial));
                    i += multi.Split(' ').Length - 1;
                    continue;
                }
                multi = TryMatchMulti(words, i, CFG.Locations);
                if (multi != null) {
                    tokens.Add(new Token(multi, TokenType.Location));
                    i += multi.Split(' ').Length - 1;
                    continue;
                }
                multi = TryMatchMulti(words, i, CFG.Conditions);
                if (multi != null) {
                    tokens.Add(new Token(multi, TokenType.Condition));
                    i += multi.Split(' ').Length - 1;
                    continue;
                }

                // Single word tokens
                
                if (CFG.Determiners.Contains(word))
                    tokens.Add(new Token(word, TokenType.Determiner));
                else if (CFG.Adjectives.Contains(word))
                    tokens.Add(new Token(word, TokenType.Adjective));
                else if (CFG.Nouns.Contains(word))
                    tokens.Add(new Token(word, TokenType.Noun));
                else if (word == "who")
                    tokens.Add(new Token(word, TokenType.RelativeClause));
                else if (CFG.Prepositions.Contains(word))
                    tokens.Add(new Token(word, TokenType.Preposition));
                else if (CFG.Conjunctions.Contains(word))
                    tokens.Add(new Token(word, TokenType.Conjunction));
                else if (CFG.Punctuation.Contains(word))
                    tokens.Add(new Token(word, TokenType.Punctuation));
                else
                    tokens.Add(new Token(word, TokenType.Unknown));
                }
            return tokens;
        }

        private static List<string> SplitInput(string input) {
            // Splitting with space as delimitter
            // Also separating punctuations in our grammar

            var result = new List<string>();
            var parts = input.Split(' ');

            foreach (var part in parts) {
                string p = part.Trim();

                if (string.IsNullOrEmpty(p)) continue;

                foreach (var punct in CFG.Punctuation) {
                    if (p.EndsWith(punct)) {
                        string w = p.Substring(0, p.Length - punct.Length);
                        if (!string.IsNullOrEmpty(w)) result.Add(w);
                        result.Add(punct);
                        goto Next;
                    }
                }
                result.Add(p);
                Next:; 
            }
            return result;
        }

        public static string TryMatchMulti(List<string> words, int start, HashSet<string> options) {
            foreach (var opt in options) {
                var optWords = opt.Split(' ');
                if (start + optWords.Length > words.Count) continue;
                bool match = true;
                for (int j = 0; j < optWords.Length; j++) {
                    if (!string.Equals(words[start + j], optWords[j], StringComparison.OrdinalIgnoreCase)) {
                        match = false;
                        break;
                    }
                }
                if (match) return opt;
            }
            return null;
        }
    }
}

