# Changelog

All notable changes to JalaTax are documented in this file.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and the project uses [semantic versioning](https://semver.org/). Each released version has a Git tag `vX.Y.Z` and a GitHub Release, created automatically from this file (see *Versioning and releases* in the README).

## [Unreleased]

## [1.0.0] - 2026-09-24

First release.

### Added

- **Solution structure**: `JalaTax.Core` class library, Windows Forms desktop app, console runner and MSTest tests on .NET 10 ([#2](https://github.com/jussipalanen/jalatax/pull/2)).
- **Code standards**: `Option Strict On`, .NET analyzers, warnings as errors, `.editorconfig` naming and style rules, and a CI check for format, build and tests ([#2](https://github.com/jussipalanen/jalatax/pull/2)).
- **VS Code support**: launch and task configuration ([#3](https://github.com/jussipalanen/jalatax/pull/3)).
- **Domain models**: tax cases, results, brackets, configuration, validation results and audit entries ([#5](https://github.com/jussipalanen/jalatax/pull/5)).
- **Configuration**: JSON rule sets with strict loading and checks (required values, contiguous brackets starting at 0, rates between 0 and 1) ([#7](https://github.com/jussipalanen/jalatax/pull/7)).
- **Input validation**: tax case checks that report every problem at once, and example cases `DEMO-001`–`DEMO-003` ([#13](https://github.com/jussipalanen/jalatax/pull/13)).
- **Tax calculation**: deduction and progressive bracket rules, with rounding to 2 decimals and halves rounded away from zero ([#14](https://github.com/jussipalanen/jalatax/pull/14)).
- **Audit trail**: every processing step is recorded with a timestamp, event type, description and rule name ([#15](https://github.com/jussipalanen/jalatax/pull/15)).
- **Desktop app**: Windows Forms UI with case input, validation messages next to the fields, a result panel with tax per bracket, the audit trail and an About dialog showing the version ([#12](https://github.com/jussipalanen/jalatax/pull/12), [#16](https://github.com/jussipalanen/jalatax/pull/16)).
- **Console runner**: processes a file of cases and prints validation, rules and results, with documented exit codes ([#16](https://github.com/jussipalanen/jalatax/pull/16)).
- **Finnish and English**: Finnish is the default. Texts, messages, audit trail and number formats follow the language, and both apps have a language switcher or `--lang` option ([#18](https://github.com/jussipalanen/jalatax/pull/18)).
- **Logo and application icon** ([#18](https://github.com/jussipalanen/jalatax/pull/18)).
- **Rule sets**: a selectable rule set with the published 2026 Finnish state income tax scale (simplified, not an official tax calculation). It can be chosen with a switcher in the desktop app or `--rules` in both apps, and the audit trail records the rule set used ([#20](https://github.com/jussipalanen/jalatax/pull/20)).
- **Documentation**: README, Finnish summary and user guide (`README.fi.md`) and screenshots ([#20](https://github.com/jussipalanen/jalatax/pull/20)).

[Unreleased]: https://github.com/jussipalanen/jalatax/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/jussipalanen/jalatax/releases/tag/v1.0.0
