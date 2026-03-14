using GdUnit4;
using Godot;

namespace GravityStellar.Tests.Unit;

[TestSuite]
public class ExampleTest
{
    [TestCase]
    public void VectorMagnitude_WithKnownVector_ReturnsCorrectLength()
    {
        var vector = new Vector2(3f, 4f);
        float expectedMagnitude = 5f;
        
        float actualMagnitude = vector.Length();
        
        Assertions.AssertThat(actualMagnitude).IsEqual(expectedMagnitude);
    }
    
    [TestCase]
    public void VectorNormalization_WithNonZeroVector_ReturnsUnitVector()
    {
        var vector = new Vector2(10f, 0f);
        
        var normalized = vector.Normalized();
        
        Assertions.AssertThat(normalized.Length()).IsEqual(1f);
        Assertions.AssertThat(normalized.X).IsEqual(1f);
        Assertions.AssertThat(normalized.Y).IsEqual(0f);
    }
    
    [TestCase]
    public void GravityCalculation_WithEqualMassesAndDistance_ReturnsExpectedForce()
    {
        const float G = 6.674e-11f;
        float mass1 = 100f;
        float mass2 = 100f;
        float distance = 10f;
        
        float expectedForce = G * (mass1 * mass2) / (distance * distance);
        float actualForce = CalculateGravitationalForce(mass1, mass2, distance);
        
        Assertions.AssertThat(actualForce).IsEqual(expectedForce);
    }
    
    [TestCase]
    public void GravityCalculation_WithZeroDistance_ThrowsException()
    {
        Assertions.AssertThrown(() => 
        {
            CalculateGravitationalForce(100f, 100f, 0f);
        }).IsInstanceOf<System.ArgumentException>();
    }
    
    [TestCase]
    public void GravityCalculation_WithNegativeMass_ThrowsException()
    {
        Assertions.AssertThrown(() => 
        {
            CalculateGravitationalForce(-100f, 100f, 10f);
        }).IsInstanceOf<System.ArgumentException>();
    }
    
    private float CalculateGravitationalForce(float mass1, float mass2, float distance)
    {
        if (distance == 0f)
        {
            throw new System.ArgumentException("Distance cannot be zero", nameof(distance));
        }
        
        if (mass1 < 0f || mass2 < 0f)
        {
            throw new System.ArgumentException("Mass cannot be negative");
        }
        
        const float G = 6.674e-11f;
        return G * (mass1 * mass2) / (distance * distance);
    }
}
