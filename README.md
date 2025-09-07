# Tokenizer

A simple **lexer + parser** project written in **C#** that demonstrates
**tokenization** and **recursive descent parsing** for a toy story-like
grammar.

The program takes input sentences (like *"The mighty hero fights the
dragon in the dark forest."*), breaks them down into **tokens** (words
classified into categories like `Noun`, `Verb`, `Location`), and then
parses them according to a **Context-Free Grammar (CFG)**, showing the
**step-by-step derivation**.

------------------------------------------------------------------------

## Features

### Tokenizer

-   Splits raw input into tokens.
-   Supports **multi-word tokens** (e.g., `"in the dark forest"`,
    `"searches for"`).
-   Handles punctuation (`.,!?`) as separate tokens.
-   Classifies tokens using categories defined in `CFG.cs` (e.g.,
    `Determiner`, `Noun`, `Verb`, `Adjective`, `Location`).

### Parser

-   Implements a **recursive descent parser**.
-   Prints **leftmost derivations** as it parses.
-   Supports grammar rules like:

```{=html}
<!-- -->
```
    <Story> ::= <Sentence>
    <Sentence> ::= <SimpleSentence>
    <SimpleSentence> ::= <Subject> <VerbPhrase> <Extra>
    <NounPhrase> ::= <Determiner> <AdjectiveList> <Noun> 
                   | <AdjectiveList> <Noun>
                   | <Noun>
    <AdjectiveList> ::= <Adjective> <AdjectiveList> | ε
    <VerbPhrase> ::= <Verb> <Object> <Extra> | <Verb> <Extra>
    <Object> ::= <NounPhrase>
    <Extra> ::= <Location> | <Condition> | <Adverbial> | <Punctuation> | ε

------------------------------------------------------------------------

## Project Structure

    Tokenizer/
    │── Program.cs        # Entry point: runs tokenizer and parser on test cases
    │── Tokenizer.cs      # Tokenizer logic (lexical analysis)
    │── Token.cs          # Token class (value + type)
    │── TokenType.cs      # Enumeration of all token categories
    │── CFG.cs            # Context-Free Grammar terminal definitions
    │── Parser.cs         # Recursive descent parser with derivation tracing

------------------------------------------------------------------------

## Example Usage

Input sentence:

    The mighty hero fights the dragon in the dark forest.

**Phase 1 (Tokenization):**

    "The" <Determiner>
    "mighty" <Adjective>
    "hero" <Noun>
    "fights" <Verb>
    "the" <Determiner>
    "dragon" <Noun>
    "in the dark forest" <Location>
    "." <Punctuation>

**Phase 2 (Parsing Derivation):**

    <Story>
    => <Sentence>
    => <SimpleSentence>
    => <Subject> <VerbPhrase> <Extra>
    => <NounPhrase> <VerbPhrase> <Extra>
    => <Determiner> <AdjectiveList> <Noun> <VerbPhrase> <Extra>
    => "The" <AdjectiveList> <Noun> <VerbPhrase> <Extra>
    => "The" <Adjective> <AdjectiveList> <Noun> <VerbPhrase> <Extra>
    => "The" "mighty" <AdjectiveList> <Noun> <VerbPhrase> <Extra>
    => "The" "mighty" <Noun> <VerbPhrase> <Extra>
    => "The" "mighty" "hero" <VerbPhrase> <Extra>
    => "The" "mighty" "hero" <Verb> <Object> <Extra> <Extra>
    => ...

------------------------------------------------------------------------

## Running the Project

### Requirements

-   .NET 6.0 SDK or later

### Build & Run

``` bash
dotnet build
dotnet run
```

------------------------------------------------------------------------

## Author

Project by:
-    **Alle Zayd Busaman**
-    **Jose Anuada Jr.**
-    **Tichard Toledo Jr.**
-    **Rico Euma Aban**\
Demonstration of **lexer + parser construction** in C# for educational
purposes.

