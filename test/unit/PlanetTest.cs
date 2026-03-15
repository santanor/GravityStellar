using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Gameplay;

/// <summary>
/// Anticipatory tests for the Planet data model (Issue #68).
/// Planet extends CelestialBodyData with gameplay-specific properties.
/// Written from spec before implementation — will not compile until
/// Kane's Planet class is merged.
/// </summary>
[TestSuite]
public class PlanetTest
{
    private const string TestId = "planet-test-1";
    private const float TestMass = 50f;
    private const float TestRadius = 5f;

    private Planet _planet;

    [BeforeTest]
    public void Setup()
    {
        _planet = new Planet(
            TestId,
            TestMass,
            TestRadius,
            new Vector2(100f, 200f),
            new Vector2(1f, -1f),
            "Earth",
            3
        );
    }

    // ── Constructor: explicit values ────────────────────────────────

    [TestCase]
    public void Constructor_ShouldSetId()
    {
        AssertThat(_planet.Id).IsEqual(TestId);
    }

    [TestCase]
    public void Constructor_ShouldSetMass()
    {
        AssertThat(_planet.Mass).IsEqual(TestMass);
    }

    [TestCase]
    public void Constructor_ShouldSetRadius()
    {
        AssertThat(_planet.Radius).IsEqual(TestRadius);
    }

    [TestCase]
    public void Constructor_ShouldSetPosition()
    {
        AssertThat(_planet.Position).IsEqual(new Vector2(100f, 200f));
    }

    [TestCase]
    public void Constructor_ShouldSetVelocity()
    {
        AssertThat(_planet.Velocity).IsEqual(new Vector2(1f, -1f));
    }

    [TestCase]
    public void Constructor_ShouldSetDisplayName()
    {
        AssertThat(_planet.DisplayName).IsEqual("Earth");
    }

    [TestCase]
    public void Constructor_ShouldSetTier()
    {
        AssertThat(_planet.Tier).IsEqual(3);
    }

    // ── Default values ──────────────────────────────────────────────

    [TestCase]
    public void Constructor_DefaultDisplayName_ShouldBePlanet()
    {
        var planet = new Planet("id-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.DisplayName).IsEqual("Planet");
    }

    [TestCase]
    public void Constructor_DefaultTier_ShouldBeOne()
    {
        var planet = new Planet("id-2", 10f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.Tier).IsEqual(1);
    }

    [TestCase]
    public void Constructor_DefaultPlanetColor_ShouldBeWhite()
    {
        var planet = new Planet("id-3", 10f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.PlanetColor).IsEqual(Colors.White);
    }

    [TestCase]
    public void Constructor_DefaultIsPlayerSpawned_ShouldBeFalse()
    {
        var planet = new Planet("id-4", 10f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.IsPlayerSpawned).IsFalse();
    }

    [TestCase]
    public void Constructor_ShouldInitializeAccumulatedForceToZero()
    {
        AssertThat(_planet.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    // ── Inheritance: Planet IS-A CelestialBodyData ──────────────────

    [TestCase]
    public void Planet_ShouldBeAssignableToCelestialBodyData()
    {
        CelestialBodyData body = _planet;
        AssertThat(body).IsNotNull();
        AssertThat(body.Id).IsEqual(TestId);
    }

    [TestCase]
    public void Planet_CastToCelestialBodyData_ShouldRetainAllBaseProperties()
    {
        CelestialBodyData body = _planet;
        AssertThat(body.Mass).IsEqual(TestMass);
        AssertThat(body.Radius).IsEqual(TestRadius);
        AssertThat(body.Position).IsEqual(new Vector2(100f, 200f));
        AssertThat(body.Velocity).IsEqual(new Vector2(1f, -1f));
    }

    // ── Inherited physics methods ───────────────────────────────────

    [TestCase]
    public void ApplyForce_ShouldAccumulateForceOnPlanet()
    {
        _planet.ApplyForce(new Vector2(10f, 5f));
        AssertThat(_planet.AccumulatedForce).IsEqual(new Vector2(10f, 5f));

        _planet.ApplyForce(new Vector2(-3f, 2f));
        AssertThat(_planet.AccumulatedForce).IsEqual(new Vector2(7f, 7f));
    }

    [TestCase]
    public void ResetForce_ShouldClearAccumulatedForceOnPlanet()
    {
        _planet.ApplyForce(new Vector2(50f, 50f));
        _planet.ResetForce();
        AssertThat(_planet.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ApplyForce_OnPlanetCastAsBase_ShouldStillWork()
    {
        CelestialBodyData body = _planet;
        body.ApplyForce(new Vector2(7f, 3f));
        AssertThat(_planet.AccumulatedForce).IsEqual(new Vector2(7f, 3f));
    }

    // ── Edge cases ──────────────────────────────────────────────────

    [TestCase]
    public void Constructor_NullId_ShouldGenerateGuid()
    {
        var planet = new Planet(null, 10f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.Id).IsNotNull();
        AssertThat(planet.Id.Length).IsGreater(0);
    }

    [TestCase]
    public void Constructor_ZeroMass_ShouldBeAllowed()
    {
        var planet = new Planet("zero-mass", 0f, 1f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.Mass).IsEqual(0f);
    }

    [TestCase]
    public void Constructor_NegativeTier_ShouldStoreValue()
    {
        // Spec doesn't restrict tier range — verify it stores whatever is given
        var planet = new Planet("neg-tier", 10f, 1f, Vector2.Zero, Vector2.Zero, "Test", -1);
        AssertThat(planet.Tier).IsEqual(-1);
    }

    [TestCase]
    public void PlanetColor_ShouldBeMutable()
    {
        _planet.PlanetColor = Colors.Red;
        AssertThat(_planet.PlanetColor).IsEqual(Colors.Red);
    }

    [TestCase]
    public void IsPlayerSpawned_ShouldBeMutable()
    {
        _planet.IsPlayerSpawned = true;
        AssertThat(_planet.IsPlayerSpawned).IsTrue();
    }

    [TestCase]
    public void Constructor_ZeroRadius_ShouldBeAllowed()
    {
        var planet = new Planet("zero-r", 10f, 0f, Vector2.Zero, Vector2.Zero);
        AssertThat(planet.Radius).IsEqual(0f);
    }

    // ── Integration: works with existing systems ────────────────────

    [TestCase]
    public void Planet_ShouldWorkWithBodyRegistry()
    {
        var registry = new BodyRegistry();
        var planet = new Planet(
            "registry-planet", 15f, 1.5f, Vector2.Zero, Vector2.Zero,
            "Jupiter", 3
        );

        registry.Add(planet);

        AssertThat(registry.Count).IsEqual(1);
        var retrieved = registry.GetById("registry-planet");
        AssertThat(retrieved).IsNotNull();
        AssertThat(retrieved).IsInstanceOf<Planet>();
    }
}
