using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Gameplay;

[TestSuite]
public class PlanetTest
{
    [TestCase]
    public void ShouldCreateWithAllProperties()
    {
        var planet = new Planet(
            "planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero,
            "Earth", Colors.Blue, 1, true);

        AssertThat(planet.Id).IsEqual("planet-1");
        AssertThat(planet.Mass).IsEqual(10f);
        AssertThat(planet.Radius).IsEqual(1f);
        AssertThat(planet.DisplayName).IsEqual("Earth");
        AssertThat(planet.PlanetColor).IsEqual(Colors.Blue);
        AssertThat(planet.Tier).IsEqual(1);
        AssertThat(planet.IsPlayerSpawned).IsTrue();
    }

    [TestCase]
    public void ShouldCreateWithDefaults()
    {
        var planet = new Planet("planet-2", 5f, 0.5f, Vector2.One, Vector2.Zero);

        AssertThat(planet.Id).IsEqual("planet-2");
        AssertThat(planet.Mass).IsEqual(5f);
        AssertThat(planet.DisplayName).IsEqual(string.Empty);
        AssertThat(planet.PlanetColor).IsEqual(Colors.White);
        AssertThat(planet.Tier).IsEqual(0);
        AssertThat(planet.IsPlayerSpawned).IsFalse();
    }

    [TestCase]
    public void ShouldInheritCelestialBodyBehavior()
    {
        var planet = new Planet(
            "planet-3", 10f, 1f, Vector2.Zero, Vector2.Zero,
            "Mars", Colors.Red, 2, false);

        planet.ApplyForce(new Vector2(5f, 0f));
        AssertThat(planet.AccumulatedForce).IsEqual(new Vector2(5f, 0f));

        planet.ResetForce();
        AssertThat(planet.AccumulatedForce).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public void ShouldBeAssignableToCelestialBodyData()
    {
        var planet = new Planet(
            "planet-4", 10f, 1f, Vector2.Zero, Vector2.Zero,
            "Venus", Colors.Yellow, 1, true);

        CelestialBodyData body = planet;
        AssertThat(body).IsNotNull();
        AssertThat(body.Id).IsEqual("planet-4");
    }

    [TestCase]
    public void ShouldWorkWithBodyRegistry()
    {
        var registry = new BodyRegistry();
        var planet = new Planet(
            "planet-5", 15f, 1.5f, Vector2.Zero, Vector2.Zero,
            "Jupiter", Colors.Orange, 3, false);

        registry.Add(planet);

        AssertThat(registry.Count).IsEqual(1);
        var retrieved = registry.GetById("planet-5");
        AssertThat(retrieved).IsNotNull();
        AssertThat(retrieved).IsInstanceOf<Planet>();
    }

    [TestCase]
    public void ShouldAllowMutableGameplayProperties()
    {
        var planet = new Planet("planet-6", 10f, 1f, Vector2.Zero, Vector2.Zero);

        planet.DisplayName = "Evolved Planet";
        planet.PlanetColor = Colors.Green;
        planet.Tier = 2;
        planet.IsPlayerSpawned = true;

        AssertThat(planet.DisplayName).IsEqual("Evolved Planet");
        AssertThat(planet.PlanetColor).IsEqual(Colors.Green);
        AssertThat(planet.Tier).IsEqual(2);
        AssertThat(planet.IsPlayerSpawned).IsTrue();
    }
}
