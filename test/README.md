# Test Suite

This directory contains automated tests for Gravity Stellar using **GdUnit4Net**.

## Framework

- **GdUnit4Net 5.0.0**: Modern C# unit testing framework for Godot 4
- **Test Adapter 3.0.0**: IDE integration for Visual Studio, VS Code, Rider
- **Microsoft.NET.Test.Sdk 18.3.0**: Required test infrastructure

## Test Conventions

### Directory Structure

```
test/
├── unit/          # Unit tests (isolated logic, no scene dependencies)
├── integration/   # Integration tests (multi-component interactions)
└── scenes/        # Test scenes (.tscn files for visual validation)
```

### Naming

- Test files: `*Test.cs` (e.g., `GravityTest.cs`)
- Test classes: `[TestSuite]` attribute, suffix with `Test`
- Test methods: `[TestCase]` attribute, descriptive names

### Example

```csharp
using GdUnit4;
using Godot;

namespace GravityStellar.Tests;

[TestSuite]
public class GravityTest
{
    [TestCase]
    public void CalculateForce_WithValidMasses_ReturnsPositiveForce()
    {
        // Arrange
        float mass1 = 100f;
        float mass2 = 200f;
        float distance = 10f;
        
        // Act
        float force = GravityHelper.CalculateForce(mass1, mass2, distance);
        
        // Assert
        Assertions.AssertThat(force).IsGreater(0f);
    }
}
```

## Running Tests

### Command Line
```bash
dotnet test
```

### IDE
- **Visual Studio**: Test Explorer (Ctrl+E, T)
- **Rider**: Unit Tests tool window
- **VS Code**: Test Explorer sidebar

### GdUnit4 UI (Godot Editor)
The Godot editor includes a GdUnit4 test runner UI for visual feedback.

## Test Types

### Unit Tests
Test isolated functions and classes without scene dependencies.
Focus: Logic correctness, edge cases, boundary conditions.

### Integration Tests
Test interactions between components (e.g., Planet + Gravity + Trail).
Focus: System behavior, component contracts, data flow.

### Test Scenes
Visual validation of gameplay systems.
Focus: Physics simulation, rendering, player interaction.

## Best Practices

1. **Keep tests fast**: Avoid unnecessary delays
2. **Test determinism**: Physics should produce consistent results
3. **Isolate tests**: No dependencies between test cases
4. **Document edge cases**: Explain why weird values are tested
5. **Assert clearly**: Use descriptive assertion messages

## Coverage Goals

- Core physics calculations: 100%
- Gameplay systems: 80%+
- UI components: 60%+
- Integration scenarios: Key user flows only
