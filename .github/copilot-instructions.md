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
