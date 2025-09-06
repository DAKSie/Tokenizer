/*
 * Project: Tokenizer Token Class
 * Author: Rico Euma O. Aban
 *
 * Description:
 * Represents an individual token in the input text. 
 * Each token has a string value (the actual word or symbol) 
 * and a TokenType (its grammatical classification).
 */

namespace Tokenizer {
    public class Token {
        /// <summary>
        /// The literal value of the token (e.g., "hero", "the", ".").
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The grammatical category or role of the token.
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Creates a new token with a given value and type.
        /// </summary>
        public Token(string value, TokenType type) {
            Value = value;
            Type = type;
        }

        /// <summary>
        /// Returns a string representation of the token in the form:
        /// "value <TokenType>"
        /// </summary>
        public override string ToString() => $"{Value} <{Type}>";
    }
}

