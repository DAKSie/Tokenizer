namespace Tokenizer {
    /// <summary>
    /// Defines the Context-Free Grammar (CFG) terminals for the story language.
    /// Each category corresponds to a lexical class of tokens (e.g., Determiners, Nouns).
    /// These sets are used by the tokenizer to classify input words and phrases.
    /// </summary>
    public static class CFG {
        /// <summary>
        /// <Determiner> ::= "The" | "A" | "An" | "My" | "Her" | "His"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Determiners =
            new(StringComparer.OrdinalIgnoreCase) { 
                "The", "A", "An", "My", "Her", "His", 
                "the", "a", "an", "my", "her", "his" 
            };

        /// <summary>
        /// <Adjective> ::= "brave" | "old" | "young" | "rusty" | "enchanted" | "mighty" | "dark"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Adjectives =
            new(StringComparer.OrdinalIgnoreCase) { 
                "brave", "old", "young", "rusty", "enchanted", "mighty", "dark" 
            };

        /// <summary>
        /// <Noun> ::= "hero" | "wizard" | "knight" | "dragon" | "princess" 
        ///          | "treasure" | "cave" | "sword" | "castle" | "villager" 
        ///          | "horse" | "forest"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Nouns =
            new(StringComparer.OrdinalIgnoreCase) { 
                "hero", "wizard", "knight", "dragon", "princess", 
                "treasure", "cave", "sword", "castle", "villager", 
                "horse", "forest" 
            };

        /// <summary>
        /// <Verb> ::= "fights" | "searches for" | "rescues" | "discovers" 
        ///          | "rides" | "finds" | "opens" | "calls" | "holds" | "protects"
        /// Supports both single-word and multi-word verbs.
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Verbs =
            new(StringComparer.OrdinalIgnoreCase) { 
                "fights", "searches for", "rescues", "discovers", "rides", 
                "finds", "opens", "calls", "holds", "protects" 
            };

        /// <summary>
        /// <Preposition> ::= "in" | "on" | "at" | "under" 
        ///                  | "inside" | "near" | "above" | "beside"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Prepositions =
            new(StringComparer.OrdinalIgnoreCase) { 
                "in", "on", "at", "under", "inside", "near", "above", "beside" 
            };

        /// <summary>
        /// <RelativeClause> ::= "who"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> RelativeClauses = 
            new(StringComparer.OrdinalIgnoreCase) { "who" };

        /// <summary>
        /// <Adverbial> ::= "quickly" | "silently" | "with care" 
        ///                | "without warning" | "while the moon rises" 
        ///                | "before the sun sets"
        /// Supports both single-word and multi-word adverbials.
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Adverbials =
            new(StringComparer.OrdinalIgnoreCase) { 
                "quickly", "silently", "with care", "without warning", 
                "while the moon rises", "before the sun sets" 
            };

        /// <summary>
        /// <Location> ::= "in the dark forest" | "at the castle" 
        ///               | "on the mountain" | "near the river" 
        ///               | "inside the cave"
        /// Always multi-word phrases.
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Locations =
            new(StringComparer.OrdinalIgnoreCase) { 
                "in the dark forest", "at the castle", "on the mountain", 
                "near the river", "inside the cave" 
            };

        /// <summary>
        /// <Condition> ::= "because he is brave" | "because she is clever" 
        ///                | "if the door is unlocked" | "if the dragon sleeps"
        ///                | "when the knight calls"
        /// Always multi-word phrases.
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Conditions =
            new(StringComparer.OrdinalIgnoreCase) { 
                "because he is brave", "because she is clever", 
                "if the door is unlocked", "if the dragon sleeps",
                "when the knight calls" 
            };

        /// <summary>
        /// <Conjunction> ::= "and" | "but" | "then" | "while"
        /// Case-insensitive.
        /// </summary>
        public static readonly HashSet<string> Conjunctions =
            new(StringComparer.OrdinalIgnoreCase) { "and", "but", "then", "while" };

        /// <summary>
        /// <Punctuation> ::= "." | "," | "!" | "?"
        /// </summary>
        public static readonly HashSet<string> Punctuation =
            new(StringComparer.OrdinalIgnoreCase) { ".", ",", "!", "?" };
    }
}

