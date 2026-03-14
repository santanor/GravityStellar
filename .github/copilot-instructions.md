# Gravity Stellar — Copilot Coding Instructions

## Project Overview

Gravity Stellar is a mobile puzzle physics game built with **Godot 4.6** and **C# (.NET 8.0)**. Players spawn planetary bodies that interact via gravity, forming orbital systems that merge into larger bodies. Visual style is minimalist and meditative.

- **Engine:** Godot 4.6 (Mono / C#)
- **SDK:** Godot.NET.Sdk 4.6.1
- **Target:** .NET 8.0 (net9.0 for Android)
- **Physics:** Jolt Physics
- **Rendering:** Mobile renderer, D3D12 on Windows

## Architecture Principles

- Small, focused scripts — no god classes
- Composition over inheritance
- Signals for decoupling between systems
- Clear system boundaries: physics, rendering, gameplay, UI
- Physics logic stays deterministic where possible

## Testing Framework: GdUnit4Net

We use [GdUnit4Net](https://github.com/MikeSchulze/gdUnit4Net) for C# unit testing inside Godot.

### Test Directory Structure

```
test/
├── unit/
│   └── ExampleTest.cs
├── integration/
├── scenes/
└── README.md
```

### Writing Tests

- **Namespace:** `GravityStellar.Tests.<SystemName>` (e.g., `GravityStellar.Tests.Physics`)
- **Test class naming:** `<ClassUnderTest>Test` (e.g., `GravityCalculationTest`)
- **File location:** Mirror source structure under `test/`
- **Attributes:** Use `[TestSuite]` on classes, `[TestCase]` on methods

```csharp
using GdUnit4;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class GravityCalculationTest
{
    [TestCase]
    public void ShouldCalculateGravitationalForce()
    {
        // Arrange
        var mass1 = 10.0f;
        var mass2 = 20.0f;
        var distance = 5.0f;

        // Act
        var force = GravityCalculation.Calculate(mass1, mass2, distance);

        // Assert
        AssertThat(force).IsGreater(0);
    }

    [TestCase]
    public void ShouldReturnZeroForceAtZeroMass()
    {
        var force = GravityCalculation.Calculate(0, 10.0f, 5.0f);
        AssertThat(force).IsEqual(0);
    }
}
```

### Key GdUnit4Net Patterns

- Use `AssertThat()` for fluent assertions
- Use `[Before]` / `[After]` for setup/teardown
- Use `[BeforeTest]` / `[AfterTest]` for per-test setup
- Scene tests: use `ISceneRunner` to load and interact with scenes
- Keep tests fast — avoid scene loading when unit tests suffice

### Running Tests Locally

```bash
# From the project root, using the GdUnit4 console runner:
dotnet test

# Or run via Godot editor with the GdUnit4 plugin installed
```

## CI/CD Pipeline

Every PR to `master` triggers the CI pipeline (`.github/workflows/ci.yml`):

1. **Test job:** Runs all GdUnit4Net C# tests via `MikeSchulze/gdUnit4-action@v1`
2. **Build job:** Exports the Windows build via `firebelley/godot-export@v6`
3. **Artifact upload:** Windows build is uploaded as a PR artifact for reviewers to download and play

Godot and export templates are cached to speed up builds.

## Code Style

- Use PascalCase for public members, _camelCase for private fields
- One class per file
- Keep methods short and focused
- Prefer explicit types for clarity in physics code
- Use `readonly` and `const` where appropriate

## PR Review Checklist and Guidelines

### Core Rule

**PRs must be small and atomic.**

If a PR contains multiple concerns:

* Request it be **split into smaller PRs**
* Suggest **clear boundaries for the split**

Acceptable PR scope:

* One gameplay component
* One class refactor
* One bug fix
* One debug feature
* One small system

Reject or split PRs that:

* Add large gameplay systems
* Mix refactors with features
* Rewrite large scenes
* Modify many unrelated files

---

### Godot Architecture

Prefer:

* Small scripts
* Modular scenes
* Signals for communication
* Composition over inheritance
* Clear node responsibilities

Avoid:

* Large monolithic scripts
* Deep node hierarchies
* Logic tightly coupled to visuals
* Heavy work in `_Process()`

Gameplay logic, physics, and visuals should remain **separate systems**.

---

### C# Code Quality

Ensure code is **clean and idiomatic**.

Prefer:

* Clear naming
* Small focused methods
* Single responsibility classes
* Readable control flow

Avoid:

* Clever abstractions
* Large classes
* Hidden side effects
* Unnecessary complexity

Favor **clarity over cleverness**.

---

### Physics & Simulation Safety

The project includes **gravity simulation and orbital mechanics**.

Check for:

* Deterministic behavior where possible
* Stable calculations
* Avoiding expensive per-frame work
* Safe handling of merged or dynamic bodies

Flag **performance or stability risks**.

---

### Scene Structure

When scenes change, ensure:

* Logical node grouping
* Reasonable depth
* Clear naming

Scenes must remain **readable and maintainable**.

---

### Documentation

Significant changes should update `/docs`.

Docs should briefly explain:

* Why the change exists
* What system it affects
* How it interacts with other systems

Keep documentation **short and practical**.

---

### Debug Tools

Encourage **toggleable debug visualizations** when working on physics systems, such as:

* Orbit paths
* Gravity fields
* Stability indicators

---

### Review Style

Feedback must be:

* Concise
* Specific
* Actionable

Avoid vague comments.

Prefer concrete suggestions and recommend **follow-up PRs** when appropriate.

---

### Approval Checklist

Approve only if:

* PR scope is small and focused
* Architecture remains modular
* Code follows Godot and C# best practices
* No unnecessary complexity introduced
* Documentation updated if needed

If all conditions pass, the PR is acceptable.
