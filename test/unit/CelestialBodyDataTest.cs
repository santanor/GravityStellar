using GdUnit4;
using Godot;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class CelestialBodyDataTest
{
    [TestCase]
    public void Constructor_SetsAllProperties()
    {
        var position = new Vector2(10f, 20f);
        var velocity = new Vector2(1f, -1f);

        var body = new CelestialBodyData("test-id", 100f, 5f, position, velocity);

        Assertions.AssertThat(body.Id).IsEqual("test-id");
        Assertions.AssertThat(body.Mass).IsEqual(100f);
        Assertions.AssertThat(body.Radius).IsEqual(5f);
        Assertions.AssertThat(body.Position).IsEqual(position);
        Assertions.AssertThat(body.Velocity).IsEqual(velocity);
    }

    [TestCase]
    public void Constructor_GeneratesGuid_WhenIdIsNull()
    {
        var body = new CelestialBodyData(null, 1f, 1f, Vector2.Zero, Vector2.Zero);

        Assertions.AssertThat(body.Id).IsNotNull();
        Assertions.AssertThat(body.Id).IsNotEmpty();
        // Verify it's a valid GUID format
        Assertions.AssertThat(System.Guid.TryParse(body.Id, out _)).IsTrue();
    }

    [TestCase]
    public void Constructor_UsesProvidedId_WhenNotNull()
    {
        var body = new CelestialBodyData("my-planet", 1f, 1f, Vector2.Zero, Vector2.Zero);

        Assertions.AssertThat(body.Id).IsEqual("my-planet");
    }

    [TestCase]
    public void Constructor_GeneratesUniqueIds_ForDifferentBodies()
    {
        var body1 = new CelestialBodyData(null, 1f, 1f, Vector2.Zero, Vector2.Zero);
        var body2 = new CelestialBodyData(null, 1f, 1f, Vector2.Zero, Vector2.Zero);

        Assertions.AssertThat(body1.Id).IsNotEqual(body2.Id);
    }

    [TestCase]
    public void AccumulatedForce_IsZero_AfterConstruction()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ResetForce_ZerosAccumulatedForce()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);
        body.ApplyForce(new Vector2(50f, 30f));

        body.ResetForce();

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ApplyForce_AccumulatesForce()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);
        var force = new Vector2(5f, -3f);

        body.ApplyForce(force);

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(force);
    }

    [TestCase]
    public void ApplyForce_MultipleCalls_AccumulateNotReplace()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);
        var force1 = new Vector2(5f, 0f);
        var force2 = new Vector2(0f, 10f);
        var force3 = new Vector2(-3f, -2f);

        body.ApplyForce(force1);
        body.ApplyForce(force2);
        body.ApplyForce(force3);

        var expected = new Vector2(2f, 8f);
        Assertions.AssertThat(body.AccumulatedForce).IsEqual(expected);
    }

    [TestCase]
    public void ResetForce_ThenApplyForce_GivesOnlyNewForce()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);
        body.ApplyForce(new Vector2(100f, 200f));

        body.ResetForce();
        var newForce = new Vector2(7f, 3f);
        body.ApplyForce(newForce);

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(newForce);
    }

    [TestCase]
    public void ApplyForce_WithZeroForce_DoesNotChangeAccumulator()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);
        var initialForce = new Vector2(5f, 5f);
        body.ApplyForce(initialForce);

        body.ApplyForce(Vector2.Zero);

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(initialForce);
    }

    [TestCase]
    public void ApplyForce_OppositeForces_CancelOut()
    {
        var body = new CelestialBodyData("id", 10f, 2f, Vector2.Zero, Vector2.Zero);

        body.ApplyForce(new Vector2(10f, 10f));
        body.ApplyForce(new Vector2(-10f, -10f));

        Assertions.AssertThat(body.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void Constructor_WithZeroMass_Allowed()
    {
        var body = new CelestialBodyData("id", 0f, 1f, Vector2.Zero, Vector2.Zero);

        Assertions.AssertThat(body.Mass).IsEqual(0f);
    }

    [TestCase]
    public void Constructor_WithNegativeValues_Allowed()
    {
        var body = new CelestialBodyData("id", -5f, -1f, new Vector2(-100f, -200f), new Vector2(-1f, -1f));

        Assertions.AssertThat(body.Mass).IsEqual(-5f);
        Assertions.AssertThat(body.Radius).IsEqual(-1f);
    }

    [TestCase]
    public void Properties_AreMutable_ExceptId()
    {
        var body = new CelestialBodyData("immutable-id", 1f, 1f, Vector2.Zero, Vector2.Zero);

        body.Mass = 999f;
        body.Radius = 50f;
        body.Position = new Vector2(42f, 42f);
        body.Velocity = new Vector2(7f, 7f);

        Assertions.AssertThat(body.Mass).IsEqual(999f);
        Assertions.AssertThat(body.Radius).IsEqual(50f);
        Assertions.AssertThat(body.Position).IsEqual(new Vector2(42f, 42f));
        Assertions.AssertThat(body.Velocity).IsEqual(new Vector2(7f, 7f));
        Assertions.AssertThat(body.Id).IsEqual("immutable-id");
    }
}
