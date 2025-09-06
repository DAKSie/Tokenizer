namespace Tokenizer {
    /// <summary>
    /// A recursive descent parser for the given grammar.
    /// It consumes a list of tokens produced by the Tokenizer
    /// and prints leftmost derivations as it parses.
    /// </summary>
    public class Parser {
        private readonly List<Token> tokens;   // Input tokens from the tokenizer
        private int tokenPosition = 0;         // Current index in the token list
        private List<object> currentDerivation = new List<object>(); // Tracks current derivation for printing

        /// <summary>
        /// Initializes the parser with a sequence of tokens.
        /// </summary>
        public Parser(List<Token> tokens) {
            this.tokens = tokens;
        }

        /// <summary>
        /// Entry point for parsing.
        /// Starts with <Story> and attempts to derive a valid parse.
        /// Prints each step of the derivation.
        /// </summary>
        public void Derive() {
            Console.WriteLine("<Story>");
            currentDerivation = new List<object> { TokenType.Sentence };
            PrintDerivation();
            
            try {
                ParseSentence();
                
                // Handle remaining tokens (Extra or punctuation at the end)
                if (tokenPosition < tokens.Count) {
                    while (tokenPosition < tokens.Count) {
                        if (LookaheadIs(TokenType.Location) || LookaheadIs(TokenType.Condition) || 
                            LookaheadIs(TokenType.Adverbial) || LookaheadIs(TokenType.Punctuation)) {
                            ParseExtra();
                        } else {
                            throw new Exception($"e = {string.Join(" ", tokens.Skip(tokenPosition).Select(t => t.Value))}");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"e = {ex.Message}");
            }
        }

        /// <summary>
        /// <Sentence> ::= <SimpleSentence>
        /// </summary>
        private void ParseSentence() {
            ExpandNonTerminal(TokenType.Sentence, new List<object> { TokenType.SimpleSentence });
            ParseSimpleSentence();
        }

        /// <summary>
        /// <SimpleSentence> ::= <Subject> <VerbPhrase> <Extra>
        /// </summary>
        private void ParseSimpleSentence() {
            ExpandNonTerminal(TokenType.SimpleSentence, new List<object> { TokenType.Subject, TokenType.VerbPhrase, TokenType.Extra });
            ParseSubject();
            ParseVerbPhrase();
            ParseExtra();
        }

        /// <summary>
        /// <Subject> ::= <NounPhrase>
        /// </summary>
        private void ParseSubject() {
            ExpandNonTerminal(TokenType.Subject, new List<object> { TokenType.NounPhrase });
            ParseNounPhrase();
        }

        /// <summary>
        /// <NounPhrase> ::= 
        ///   <Determiner> <AdjectiveList> <Noun>
        /// | <AdjectiveList> <Noun>
        /// | <Noun>
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
        /// <AdjectiveList> ::= 
        ///   <Adjective> <AdjectiveList>
        /// | ε
        /// (Recursive rule)
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
        /// <VerbPhrase> ::= 
        ///   <Verb> <Object> <Extra>
        /// | <Verb> <Extra>
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
                    ExpandNonTerminal(TokenType.VerbPhrase, new List<object> { TokenType.Verb, TokenType.Extra });
                    Consume(TokenType.Verb);
                    ParseExtra();
                }
            }
            else {
                throw new Exception($"Expected verb at position {tokenPosition}");
            }
        }

        /// <summary>
        /// <Object> ::= <NounPhrase>
        /// </summary>
        private void ParseObject() {
            ExpandNonTerminal(TokenType.Object, new List<object> { TokenType.NounPhrase });
            ParseNounPhrase();
        }

        /// <summary>
        /// <Extra> ::= 
        ///   <Location> 
        /// | <Condition> 
        /// | <Adverbial> 
        /// | <Punctuation> 
        /// | ε
        /// </summary>
        private void ParseExtra() {
            if (LookaheadIs(TokenType.Location)) {
                ExpandNonTerminal(TokenType.Extra, new List<object> { TokenType.Location });
                Consume(TokenType.Location);
            }
            else if (LookaheadIs(TokenType.Condition)) {
                ExpandNonTerminal(TokenType.Extra, new List<object> { TokenType.Condition });
                Consume(TokenType.Condition);
            }
            else if (LookaheadIs(TokenType.Adverbial)) {
                ExpandNonTerminal(TokenType.Extra, new List<object> { TokenType.Adverbial });
                Consume(TokenType.Adverbial);
            }
            else if (LookaheadIs(TokenType.Punctuation)) {
                ExpandNonTerminal(TokenType.Extra, new List<object> { TokenType.Punctuation });
                Consume(TokenType.Punctuation);
            }
            else {
                ExpandNonTerminal(TokenType.Extra, new List<object>()); // epsilon
            }
        }

        /// <summary>
        /// Replaces a non-terminal in the current derivation with a production.
        /// </summary>
        private void ExpandNonTerminal(TokenType nonTerminal, List<object> production) {
            int index = FindNonTerminalInDerivation(nonTerminal);
            if (index == -1) {
                throw new Exception($"non-terminal missing {nonTerminal}");
            }

            currentDerivation.RemoveAt(index);
            currentDerivation.InsertRange(index, production);
            PrintDerivation();
        }

        /// <summary>
        /// Finds a non-terminal in the current derivation sequence.
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
        /// Checks ahead to see if the next tokens form an <Object>.
        /// Helps decide between <Verb> <Object> and <Verb> only.
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
                if (token.Type == TokenType.Location || 
                    token.Type == TokenType.Condition ||
                    token.Type == TokenType.Adverbial ||
                    token.Type == TokenType.Punctuation ||
                    token.Type == TokenType.Verb) {
                    return false;
                }
                tempPos++;
            }
            return false;
        }

        /// <summary>
        /// Checks if the current token matches a given type.
        /// </summary>
        private bool LookaheadIs(TokenType type) {
            return tokenPosition < tokens.Count && tokens[tokenPosition].Type == type;
        }

        /// <summary>
        /// Consumes a token of the expected type and updates the derivation.
        /// Throws an exception if the token does not match.
        /// </summary>
        private void Consume(TokenType expectedType) {
            if (tokenPosition >= tokens.Count) {
                throw new Exception($"expected {expectedType}");
            }

            Token current = tokens[tokenPosition];
            if (current.Type != expectedType && current.Type != TokenType.Unknown) {
                throw new Exception($"Expected {expectedType}, but found {current.Type} '{current.Value}' at position {tokenPosition}");
            }

            int index = FindTerminalInDerivation(expectedType);
            if (index == -1) {
                throw new Exception($"terminal missing {expectedType}");
            }

            currentDerivation[index] = $"\"{current.Value}\"";
            PrintDerivation();
            tokenPosition++;
        }

        /// <summary>
        /// Finds a terminal symbol in the current derivation sequence.
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
        /// Prints the current state of the derivation.
        /// Non-terminals are shown in angle brackets.
        /// Terminals are shown as quoted strings.
        /// </summary>
        private void PrintDerivation() {
            Console.WriteLine("=> " + string.Join(" ", currentDerivation.Select(item =>
                item is TokenType type ? $"<{type}>" : item.ToString()
            )));
        }
    }
}

