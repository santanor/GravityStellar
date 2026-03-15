using Godot;
using GdUnit4;
using GravityStellar.Visual;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Visual;

[TestSuite]
public class PlanetVisualTest
{
    [TestCase]
    public void ShouldHaveNullBoundPlanetInitially()
    {
        var visual = new PlanetVisual();
        AssertThat(visual.BoundPlanet).IsNull();
    }

    [TestCase]
    public void ShouldBindPlanet()
    {
        var planet = new Planet("test-1", 100f, 50f, Vector2.Zero, Vector2.Zero);
        var visual = new PlanetVisual();
        visual.Bind(planet);
        AssertThat(visual.BoundPlanet).IsEqual(planet);
    }

    [TestCase]
    public void ShouldUpdatePositionFromPlanet()
    {
        var expectedPosition = new Vector2(200f, 300f);
        var planet = new Planet("test-2", 100f, 50f, expectedPosition, Vector2.Zero);
        var visual = new PlanetVisual();
        visual.Bind(planet);
        AssertThat(visual.Position).IsEqual(expectedPosition);
    }

    [TestCase]
    public void ShouldUpdateScaleFromRadius()
    {
        float radius = 100f;
        float expectedScale = radius / 50.0f;
        var planet = new Planet("test-3", 100f, radius, Vector2.Zero, Vector2.Zero);
        var visual = new PlanetVisual();
        visual.Bind(planet);
        AssertThat(visual.Scale).IsEqual(new Vector2(expectedScale, expectedScale));
    }

    [TestCase]
    public void ShouldUpdateColorFromPlanetColor()
    {
        var expectedColor = Colors.Red;
        var planet = new Planet("test-4", 100f, 50f, Vector2.Zero, Vector2.Zero,
            "Red Planet", expectedColor, 1, false);
        var visual = new PlanetVisual();
        visual.Bind(planet);
        AssertThat(visual.Modulate).IsEqual(expectedColor);
    }

    [TestCase]
    public void ShouldNotCrashWhenUpdateVisualsCalledWithoutBind()
    {
        var visual = new PlanetVisual();
        visual.UpdateVisuals();
        AssertThat(visual.BoundPlanet).IsNull();
    }

    [TestCase]
    public void ShouldUpdateVisualsAfterPlanetDataChanges()
    {
        var planet = new Planet("test-5", 100f, 50f, Vector2.Zero, Vector2.Zero);
        var visual = new PlanetVisual();
        visual.Bind(planet);

        var newPosition = new Vector2(500f, 600f);
        planet.Position = newPosition;
        planet.Radius = 200f;
        visual.UpdateVisuals();

        AssertThat(visual.Position).IsEqual(newPosition);
        float expectedScale = 200f / 50.0f;
        AssertThat(visual.Scale).IsEqual(new Vector2(expectedScale, expectedScale));
    }
}
