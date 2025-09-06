/*
 * Project: Tokenizer Lexical Analyzer
 * Author: Rico Euma O. Aban
 *
 * Description:
 * The Tokenizer (lexer) converts a raw input string into a sequence of tokens. 
 * It identifies the token type for each word by checking against the CFG 
 * (Context-Free Grammar) definitions. It also separates punctuation into 
 * individual tokens.
 */

namespace Tokenizer {
    public static class Tokenizer {
        /// <summary>
        /// Splits input text into a list of tokens (words + punctuation).
        /// </summary>
        public static List<Token> Tokenize(string input) {
            var tokens = new List<Token>();
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var word in words) {
                // Strip punctuation temporarily
                string cleanWord = word.Trim().TrimEnd('.', ',', '!', '?');
                
                // Assign token type using CFG
                TokenType type = GetTokenType(cleanWord);
                tokens.Add(new Token(cleanWord, type));
                
                // If the word ended with punctuation, add it as a separate token
                if (word.EndsWith('.') || word.EndsWith(',') || word.EndsWith('!') || word.EndsWith('?')) {
                    tokens.Add(new Token(word[^1..], TokenType.Punctuation));
                }
            }
            
            return tokens;
        }

        /// <summary>
        /// Determines the token type for a given word by checking 
        /// the CFG word sets. If no match is found, assigns Unknown.
        /// </summary>
        private static TokenType GetTokenType(string word) {
            if (CFG.Determiners.Contains(word)) return TokenType.Determiner;
            if (CFG.Adjectives.Contains(word)) return TokenType.Adjective;
            if (CFG.Nouns.Contains(word)) return TokenType.Noun;
            if (CFG.Verbs.Contains(word)) return TokenType.Verb;
            if (CFG.Prepositions.Contains(word)) return TokenType.Preposition;
            if (CFG.Adverbials.Contains(word)) return TokenType.Adverbial;
            if (CFG.Locations.Contains(word)) return TokenType.Location;
            if (CFG.Conditions.Contains(word)) return TokenType.Condition;
            if (CFG.RelativeClauses.Contains(word)) return TokenType.RelativeClause;
            if (CFG.Conjunctions.Contains(word)) return TokenType.Conjunction;
            
            return TokenType.Unknown;
        }
    }
}

