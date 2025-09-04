"# Tokenizer"

# Tokenizer

A simple **C# tokenizer** that parses sentences into tokens based on a **Context-Free Grammar (CFG)**.  
It supports both single-word and multi-word tokens such as verbs, determiners, adjectives, nouns, adverbials, prepositions, conjunctions, punctuation, locations, and conditions.

---

## 📖 Features
- Splits input into words and punctuation.
- Detects multi-word tokens (e.g., `in front of`, `as soon as`).
- Classifies words into categories using `CFG` (stored in `HashSet<string>` collections).
- Returns a list of tokens with their types.

---

## 💻 Example
Input:
A wizard discovers a cave while the moon rises.

Output:

Tokens:
 A <Determiner>
 wizard <Noun>
 discovers <Verb>
 a <Determiner>
 cave <Noun>
 while the moon rises <Adverbial>
 . <Punctuation>





 
---

## ⚡ Usage
### Compile
```bash
dotnet build

csc Program.cs Tokenizer.cs CFG.cs Token.cs TokenType.csc

dotnet run
