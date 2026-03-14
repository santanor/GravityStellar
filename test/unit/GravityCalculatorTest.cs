using GdUnit4;
using Godot;
using System.Collections.Generic;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class GravityCalculatorTest
{
    private GravityCalculator _calculator;

    [BeforeTest]
    public void Setup()
    {
        _calculator = new GravityCalculator();
    }

    [TestCase]
    public void ShouldHandleEmptyAndSingleBody()
    {
        _calculator.CalculateForces(new List<CelestialBodyData>(), 1f, 0.1f);

        var bodies = new List<CelestialBodyData>
        {
            new("body-1", 10f, 1f, Vector2.Zero, Vector2.Zero)
        };
        _calculator.CalculateForces(bodies, 1f, 0.1f);
        AssertThat(bodies[0].AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ShouldApplyEqualAndOppositeForces()
    {
        var bodyA = new CelestialBodyData("a", 10f, 1f, new Vector2(0, 0), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 10f, 1f, new Vector2(10, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { bodyA, bodyB }, 1f, 0f);

        AssertThat(bodyA.AccumulatedForce.X).IsGreater(0f);
        AssertThat(bodyB.AccumulatedForce.X).IsLess(0f);
        AssertThat(bodyA.AccumulatedForce.Length())
            .IsEqualApprox(bodyB.AccumulatedForce.Length(), 0.001f);
    }

    [TestCase]
    public void ShouldScaleForceWithMass()
    {
        var a1 = new CelestialBodyData("a", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var b1 = new CelestialBodyData("b", 10f, 1f, new Vector2(10, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { a1, b1 }, 1f, 0f);

        var a2 = new CelestialBodyData("c", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var b2 = new CelestialBodyData("d", 20f, 1f, new Vector2(10, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { a2, b2 }, 1f, 0f);

        AssertThat(a2.AccumulatedForce.Length()).IsEqualApprox(a1.AccumulatedForce.Length() * 2f, 0.001f);
    }

    [TestCase]
    public void ShouldReduceForceWithDistance()
    {
        var near1 = new CelestialBodyData("a", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var near2 = new CelestialBodyData("b", 10f, 1f, new Vector2(5, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { near1, near2 }, 1f, 0f);

        var far1 = new CelestialBodyData("c", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var far2 = new CelestialBodyData("d", 10f, 1f, new Vector2(10, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { far1, far2 }, 1f, 0f);

        AssertThat(near1.AccumulatedForce.Length()).IsGreater(far1.AccumulatedForce.Length());
    }

    [TestCase]
    public void ShouldApplySoftening()
    {
        var a1 = new CelestialBodyData("a", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var b1 = new CelestialBodyData("b", 10f, 1f, new Vector2(1, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { a1, b1 }, 1f, 0f);

        var a2 = new CelestialBodyData("c", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var b2 = new CelestialBodyData("d", 10f, 1f, new Vector2(1, 0), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { a2, b2 }, 1f, 5f);

        AssertThat(a2.AccumulatedForce.Length()).IsLess(a1.AccumulatedForce.Length());
    }

    [TestCase]
    public void ShouldResetForcesBeforeCalculation()
    {
        var bodyA = new CelestialBodyData("a", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 10f, 1f, new Vector2(10, 0), Vector2.Zero);
        bodyA.ApplyForce(new Vector2(999, 999));

        _calculator.CalculateForces(new List<CelestialBodyData> { bodyA, bodyB }, 1f, 0f);
        AssertThat(bodyA.AccumulatedForce.X).IsLess(999f);
    }
}
