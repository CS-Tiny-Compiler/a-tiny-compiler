# Tiny-Compiler
A simple compiler for a tiny language, written using C#.

- [Tiny Language Description](https://docs.google.com/document/d/1WbSTlhhWeW67SFEdHkaQGiKx4_WBjdlX/edit?rtpof=true&tab=t.0)


## Local Setup

### Unit Tests

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
