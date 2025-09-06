/*
 * Project: Tokenizer Token Types
 * Author: Rico Euma O. Aban
 *
 * Description:
 * The TokenType enumeration defines all possible categories of tokens.
 * It distinguishes between higher-level grammar structures (non-terminals),
 * word categories (parts of speech), terminal words, and special cases.
 */

namespace Tokenizer {
    public enum TokenType {
        // ---------------------
        // High-level structures
        // ---------------------
        Sentence,
        CompoundSentence,
        SimpleSentence,
        Story,

        // ---------------------
        // Phrase types
        // ---------------------
        NounPhrase,
        VerbPhrase,
        RelativeClause,
        PrepositionalPhrase,
        Object,
        Extra,
        Subject,
        AdjectiveList,

        // ---------------------
        // Word categories
        // ---------------------
        Determiner,
        Adjective,
        Noun,
        Verb,
        Preposition,
        Adverbial,
        Location,
        Condition,
        Conjunction,
        Punctuation,

        // ---------------------
        // Terminal symbols
        // ---------------------
        DeterminerWord,   
        AdjectiveWord,            
        NounWord,              
        VerbWord,
        PrepositionWord, 
        AdverbWord,    
        LocationWord,    
        ConditionWord,    
        RelativeClauseWord,
        ConjunctionWord,
        PunctuationWord,

        // ---------------------
        // Miscellaneous
        // ---------------------
        Unknown    
    }
}

