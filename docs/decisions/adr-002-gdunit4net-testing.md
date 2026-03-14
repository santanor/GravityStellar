# ADR-002: GdUnit4Net Testing Framework

**Date:** 2026-03  
**Status:** Accepted  
**Author:** Dallas (Documentation Engineer)

## Context

The GravityStellar project requires automated testing to ensure the physics simulation, gameplay mechanics, and C# runtime remain stable as features are added. We needed to select a testing framework that integrates well with Godot 4 and C#.

### Options Considered

1. **GdUnit4** (C# port / GdUnit4Net)
   - Native Godot integration
   - Scene testing support
   - CI action ready (MikeSchulze/gdUnit4-action)
   - Fluent assertion API designed for game testing
   - Maintained actively

2. **xUnit**
   - General-purpose .NET testing
   - Strong C# support
   - No native Godot integration
   - Would require custom Godot scene setup/teardown

3. **NUnit**
   - Mature .NET framework
   - Works with Godot but not optimized for it
   - Limited scene-level testing support

4. **MSTest**
   - Microsoft's default testing framework
   - No specific game development advantages
   - Minimal Godot integration

## Decision

**Use GdUnit4Net** — the C# port of GdUnit4, maintained by MikeSchulze. All unit tests will use GdUnit4Net for assertions, scene testing, and automation.

## Rationale

- **Native Godot Integration:** GdUnit4Net is purpose-built for Godot 4, with seamless scene testing and node inspection
- **Scene Testing:** Tests can manipulate scenes, nodes, and physics directly — critical for gameplay mechanics testing
- **CI Action Ready:** The MikeSchulze/gdUnit4-action GitHub Action runs tests automatically; no custom runner needed
- **Fluent API:** Assertion methods read naturally for game development (e.g., `AssertThat(velocity).IsGreaterThan(0)`)
- **Active Maintenance:** Actively maintained with Godot 4 compatibility guarantees

## Consequences

- **Team Learning:** Team must learn GdUnit4Net API and idioms
- **Test Location:** All tests live in the `test/` directory at the project root, not alongside source files
- **CI Integration:** CI pipeline will use `MikeSchulze/gdUnit4-action` to run tests on every PR
- **Local Testing:** Developers can run tests locally via the GdUnit4 test runner in the Godot editor or via `dotnet test` with GdUnit4Net command-line support

## See Also

- [ADR-003: CI/CD Pipeline](adr-003-cicd-pipeline.md) — describes how GdUnit4Net tests run in CI
- [../ci-cd.md](../ci-cd.md) — detailed CI/CD pipeline documentation
