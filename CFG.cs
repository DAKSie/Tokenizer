/*
 * Project: Tokenizer CFG
 * Author: Rico Euma O. Aban
 *
 * Description:
 * The CFG (Context-Free Grammar) class defines the vocabulary and categories
 * of words used by the Tokenizer and Parser. Each `HashSet<string>` represents
 * a terminal category such as determiners, nouns, verbs, adjectives, etc.
 * 
 * This acts as the lexical dictionary for the parsing process.
 */

namespace Tokenizer {
    public static class CFG {
        // ---------------------
        // Lexical Categories
        // ---------------------

        /// <summary>
        /// Words functioning as determiners (e.g., "the", "a", "my").
        /// </summary>
        public static readonly HashSet<string> Determiners =
            new() { "The", "A", "An", "My", "Her", "His", "a", "the", "an", "my", "her", "his" };

        /// <summary>
        /// Words functioning as adjectives (e.g., "brave", "enchanted").
        /// </summary>
        public static readonly HashSet<string> Adjectives =
            new() { "brave", "old", "young", "rusty", "enchanted", "mighty", "dark" };

        /// <summary>
        /// Words functioning as nouns (e.g., "hero", "dragon", "castle").
        /// </summary>
        public static readonly HashSet<string> Nouns =
            new() { "hero", "wizard", "knight", "dragon", "princess", 
                    "treasure", "cave", "sword", "castle", "villager", 
                    "horse", "forest", "dragon" };

        /// <summary>
        /// Words functioning as verbs (e.g., "fights", "searches").
        /// </summary>
        public static readonly HashSet<string> Verbs =
            new() { "fights", "searches", "rescues", "discovers", "rides", 
                    "finds", "opens", "calls", "holds", "protects" };

        /// <summary>
        /// Words functioning as prepositions (e.g., "in", "on").
        /// </summary>
        public static readonly HashSet<string> Prepositions =
            new() { "in", "on", "at", "under", "inside", "near", "above", "beside" };

        /// <summary>
        /// Words introducing relative clauses (e.g., "who").
        /// </summary>
        public static readonly HashSet<string> RelativeClauses = 
            new() { "who" };

        /// <summary>
        /// Words functioning as adverbials (e.g., "quickly", "silently").
        /// </summary>
        public static readonly HashSet<string> Adverbials =
            new() { "quickly", "silently", "carefully" };

        /// <summary>
        /// Words representing locations (e.g., "castle", "forest").
        /// </summary>
        public static readonly HashSet<string> Locations =
            new() { "castle", "mountain", "river", "cave", "forest" };

        /// <summary>
        /// Words expressing conditions or states (e.g., "brave", "sleeps").
        /// </summary>
        public static readonly HashSet<string> Conditions =
            new() { "brave", "clever", "unlocked", "sleeps" };

        /// <summary>
        /// Words functioning as conjunctions (e.g., "and", "but").
        /// </summary>
        public static readonly HashSet<string> Conjunctions =
            new() { "and", "but", "then", "while" };

        /// <summary>
        /// Punctuation symbols recognized as tokens.
        /// </summary>
        public static readonly HashSet<string> Punctuation =
            new() { ".", ",", "!", "?" };
    }
}

