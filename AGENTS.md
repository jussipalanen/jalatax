# JalaTax – Agent Development Guidelines

## Project Overview

JalaTax is a small VB.NET demo application that demonstrates configurable business rules, validation, rule processing, testing, error handling, and auditability.

The project is inspired by enterprise rule-processing systems, but it must remain a fictional demonstration application. It must not attempt to reproduce, imitate, or claim compatibility with any real tax administration system.

The main goals of the project are to demonstrate:

* Modern VB.NET development with .NET
* Clean separation of responsibilities
* Configurable business rules
* Input validation
* Rule processing
* Error handling
* Audit logging
* Unit testing
* Maintainable and readable code
* Clear technical documentation

Keep the project intentionally small and easy to understand.

---

## Technology Stack

Use:

* VB.NET
* Modern .NET
* .NET Console Application
* .NET Class Library
* MSTest
* System.Text.Json
* JSON configuration files
* Git and GitHub

Do not introduce additional frameworks or dependencies unless they provide clear value.

Prefer built-in .NET functionality whenever possible.

---

## Solution Structure

Use the following general structure:

```text
JalaTax/
├── JalaTax.sln
├── README.md
├── AGENTS.md
├── CLAUDE.md
│
├── src/
│   ├── JalaTax.Core/
│   │   ├── Models/
│   │   ├── Rules/
│   │   ├── Services/
│   │   ├── Configuration/
│   │   ├── Validation/
│   │   └── Audit/
│   │
│   └── JalaTax.Console/
│       └── Program.vb
│
├── tests/
│   └── JalaTax.Tests/
│
└── data/
    ├── rules.json
    └── example-taxpayer.json
```

Do not create unnecessary projects or directories.

---

## Architecture

Follow a simple layered architecture.

### JalaTax.Core

Contains the actual domain and business logic.

It must not depend on the console application.

Responsibilities include:

* Domain models
* Validation
* Rule processing
* Tax calculation examples
* Configuration loading
* Audit events
* Business services

### JalaTax.Console

Acts only as the application entry point.

Responsibilities include:

* Loading configuration
* Loading example input
* Calling JalaTax.Core services
* Showing results
* Showing errors in a user-friendly way

Do not place business rules directly inside `Program.vb`.

### JalaTax.Tests

Contains automated tests for JalaTax.Core.

Prioritize testing:

* Rule behavior
* Boundary values
* Invalid input
* Configuration handling
* Calculation logic
* Exceptional conditions

---

## Domain Model

Use fictional demonstration data only.

Example model:

```vb
Public Class TaxCase
    Public Property TaxpayerId As String
    Public Property AnnualIncome As Decimal
    Public Property Deductions As Decimal
End Class
```

Possible supporting models include:

* `TaxCase`
* `TaxResult`
* `TaxBracket`
* `TaxRuleConfiguration`
* `ValidationResult`
* `AuditEntry`

Keep domain models simple.

Avoid database-style entity complexity unless a future task specifically requires persistence.

---

## Business Rules

Business rules are one of the main purposes of the application.

Prefer separating rules from orchestration logic.

For example:

```text
Rules/
├── ITaxRule.vb
├── IncomeValidationRule.vb
├── DeductionRule.vb
└── TaxBracketRule.vb
```

Rules should preferably:

* Have one clear responsibility
* Be independently testable
* Return understandable results
* Avoid hidden side effects
* Avoid direct console output

Where practical, rule behavior should be driven by configuration instead of hard-coded values.

---

## Configuration

Use JSON for configurable values.

Example:

```json
{
  "basicDeduction": 3000,
  "taxBrackets": [
    {
      "min": 0,
      "max": 20000,
      "rate": 0.10
    },
    {
      "min": 20000,
      "max": 50000,
      "rate": 0.20
    },
    {
      "min": 50000,
      "max": null,
      "rate": 0.30
    }
  ]
}
```

These numbers are fictional and exist only for demonstration purposes.

Never present them as current Finnish tax rules.

Configuration loading must include error handling for:

* Missing files
* Invalid JSON
* Missing required values
* Invalid ranges
* Invalid tax rates

---

## Validation

Validate input before executing calculations.

Examples:

* Annual income must not be negative
* Deductions must not be negative
* Required identifiers must not be empty
* Configuration ranges must be logically valid
* Tax rates must remain within acceptable numeric limits

Prefer explicit validation classes or services over scattered checks.

Validation failures should provide clear error messages.

---

## Error Handling

Expected invalid user input should not crash the application.

Handle predictable failures explicitly.

Examples:

* Configuration file cannot be found
* JSON cannot be parsed
* Input data is invalid
* Required configuration is missing
* No matching rule exists

Do not use empty catch blocks.

Do not silently ignore exceptions.

Unexpected exceptions may be caught at the console application boundary and displayed with an understandable message.

---

## Audit Trail

The application should demonstrate basic auditability.

Record meaningful rule-processing events, for example:

```text
Tax case DEMO-001 loaded
Income validation passed
Deduction rule applied
Tax bracket selected
Calculation completed
```

Audit entries should preferably contain:

* Timestamp
* Event type
* Description
* Rule name when applicable

Do not log sensitive or real personal information.

Use fictional identifiers such as:

```text
DEMO-001
DEMO-002
```

---

## Coding Style

Write idiomatic and readable VB.NET.

Prefer:

```vb
Public Function CalculateTaxableIncome(taxCase As TaxCase) As Decimal
    Dim taxableIncome = taxCase.AnnualIncome - taxCase.Deductions
    Return Math.Max(0D, taxableIncome)
End Function
```

Avoid unnecessary complexity.

### General rules

* Enable strict typing.
* Prefer `Option Strict On`.
* Prefer `Option Explicit On`.
* Use meaningful names.
* Use PascalCase for public types and members.
* Use camelCase for parameters and local variables.
* Keep methods small and focused.
* Avoid deeply nested conditions.
* Prefer early validation where appropriate.
* Do not use global mutable state.
* Avoid unnecessary inheritance.
* Prefer composition and simple services.
* Do not duplicate logic.
* Add comments only when they explain why something exists, not what obvious code does.

---

## Async Programming

Do not make methods asynchronous unless actual asynchronous I/O is involved.

Suitable examples include:

* Reading files asynchronously
* Future HTTP integrations

Do not add `Async` merely for architectural appearance.

---

## Dependencies

Keep dependencies minimal.

Before adding a NuGet package:

1. Check whether .NET already provides the functionality.
2. Confirm that the package solves a real project requirement.
3. Prefer mature and actively maintained packages.
4. Document why the dependency was introduced.

Do not add large frameworks to solve small problems.

---

## Testing

Use MSTest.

Tests should follow an Arrange / Act / Assert structure.

Example naming:

```text
CalculateTaxableIncome_WhenDeductionsExist_ReturnsReducedIncome
CalculateTaxableIncome_WhenIncomeIsNegative_ThrowsArgumentException
Validate_WhenTaxpayerIdIsEmpty_ReturnsValidationError
```

Test:

* Happy paths
* Invalid input
* Zero values
* Boundary values
* Configuration errors
* Rule selection behavior

Every bug fix should include a regression test when practical.

Run before completing changes:

```powershell
dotnet build
dotnet test
```

Both commands must succeed.

---

## Console Output

Keep console output professional and readable.

Example:

```text
JalaTax
----------------------------------------

Processing case: DEMO-001

Validation
✓ Income validated
✓ Deductions validated

Rules
✓ Deduction rule applied
✓ Tax bracket selected

Result
----------------------------------------
Annual income:    45,000.00
Deductions:        2,500.00
Taxable income:   42,500.00
Calculated tax:    6,500.00

Demo calculation completed.
```

Exact formatting may evolve.

Do not build a complex console UI.

---

## Security and Privacy

This is a demonstration application.

Never add:

* Real tax identifiers
* Finnish personal identity codes
* Real customer data
* Passwords
* API keys
* Secrets
* Production credentials

Use fictional test data only.

Secrets must never be committed to Git.

---

## Git Workflow

For non-trivial features:

1. Create a dedicated branch.
2. Make focused changes.
3. Add or update tests.
4. Run build and tests.
5. Update documentation when necessary.
6. Create a pull request.

Suggested branch naming:

```text
feature/configurable-tax-rules
feature/audit-trail
feature/input-validation
fix/configuration-validation
test/tax-rule-boundaries
docs/update-readme
```

Commits should be focused.

Suggested commit style:

```text
Add configurable tax bracket rules
Add validation for negative income
Add audit trail for rule processing
Add unit tests for deduction rules
Update project architecture documentation
```

Avoid generic commit messages such as:

```text
changes
fix stuff
updates
```

---

## Pull Requests

A pull request should explain:

* What changed
* Why it changed
* How it was tested
* Any architectural impact

Keep pull requests small enough to review easily.

Do not combine unrelated refactoring and new features unless necessary.

---

## Documentation

Keep `README.md` useful for a technical reviewer.

It should eventually explain:

* What JalaTax is
* Why it was created
* Technology stack
* Architecture
* How business rules work
* How to run the application
* How to run tests
* Example output
* Important disclaimer that all tax rules are fictional

Update the README when externally visible behavior changes.

---

## Scope Control

JalaTax is intentionally a small demo.

Do not automatically add:

* React
* Angular
* Vue
* Blazor
* Databases
* Authentication
* Docker
* Kubernetes
* Cloud infrastructure
* Microservices
* Message queues

These technologies may only be introduced if a future requirement clearly justifies them.

Prefer demonstrating good software engineering with a small codebase over maximizing the number of technologies.

---

## Definition of Done

A task is complete when:

* The requested functionality works.
* Code follows the project architecture.
* Business logic remains outside the console entry point.
* Appropriate tests exist.
* `dotnet build` succeeds.
* `dotnet test` succeeds.
* No secrets or real personal data are introduced.
* Documentation is updated when required.
* The solution remains understandable and appropriately small.

---

## Primary Principle

Build JalaTax as if another developer will need to understand, debug, test, and modify the business rules later.

Clarity, testability, traceability, and maintainability are more important than clever code.
