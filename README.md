# Tiny-Compiler
A simple compiler for the tiny language (C-like), implemented using C# and Windows Forms.

- [Tiny Language Description](https://docs.google.com/document/d/1WbSTlhhWeW67SFEdHkaQGiKx4_WBjdlX/edit?rtpof=true&tab=t.0)
- [Tiny Language Context Free Grammar Document](docs/CFG.md)
- [Deterministic Finite Automaton (DFA) for Tiny Language](docs/DFA.md)

#### Description
The implementation of the compiler is divided into two main phases:

- **Scanner Phase (Lexical Analysis)**: Recognizes basic language units (tokens) such as identifiers, literals, operators, and keywords, extracting their attributes and storing them in a symbol table.

- **Parser Phase (Syntax Analysis)**: Builds a syntax tree from tokens and validates their arrangement against the source code grammar to ensure syntactic correctness


### Testing

#### Setup

**Add NUnit NuGet Packages**

- Open the NuGet Package Manager by right-clicking on your project in Solution Explorer and selecting Manage NuGet Packages.
- Search for NUnit and click Install.
- Then search for NUnit3TestAdapter and install it. 
	- This adapter allows NUnit tests to be discovered and run using Visual Studio's test runner.

Alternatively, you can use the *Package Manager Console*:

```bash
Install-Package NUnit
Install-Package NUnit3TestAdapter
```
---

if you encounter reference errors, make sure a reference to the main project is present in the test project.
- Right-click on the test project (*TinyCompiler.Tests*) in Solution Explorer.
- Select Add > Project Reference.
- Check on the main project (*TINY_Compiler*) and click OK.

#### Running Tests
- In Visual Studio, go to Test > Test Explorer from the menu
- **Run All** to execute all tests, or choose specific tests to execute

