
# Playwright with .NET

This project brifly sumerize basic setup for Playwright automation with .NET.

## Running tests

### Execute single test:

```bash
dotnet test --filter "CheckErrorMessagesValidation"
```

### Execute all tests:

```bash
dotnet test
```

### Execute and debug test using Playwright UI:

1. Set Headless = false to launch the Playwright UI in your tests.
2. Add SlowMo to slow down actions for better visibility.
3. Use the Playwright Inspector to step through and debug tests interactively.
4. Enable PWDEBUG=1 to open Playwright Inspector automatically.

```bash
set PWDEBUG=1 
dotnet test
```
## Parallel Execution

Paralel Execution : 

[Parallelizable(ParallelScope.Fixtures)]: 

This tells NUnit to run all tests in the class in parallel at the fixture level. 

Examples:

- ParallelScope.Self runs the test in parallel with other tests in the same class. 
- ParallelScope.All runs all tests in parallel regardless of fixture.
- ParallelScope.Children allows child tests to run in parallel.
