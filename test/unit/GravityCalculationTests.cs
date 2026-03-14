using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using System.Collections.Generic;
using GravityStellar.Stubs;

namespace GravityStellar.Tests.Unit;

[TestSuite]
public class GravityCalculationTests
{
    private GravityCalculator _calculator = null!;
    private const float G = 6.674f;
    private const float Softening = 10.0f;

    [BeforeTest]
    public void Setup()
    {
        _calculator = new GravityCalculator();
    }

    // --- Two-Body Force ---

    [TestCase]
    public void TwoBody_EqualMasses_ForcesAreEqualAndOpposite()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        // Newton's 3rd law: forces must be equal magnitude, opposite direction
        var forceSum = bodyA.AccumulatedForce + bodyB.AccumulatedForce;
        AssertThat(forceSum.Length()).IsLess(1e-5f);
    }

    [TestCase]
    public void TwoBody_AttractionDirection_PointsTowardEachOther()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        // A should be pulled toward B (positive X)
        AssertThat(bodyA.AccumulatedForce.X).IsGreater(0f);
        // B should be pulled toward A (negative X)
        AssertThat(bodyB.AccumulatedForce.X).IsLess(0f);
    }

    [TestCase]
    public void TwoBody_ForceFormula_MatchesExpectedMagnitude()
    {
        var bodyA = new CelestialBodyData("a", 50f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 200f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        // F = G * m1 * m2 / (r² + ε²)
        float distSq = 100f * 100f;
        float softenedDistSq = distSq + Softening * Softening;
        float expectedForce = G * 50f * 200f / softenedDistSq;

        float actualForceMagnitude = bodyA.AccumulatedForce.Length();
        float diff = MathF.Abs(actualForceMagnitude - expectedForce);
        AssertThat(diff).IsLess(1e-3f);
    }

    [TestCase]
    public void TwoBody_UnequalMasses_ForceMagnitudesAreEqual()
    {
        var bodyA = new CelestialBodyData("a", 10f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 1000f, 20f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        float forceA = bodyA.AccumulatedForce.Length();
        float forceB = bodyB.AccumulatedForce.Length();
        float diff = MathF.Abs(forceA - forceB);
        AssertThat(diff).IsLess(1e-3f);
    }

    // --- Three-Body ---

    [TestCase]
    public void ThreeBody_AllPairsContribute_ForcesAreNonZero()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodyC = new CelestialBodyData("c", 100f, 5f,
            new Vector2(50f, 87f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB, bodyC };

        _calculator.CalculateForces(bodies, G, Softening);

        AssertThat(bodyA.AccumulatedForce.Length()).IsGreater(0f);
        AssertThat(bodyB.AccumulatedForce.Length()).IsGreater(0f);
        AssertThat(bodyC.AccumulatedForce.Length()).IsGreater(0f);
    }

    [TestCase]
    public void ThreeBody_TotalForce_SumsToZero()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 200f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodyC = new CelestialBodyData("c", 50f, 5f,
            new Vector2(50f, 80f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB, bodyC };

        _calculator.CalculateForces(bodies, G, Softening);

        // Newton's 3rd law extended: total internal force = 0
        var totalForce = bodyA.AccumulatedForce + bodyB.AccumulatedForce + bodyC.AccumulatedForce;
        AssertThat(totalForce.Length()).IsLess(1e-3f);
    }

    [TestCase]
    public void ThreeBody_SymmetricArrangement_SymmetricForces()
    {
        // Equilateral triangle, equal masses => equal force magnitudes
        float y = 86.6f; // ~100 * sin(60°)
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodyC = new CelestialBodyData("c", 100f, 5f,
            new Vector2(50f, y), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB, bodyC };

        _calculator.CalculateForces(bodies, G, Softening);

        float forceA = bodyA.AccumulatedForce.Length();
        float forceB = bodyB.AccumulatedForce.Length();
        float forceC = bodyC.AccumulatedForce.Length();

        // All magnitudes should be approximately equal
        AssertThat(MathF.Abs(forceA - forceB)).IsLess(1e-2f);
        AssertThat(MathF.Abs(forceB - forceC)).IsLess(1e-2f);
    }

    // --- Softening Prevents Singularity ---

    [TestCase]
    public void Softening_BodiesAtSamePosition_ForceIsFinite()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        // With softening, force at r=0 should be finite (not NaN or Infinity)
        AssertThat(float.IsNaN(bodyA.AccumulatedForce.X)).IsFalse();
        AssertThat(float.IsNaN(bodyA.AccumulatedForce.Y)).IsFalse();
        AssertThat(float.IsInfinity(bodyA.AccumulatedForce.X)).IsFalse();
        AssertThat(float.IsInfinity(bodyA.AccumulatedForce.Y)).IsFalse();
    }

    [TestCase]
    public void Softening_VeryCloseDistance_ForceIsCapped()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyClose = new CelestialBodyData("close", 100f, 5f,
            new Vector2(0.001f, 0f), Vector2.Zero);
        var bodyFar = new CelestialBodyData("far", 100f, 5f,
            new Vector2(1000f, 0f), Vector2.Zero);

        var closeTest = new List<CelestialBodyData> { bodyA, bodyClose };
        _calculator.CalculateForces(closeTest, G, Softening);
        float closeForce = bodyA.AccumulatedForce.Length();

        // At r ≈ 0, force ≈ G*m1*m2/ε²
        float maxExpected = G * 100f * 100f / (Softening * Softening);
        AssertThat(closeForce).IsLessEqual(maxExpected + 1e-3f);
    }

    [TestCase]
    public void Softening_ZeroSoftening_BodiesAtSamePosition_ForceIsZero()
    {
        // With zero softening and same position, direction is zero vector
        // so Normalized() returns Vector2.Zero => force = 0
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, 0f);

        // Direction.Normalized() on zero vector => zero force (safe behavior per issue #64)
        AssertThat(float.IsNaN(bodyA.AccumulatedForce.X)).IsFalse();
        AssertThat(float.IsNaN(bodyA.AccumulatedForce.Y)).IsFalse();
    }

    // --- Force Resets Each Step ---

    [TestCase]
    public void CalculateForces_ResetsForcesBefore_Accumulating()
    {
        var bodyA = new CelestialBodyData("a", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        // Manually set a pre-existing force
        bodyA.ApplyForce(new Vector2(9999f, 9999f));

        _calculator.CalculateForces(bodies, G, Softening);

        // The pre-existing force should be gone; only the gravity force remains
        AssertThat(bodyA.AccumulatedForce.X).IsLess(9999f);
    }

    // --- Edge Cases ---

    [TestCase]
    public void EmptyBodies_DoesNotThrow()
    {
        var bodies = new List<CelestialBodyData>();

        // Should not throw
        _calculator.CalculateForces(bodies, G, Softening);

        AssertThat(bodies.Count).IsEqual(0);
    }

    [TestCase]
    public void SingleBody_NoForcesApplied()
    {
        var body = new CelestialBodyData("lonely", 100f, 5f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { body };

        _calculator.CalculateForces(bodies, G, Softening);

        AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ZeroMassBody_ProducesZeroForce()
    {
        var bodyA = new CelestialBodyData("zero", 0f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("heavy", 100f, 5f,
            new Vector2(100f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, G, Softening);

        AssertThat(bodyA.AccumulatedForce).IsEqual(Vector2.Zero);
        AssertThat(bodyB.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ForceIncreasesAsDistanceDecreases()
    {
        var anchor = new CelestialBodyData("anchor", 100f, 5f,
            new Vector2(0f, 0f), Vector2.Zero);

        // Body at distance 200
        var farBody = new CelestialBodyData("far", 100f, 5f,
            new Vector2(200f, 0f), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { anchor, farBody }, G, Softening);
        float farForce = anchor.AccumulatedForce.Length();

        // Body at distance 50
        var nearBody = new CelestialBodyData("near", 100f, 5f,
            new Vector2(50f, 0f), Vector2.Zero);
        _calculator.CalculateForces(new List<CelestialBodyData> { anchor, nearBody }, G, Softening);
        float nearForce = anchor.AccumulatedForce.Length();

        AssertThat(nearForce).IsGreater(farForce);
    }

    // --- Determinism ---

    [TestCase]
    public void SameInputs_ProduceSameOutputs_Deterministic()
    {
        var bodiesA = new List<CelestialBodyData>
        {
            new CelestialBodyData("a", 100f, 5f, new Vector2(0f, 0f), Vector2.Zero),
            new CelestialBodyData("b", 200f, 10f, new Vector2(75f, 50f), Vector2.Zero)
        };
        _calculator.CalculateForces(bodiesA, G, Softening);
        var forceA1 = bodiesA[0].AccumulatedForce;
        var forceB1 = bodiesA[1].AccumulatedForce;

        var bodiesB = new List<CelestialBodyData>
        {
            new CelestialBodyData("a", 100f, 5f, new Vector2(0f, 0f), Vector2.Zero),
            new CelestialBodyData("b", 200f, 10f, new Vector2(75f, 50f), Vector2.Zero)
        };
        _calculator.CalculateForces(bodiesB, G, Softening);
        var forceA2 = bodiesB[0].AccumulatedForce;
        var forceB2 = bodiesB[1].AccumulatedForce;

        AssertThat(forceA1).IsEqual(forceA2);
        AssertThat(forceB1).IsEqual(forceB2);
    }
}
