// I used Ai to store every lexemes into HashSets Determiners to make life easier.
// using HashSets instead of string arrays because they have O(1) lookup times instead
// of arrays which have O(n) because you have to iterate througt every index.
namespace Tokenizer {
    public static class CFG {
        // Determiners
        public static readonly HashSet<string> Determiners =
            new() { "The", "A", "An", "My", "Her", "His", "a" , "the", "an", "my", "her", "his",};
        // Adjectives
        public static readonly HashSet<string> Adjectives =
            new() { "brave", "old", "young", "rusty", "enchanted", "mighty" };
        // Nouns
        public static readonly HashSet<string> Nouns =
            new() { "hero", "wizard", "knight", "dragon",
                    "princess", "treasure", "cave", "sword",
                    "castle", "villager", "horse" };
        // Verbs
        public static readonly HashSet<string> Verbs =
            new() { "fights", "searches for", "rescues",
                    "discovers", "rides", "finds",
                    "opens", "calls" };
        // Prepositions
        public static readonly HashSet<string> Prepositions =
            new() { "in", "on", "at", "under", "inside",
                    "near", "above", "beside" };
        // Adverbials
        public static readonly HashSet<string> Adverbials =
            new() { "while the moon rises", "quickly", "silently", "with care", "without warning" };
        // Locations
        public static readonly HashSet<string> Locations =
            new() { "before the sun sets",
                    "at the castle", "on the mountain",
                    "near the river", "inside the cave", "in the dark forest" };
        // Conditions
        public static readonly HashSet<string> Conditions =
            new() { "because he is brave", "because she is clever",
                    "if the door is unlocked", "if the dragon sleeps",
                    "when the knight calls", "while" };
        // Conjunctions
        public static readonly HashSet<string> Conjunctions =
            new() { "and", "but", "then", "while" };
        // Punctuation
        public static readonly HashSet<string> Punctuation =
            new() { ".", ",", "!", "?" };
    }
}
