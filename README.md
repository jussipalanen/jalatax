# JalaTax

JalaTax is a small VB.NET demo application that shows configurable business rules, input validation, rule processing, error handling, audit logging and unit testing.

> **Disclaimer:** JalaTax is a fictional demonstration. All tax rules, rates and data in this repository are made up for demo purposes. They are not current Finnish tax rules and do not reproduce or claim compatibility with any real tax administration system.

## Status

Project scaffold only. The solution structure and code standards are in place; tax rules, configuration loading, validation and the audit trail are not implemented yet.

## Technology stack

- VB.NET on .NET 10
- .NET console application and class library
- MSTest (Microsoft.Testing.Platform runner)
- System.Text.Json and JSON configuration files

## Project structure

```text
JalaTax/
├── JalaTax.sln
├── Directory.Build.props      Shared build settings and code validation
├── .editorconfig              Formatting, code style and naming rules
├── global.json                .NET SDK version and test runner
├── src/
│   ├── JalaTax.Core/          Domain models and business logic
│   └── JalaTax.Console/       Console entry point (Program.vb)
├── tests/
│   └── JalaTax.Tests/         MSTest tests for JalaTax.Core
└── data/                      JSON rules and example input
```

`JalaTax.Core` must not depend on the console application. See [AGENTS.md](AGENTS.md) for the full architecture and coding guidelines.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (10.0.100 or a later 10.0 feature band)
- Optional: Visual Studio 2026, Visual Studio Code with the C# Dev Kit, or JetBrains Rider

Check the installed SDK:

```powershell
dotnet --version
```

## Build and run

From the repository root:

```powershell
dotnet restore
dotnet build
dotnet run --project src/JalaTax.Console
```

Expected output while the project is a scaffold:

```text
JalaTax
----------------------------------------
Demo scaffold. Tax rules are not implemented yet.
```

In Visual Studio, open `JalaTax.sln`, set **JalaTax.Console** as the startup project and press **F5**.

## Run tests

```powershell
dotnet test
```

## Code validation and standards

The rules are enforced automatically, so a local build fails the same way CI does.

| What | Where | Enforced by |
|---|---|---|
| `Option Strict On`, `Option Explicit On`, `Option Infer On` | `Directory.Build.props` | Compiler |
| Warnings treated as errors | `Directory.Build.props` | Compiler |
| .NET analyzers (`latest-recommended`) | `Directory.Build.props` | Build |
| Formatting, code style, naming conventions | `.editorconfig` | Build and `dotnet format` |
| Format, build and test on every push and pull request | `.github/workflows/ci.yml` | GitHub Actions |

Naming conventions: PascalCase for types and public members, camelCase for parameters and locals, and an `I` prefix for interfaces. Test methods use the `Method_WhenCondition_ExpectedResult` pattern.

Before opening a pull request, run:

```powershell
dotnet format --verify-no-changes
dotnet build
dotnet test
```

To fix formatting and code style issues automatically:

```powershell
dotnet format
```

## Contributing

Work happens on feature branches (for example `feature/configurable-tax-rules`) and is merged to `main` through a pull request that a human reviews. The pull request template lists what to describe and check.
