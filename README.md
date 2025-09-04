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
```
A wizard discovers a cave while the moon rises.
```

Output:
```
Determiner(A), Noun(wizard), Verb(discovers), Determiner(a),
Noun(cave), Conjunction(while), Determiner(the), Noun(moon),
Verb(rises), Punctuation(.)
```

---

## ⚡ Usage
### Compile
```bash
dotnet build
```
or with the C# compiler:
```bash
csc Program.cs Tokenizer.cs CFG.cs Token.cs TokenType.cs
```

### Run
```bash
dotnet run
```

---

## 📂 Project Structure
```
├── CFG.cs          # Stores grammar definitions
├── Token.cs        # Token class (word + TokenType)
├── TokenType.cs    # Enum for token categories
├── Tokenizer.cs    # Core tokenizer logic
├── Program.cs      # Example usage
└── README.md       # Documentation
```

---

## 🧩 Future Improvements
- Expand CFG vocabulary
- Add more relative clauses
- Normalize capitalization
- Unit tests for token matching

