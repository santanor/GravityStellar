using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class BodyRegistryTest
{
    private BodyRegistry _registry;

    [BeforeTest]
    public void Setup()
    {
        _registry = new BodyRegistry();
    }

    [TestCase]
    public void ShouldStartEmpty()
    {
        AssertThat(_registry.Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldAddBody()
    {
        var body = new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        _registry.Add(body);
        AssertThat(_registry.Count).IsEqual(1);
    }

    [TestCase]
    public void ShouldRetrieveBodyById()
    {
        var body = new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        _registry.Add(body);

        var retrieved = _registry.GetById("planet-1");
        AssertThat(retrieved).IsNotNull();
        AssertThat(retrieved.Id).IsEqual("planet-1");
    }

    [TestCase]
    public void ShouldReturnNullForUnknownId()
    {
        var result = _registry.GetById("nonexistent");
        AssertThat(result).IsNull();
    }

    [TestCase]
    public void ShouldRemoveBody()
    {
        var body = new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        _registry.Add(body);

        var removed = _registry.Remove("planet-1");
        AssertThat(removed).IsTrue();
        AssertThat(_registry.Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldReturnFalseWhenRemovingUnknownId()
    {
        var removed = _registry.Remove("nonexistent");
        AssertThat(removed).IsFalse();
    }

    [TestCase]
    public void ShouldGetAllBodies()
    {
        var body1 = new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var body2 = new CelestialBodyData("planet-2", 20f, 2f, Vector2.One, Vector2.Zero);
        _registry.Add(body1);
        _registry.Add(body2);

        var all = _registry.GetAll();
        AssertThat(all.Count).IsEqual(2);
    }

    [TestCase]
    public void ShouldOverwriteExistingBodyWithSameId()
    {
        var body1 = new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero);
        var body2 = new CelestialBodyData("planet-1", 50f, 5f, Vector2.One, Vector2.Zero);
        _registry.Add(body1);
        _registry.Add(body2);

        AssertThat(_registry.Count).IsEqual(1);
        AssertThat(_registry.GetById("planet-1").Mass).IsEqual(50f);
    }

    [TestCase]
    public void ShouldClearAllBodies()
    {
        _registry.Add(new CelestialBodyData("planet-1", 10f, 1f, Vector2.Zero, Vector2.Zero));
        _registry.Add(new CelestialBodyData("planet-2", 20f, 2f, Vector2.One, Vector2.Zero));
        _registry.Clear();

        AssertThat(_registry.Count).IsEqual(0);
    }
}
