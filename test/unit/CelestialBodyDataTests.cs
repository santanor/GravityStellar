using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using GravityStellar.Stubs;

namespace GravityStellar.Tests.Unit;

[TestSuite]
public class CelestialBodyDataTests
{
    // --- Construction ---

    [TestCase]
    public void Constructor_WithValidArgs_SetsAllProperties()
    {
        var body = new CelestialBodyData("planet-1", 100f, 10f,
            new Vector2(50f, 50f), new Vector2(1f, 0f));

        AssertThat(body.Id).IsEqual("planet-1");
        AssertThat(body.Mass).IsEqual(100f);
        AssertThat(body.Radius).IsEqual(10f);
        AssertThat(body.Position).IsEqual(new Vector2(50f, 50f));
        AssertThat(body.Velocity).IsEqual(new Vector2(1f, 0f));
        AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void Constructor_WithNullId_GeneratesGuid()
    {
        var body = new CelestialBodyData(null, 10f, 5f, Vector2.Zero, Vector2.Zero);

        AssertThat(body.Id).IsNotNull();
        AssertThat(body.Id).IsNotEmpty();
    }

    [TestCase]
    public void Constructor_WithNullId_GeneratesUniqueIds()
    {
        var body1 = new CelestialBodyData(null, 10f, 5f, Vector2.Zero, Vector2.Zero);
        var body2 = new CelestialBodyData(null, 10f, 5f, Vector2.Zero, Vector2.Zero);

        AssertThat(body1.Id).IsNotEqual(body2.Id);
    }

    [TestCase]
    public void Constructor_WithNegativeMass_ThrowsException()
    {
        AssertThrown(() =>
        {
            new CelestialBodyData("bad", -10f, 5f, Vector2.Zero, Vector2.Zero);
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void Constructor_WithNegativeRadius_ThrowsException()
    {
        AssertThrown(() =>
        {
            new CelestialBodyData("bad", 10f, -5f, Vector2.Zero, Vector2.Zero);
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void Constructor_WithZeroMass_Succeeds()
    {
        var body = new CelestialBodyData("zero-mass", 0f, 5f, Vector2.Zero, Vector2.Zero);
        AssertThat(body.Mass).IsEqual(0f);
    }

    [TestCase]
    public void Constructor_WithZeroRadius_Succeeds()
    {
        var body = new CelestialBodyData("zero-radius", 10f, 0f, Vector2.Zero, Vector2.Zero);
        AssertThat(body.Radius).IsEqual(0f);
    }

    // --- Force Accumulation ---

    [TestCase]
    public void ApplyForce_SingleForce_AccumulatesCorrectly()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);
        var force = new Vector2(10f, 5f);

        body.ApplyForce(force);

        AssertThat(body.AccumulatedForce).IsEqual(force);
    }

    [TestCase]
    public void ApplyForce_MultipleForces_AccumulatesSum()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);

        body.ApplyForce(new Vector2(10f, 0f));
        body.ApplyForce(new Vector2(0f, 10f));
        body.ApplyForce(new Vector2(-5f, -5f));

        AssertThat(body.AccumulatedForce).IsEqual(new Vector2(5f, 5f));
    }

    [TestCase]
    public void ApplyForce_OppositeForces_CancelOut()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);

        body.ApplyForce(new Vector2(100f, 200f));
        body.ApplyForce(new Vector2(-100f, -200f));

        AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ResetForce_AfterAccumulation_ZerosForce()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);
        body.ApplyForce(new Vector2(100f, 200f));

        body.ResetForce();

        AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ResetForce_WhenAlreadyZero_StaysZero()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);

        body.ResetForce();

        AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    // --- Position and Velocity Mutability ---

    [TestCase]
    public void Position_IsMutable_CanBeUpdated()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);

        body.Position = new Vector2(100f, 200f);

        AssertThat(body.Position).IsEqual(new Vector2(100f, 200f));
    }

    [TestCase]
    public void Velocity_IsMutable_CanBeUpdated()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);

        body.Velocity = new Vector2(5f, -3f);

        AssertThat(body.Velocity).IsEqual(new Vector2(5f, -3f));
    }

    // --- Momentum Conservation Math ---

    [TestCase]
    public void Momentum_MassTimesVelocity_IsCorrect()
    {
        var body = new CelestialBodyData("test", 25f, 5f,
            Vector2.Zero, new Vector2(4f, 3f));

        var momentum = body.Mass * body.Velocity;

        AssertThat(momentum).IsEqual(new Vector2(100f, 75f));
    }

    [TestCase]
    public void Momentum_TwoBodies_TotalIsConservedAfterForceExchange()
    {
        var bodyA = new CelestialBodyData("a", 10f, 5f,
            Vector2.Zero, new Vector2(2f, 0f));
        var bodyB = new CelestialBodyData("b", 20f, 5f,
            new Vector2(100f, 0f), new Vector2(-1f, 0f));

        var initialMomentum = bodyA.Mass * bodyA.Velocity + bodyB.Mass * bodyB.Velocity;

        // Simulate equal-and-opposite force application
        var force = new Vector2(5f, 0f);
        bodyA.ApplyForce(force);
        bodyB.ApplyForce(-force);

        // Apply acceleration: a = F/m, update velocity: v += a*dt
        float dt = 1.0f / 60.0f;
        bodyA.Velocity += (bodyA.AccumulatedForce / bodyA.Mass) * dt;
        bodyB.Velocity += (bodyB.AccumulatedForce / bodyB.Mass) * dt;

        var finalMomentum = bodyA.Mass * bodyA.Velocity + bodyB.Mass * bodyB.Velocity;

        // Momentum should be conserved (within floating-point tolerance)
        AssertThat((finalMomentum - initialMomentum).Length()).IsLess(1e-5f);
    }

    // --- Edge Cases ---

    [TestCase]
    public void Id_IsImmutable_RetainedAfterMutation()
    {
        var body = new CelestialBodyData("immutable-id", 10f, 5f,
            Vector2.Zero, Vector2.Zero);
        body.Mass = 999f;
        body.Position = new Vector2(1000f, 1000f);

        AssertThat(body.Id).IsEqual("immutable-id");
    }

    [TestCase]
    public void Mass_CanBeChangedAfterConstruction()
    {
        var body = new CelestialBodyData("test", 10f, 5f,
            Vector2.Zero, Vector2.Zero);

        body.Mass = 50f;

        AssertThat(body.Mass).IsEqual(50f);
    }

    [TestCase]
    public void ApplyForce_ZeroForce_NoEffect()
    {
        var body = new CelestialBodyData("test", 10f, 5f, Vector2.Zero, Vector2.Zero);
        body.ApplyForce(new Vector2(10f, 20f));

        body.ApplyForce(Vector2.Zero);

        AssertThat(body.AccumulatedForce).IsEqual(new Vector2(10f, 20f));
    }
}
