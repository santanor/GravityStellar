using GdUnit4;
using Godot;
using static GdUnit4.Assertions;
using System.Collections.Generic;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class CollisionDetectorTest
{
    private CollisionDetector _detector;

    [BeforeTest]
    public void Setup()
    {
        _detector = new CollisionDetector();
    }

    [TestCase]
    public void ShouldReturnEmptyForZeroOrOneBody()
    {
        AssertThat(_detector.DetectCollisions(new List<CelestialBodyData>()).Count).IsEqual(0);

        var single = new List<CelestialBodyData> { new("body-1", 10f, 1f, Vector2.Zero, Vector2.Zero) };
        AssertThat(_detector.DetectCollisions(single).Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldDetectOverlappingBodiesAndReturnIds()
    {
        var bodies = new List<CelestialBodyData>
        {
            new("planet-1", 10f, 5f, new Vector2(0, 0), Vector2.Zero),
            new("planet-2", 10f, 5f, new Vector2(3, 0), Vector2.Zero)
        };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(1);
        AssertThat(collisions[0].IdA).IsEqual("planet-1");
        AssertThat(collisions[0].IdB).IsEqual("planet-2");
    }

    [TestCase]
    public void ShouldNotDetectDistantBodies()
    {
        var bodies = new List<CelestialBodyData>
        {
            new("a", 10f, 1f, new Vector2(0, 0), Vector2.Zero),
            new("b", 10f, 1f, new Vector2(100, 0), Vector2.Zero)
        };
        AssertThat(_detector.DetectCollisions(bodies).Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldNotCollideWhenExactlyTouching()
    {
        // Strict less-than: distance == combined radii is not a collision
        var bodies = new List<CelestialBodyData>
        {
            new("a", 10f, 5f, new Vector2(0, 0), Vector2.Zero),
            new("b", 10f, 5f, new Vector2(10, 0), Vector2.Zero)
        };
        AssertThat(_detector.DetectCollisions(bodies).Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldDetectMultipleCollisions()
    {
        var bodies = new List<CelestialBodyData>
        {
            new("a", 10f, 5f, new Vector2(0, 0), Vector2.Zero),
            new("b", 10f, 5f, new Vector2(3, 0), Vector2.Zero),
            new("c", 10f, 5f, new Vector2(6, 0), Vector2.Zero)
        };
        AssertThat(_detector.DetectCollisions(bodies).Count).IsGreaterEqual(2);
    }

    [TestCase]
    public void ShouldDetectBarelyOverlappingBodies()
    {
        var bodies = new List<CelestialBodyData>
        {
            new("a", 10f, 5f, new Vector2(0, 0), Vector2.Zero),
            new("b", 10f, 5f, new Vector2(9.99f, 0), Vector2.Zero)
        };
        AssertThat(_detector.DetectCollisions(bodies).Count).IsEqual(1);
    }
}
