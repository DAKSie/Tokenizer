namespace Tokenizer {
    public enum TokenType {
        // Sentence structure
        Sentence,
        CompoundSentence,
        SimpleSentence,
        Story,

        // Phrase types
        NounPhrase,
        VerbPhrase,
        RelativeClause,
        PrepositionalPhrase,
        Object,
        Extra,

        // Word categories
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

        // Terminals (keywords/literals)
        DeterminerWord,   
        AdjectiveWord,            
        NounWord,              
        VerbWord,
        PrepositionWord, 
        AdverbWord,    
        LocationWord,    
        ConditionWord,    

        // Misc
        Unknown    }
}
