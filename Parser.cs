/*
 * Project: Tokenizer Parser
 * Author: Rico Euma O. Aban
 *
 * Description:
 * The Parser is a recursive descent parser that takes a list of tokens
 * (produced by the Tokenizer) and attempts to validate them against 
 * a simplified Context-Free Grammar (CFG). It also prints out the 
 * leftmost derivation steps for educational and debugging purposes.
 */

namespace Tokenizer {
    public class Parser {
        private readonly List<Token> tokens;       // List of tokens provided by the Tokenizer
        private int tokenPosition = 0;             // Current position in the token stream
        private List<object> currentDerivation = new List<object>(); // Tracks ongoing derivation steps

        /// <summary>
        /// Initializes a new parser with a token sequence.
        /// </summary>
        public Parser(List<Token> tokens) {
            this.tokens = tokens;
        }

        /// <summary>
        /// Entry point for parsing. Starts derivation from <Story> and 
        /// attempts to fully parse the token sequence.
        /// </summary>
        public void Derive() {
            Console.WriteLine("<Story>");
            currentDerivation = new List<object> { TokenType.Sentence };
            PrintDerivation();
            
            try {
                ParseSentence();
                
                // Ensure all tokens are consumed
                if (tokenPosition < tokens.Count) {
                    throw new Exception($"Unexpected tokens remaining: {string.Join(" ", tokens.Skip(tokenPosition).Select(t => t.Value))}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Someone Didn't Read the Grammar Rules: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses a <Sentence> → <SimpleSentence>.
        /// </summary>
        private void ParseSentence() {
            ExpandNonTerminal(TokenType.Sentence, new List<object> { TokenType.SimpleSentence });
            ParseSimpleSentence();
        }

        /// <summary>
        /// Parses a <SimpleSentence> → <Subject> <VerbPhrase> <Extra>.
        /// </summary>
        private void ParseSimpleSentence() {
            ExpandNonTerminal(TokenType.SimpleSentence, new List<object> { TokenType.Subject, TokenType.VerbPhrase, TokenType.Extra });
            ParseSubject();
            ParseVerbPhrase();
            ParseExtra();
        }

        /// <summary>
        /// Parses a <Subject> → <NounPhrase>.
        /// </summary>
        private void ParseSubject() {
            ExpandNonTerminal(TokenType.Subject, new List<object> { TokenType.NounPhrase });
            ParseNounPhrase();
        }

        /// <summary>
        /// Parses a <NounPhrase> depending on its structure:
        ///     Determiner AdjectiveList Noun
        ///     AdjectiveList Noun
        ///     Noun
        /// </summary>
        private void ParseNounPhrase() {
            if (LookaheadIs(TokenType.Determiner)) {
                ExpandNonTerminal(TokenType.NounPhrase, new List<object> { TokenType.Determiner, TokenType.AdjectiveList, TokenType.Noun });
                Consume(TokenType.Determiner);
                ParseAdjectiveList();
                Consume(TokenType.Noun);
            }
            else if (LookaheadIs(TokenType.Adjective)) {
                ExpandNonTerminal(TokenType.NounPhrase, new List<object> { TokenType.AdjectiveList, TokenType.Noun });
                ParseAdjectiveList();
                Consume(TokenType.Noun);
            }
            else if (LookaheadIs(TokenType.Noun)) {
                ExpandNonTerminal(TokenType.NounPhrase, new List<object> { TokenType.Noun });
                Consume(TokenType.Noun);
            }
            else {
                throw new Exception($"Expected noun phrase at position {tokenPosition}");
            }
        }

        /// <summary>
        /// Parses an <AdjectiveList> recursively or epsilon (empty).
        /// </summary>
        private void ParseAdjectiveList() {
            if (LookaheadIs(TokenType.Adjective)) {
                ExpandNonTerminal(TokenType.AdjectiveList, new List<object> { TokenType.Adjective, TokenType.AdjectiveList });
                Consume(TokenType.Adjective);
                ParseAdjectiveList();
            }
            else {
                ExpandNonTerminal(TokenType.AdjectiveList, new List<object>()); // epsilon
            }
        }

        /// <summary>
        /// Parses a <VerbPhrase>, optionally followed by an <Object> and <Extra>.
        /// </summary>
        private void ParseVerbPhrase() {
            if (LookaheadIs(TokenType.Verb)) {
                bool hasObject = HasObjectAfterCurrentVerb();
                
                if (hasObject) {
                    ExpandNonTerminal(TokenType.VerbPhrase, new List<object> { TokenType.Verb, TokenType.Object, TokenType.Extra });
                    Consume(TokenType.Verb);
                    ParseObject();
                    ParseExtra();
                }
                else {
                    ExpandNonTerminal(TokenType.VerbPhrase, new List<object> { TokenType.Verb });
                    Consume(TokenType.Verb);
                }
            }
            else {
                throw new Exception($"Expected verb at position {tokenPosition}");
            }
        }

        /// <summary>
        /// Parses an <Object> → <NounPhrase>.
        /// </summary>
        private void ParseObject() {
            ExpandNonTerminal(TokenType.Object, new List<object> { TokenType.NounPhrase });
            ParseNounPhrase();
        }

        /// <summary>
        /// Parses optional <Extra> → <Adverbial> or epsilon.
        /// </summary>
        private void ParseExtra() {
            if (LookaheadIs(TokenType.Adverbial)) {
                ExpandNonTerminal(TokenType.Extra, new List<object> { TokenType.Adverbial });
                Consume(TokenType.Adverbial);
            }
            else {
                ExpandNonTerminal(TokenType.Extra, new List<object>()); // epsilon
            }
        }

        /// <summary>
        /// Expands a non-terminal in the derivation sequence into its production rule.
        /// </summary>
        private void ExpandNonTerminal(TokenType nonTerminal, List<object> production) {
            int index = FindNonTerminalInDerivation(nonTerminal);
            if (index == -1) {
                throw new Exception($"Could not find non-terminal {nonTerminal} in derivation");
            }

            currentDerivation.RemoveAt(index);
            currentDerivation.InsertRange(index, production);
            PrintDerivation();
        }

        /// <summary>
        /// Locates the position of a non-terminal in the current derivation.
        /// </summary>
        private int FindNonTerminalInDerivation(TokenType nonTerminal) {
            for (int i = 0; i < currentDerivation.Count; i++) {
                if (currentDerivation[i] is TokenType type && type == nonTerminal) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks ahead in the tokens to determine if a verb has an object.
        /// </summary>
        private bool HasObjectAfterCurrentVerb() {
            int tempPos = tokenPosition + 1;
            while (tempPos < tokens.Count) {
                var token = tokens[tempPos];
                if (token.Type == TokenType.Determiner || 
                    token.Type == TokenType.Adjective || 
                    token.Type == TokenType.Noun) {
                    return true;
                }
                if (token.Type == TokenType.Adverbial || 
                    token.Type == TokenType.Punctuation) {
                    return false;
                }
                tempPos++;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the next token matches the expected type.
        /// </summary>
        private bool LookaheadIs(TokenType type) {
            return tokenPosition < tokens.Count && tokens[tokenPosition].Type == type;
        }

        /// <summary>
        /// Consumes a token if it matches the expected type.
        /// Throws an exception if the match fails.
        /// </summary>
        private void Consume(TokenType expectedType) {
            if (tokenPosition >= tokens.Count) {
                throw new Exception($"Unexpected end of input, expected {expectedType}");
            }

            Token current = tokens[tokenPosition];
            if (current.Type != expectedType && current.Type != TokenType.Unknown) {
                throw new Exception($"Expected {expectedType}, but found {current.Type} '{current.Value}' at position {tokenPosition}");
            }

            // Replace terminal with actual token in derivation
            int index = FindTerminalInDerivation(expectedType);
            if (index == -1) {
                throw new Exception($"Could not find terminal {expectedType} in derivation");
            }

            currentDerivation[index] = $"\"{current.Value}\"";
            PrintDerivation();
            tokenPosition++;
        }

        /// <summary>
        /// Finds the location of a terminal symbol in the current derivation.
        /// </summary>
        private int FindTerminalInDerivation(TokenType terminal) {
            for (int i = 0; i < currentDerivation.Count; i++) {
                if (currentDerivation[i] is TokenType type && type == terminal) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Prints the current derivation sequence in readable form.
        /// </summary>
        private void PrintDerivation() {
            Console.WriteLine("=> " + string.Join(" ", currentDerivation.Select(item =>
                item is TokenType type ? $"<{type}>" : item.ToString()
            )));
        }
    }
}

