using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GravityStellar.Stubs;

namespace GravityStellar.Tests.Unit;

[TestSuite]
public class CollisionDetectionTests
{
    private CollisionDetector _detector = null!;

    [BeforeTest]
    public void Setup()
    {
        _detector = new CollisionDetector();
    }

    // --- Overlapping Bodies ---

    [TestCase]
    public void TwoBodiesOverlapping_DetectsCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 20f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 20f,
            new Vector2(30f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = 30, combined radii = 40 → collision
        AssertThat(collisions.Count).IsEqual(1);
        AssertThat(collisions[0].IdA).IsEqual("a");
        AssertThat(collisions[0].IdB).IsEqual("b");
    }

    [TestCase]
    public void TwoBodiesAtSamePosition_DetectsCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 10f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 10f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(1);
    }

    [TestCase]
    public void MultiplePairsOverlapping_DetectsAll()
    {
        var bodyA = new CelestialBodyData("a", 100f, 30f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 30f,
            new Vector2(40f, 0f), Vector2.Zero);
        var bodyC = new CelestialBodyData("c", 100f, 30f,
            new Vector2(20f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB, bodyC };

        var collisions = _detector.DetectCollisions(bodies);

        // A-B: dist=40, radii=60 → yes
        // A-C: dist=20, radii=60 → yes
        // B-C: dist=20, radii=60 → yes
        AssertThat(collisions.Count).IsEqual(3);
    }

    // --- Near-Miss ---

    [TestCase]
    public void TwoBodiesJustOutsideRange_NoCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 10f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 10f,
            new Vector2(20.01f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = 20.01, combined radii = 20 → no collision
        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void TwoBodiesExactlyTouching_NoCollision()
    {
        // Collision requires strict overlap (distSq < combinedRadii²)
        // At exact boundary, distance == combined radii → no collision
        var bodyA = new CelestialBodyData("a", 100f, 10f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 10f,
            new Vector2(20f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = 20, combined radii = 20 → strictly equal, not less-than
        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void TwoBodiesFarApart_NoCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 10f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 10f,
            new Vector2(1000f, 1000f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(0);
    }

    // --- Edge Cases: Zero-Radius Bodies ---

    [TestCase]
    public void ZeroRadiusBodies_AtSamePosition_NoCollision()
    {
        // Combined radii = 0, distance = 0
        // distSq (0) < 0*0 (0) is false → no collision
        var bodyA = new CelestialBodyData("a", 100f, 0f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 0f,
            new Vector2(50f, 50f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void OneZeroRadiusBody_InsideOther_DetectsCollision()
    {
        // Point body inside a large body → should detect
        var bigBody = new CelestialBodyData("big", 100f, 50f,
            new Vector2(0f, 0f), Vector2.Zero);
        var pointBody = new CelestialBodyData("point", 10f, 0f,
            new Vector2(10f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bigBody, pointBody };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = 10, combined radii = 50 → collision
        AssertThat(collisions.Count).IsEqual(1);
    }

    // --- Edge Cases: Empty and Single ---

    [TestCase]
    public void EmptyBodies_ReturnsEmptyList()
    {
        var bodies = new List<CelestialBodyData>();

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void SingleBody_ReturnsEmptyList()
    {
        var body = new CelestialBodyData("lonely", 100f, 10f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { body };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(0);
    }

    // --- Collision IDs ---

    [TestCase]
    public void CollisionPair_ReturnsCorrectIds()
    {
        var bodyA = new CelestialBodyData("planet-alpha", 100f, 20f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("planet-beta", 100f, 20f,
            new Vector2(10f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions[0].IdA).IsEqual("planet-alpha");
        AssertThat(collisions[0].IdB).IsEqual("planet-beta");
    }

    [TestCase]
    public void NoPairsAreDuplicated()
    {
        var bodyA = new CelestialBodyData("a", 100f, 100f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 100f,
            new Vector2(10f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Should only have (a,b), not also (b,a)
        AssertThat(collisions.Count).IsEqual(1);
    }

    // --- Diagonal Distance ---

    [TestCase]
    public void DiagonalOverlap_DetectsCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 20f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 20f,
            new Vector2(20f, 20f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = sqrt(800) ≈ 28.28, combined radii = 40 → collision
        AssertThat(collisions.Count).IsEqual(1);
    }

    [TestCase]
    public void DiagonalNearMiss_NoCollision()
    {
        var bodyA = new CelestialBodyData("a", 100f, 10f,
            new Vector2(0f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 10f,
            new Vector2(15f, 15f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        var collisions = _detector.DetectCollisions(bodies);

        // Distance = sqrt(450) ≈ 21.21, combined radii = 20 → no collision
        AssertThat(collisions.Count).IsEqual(0);
    }

    // --- Many Bodies ---

    [TestCase]
    public void ManyBodies_NoOverlaps_ReturnsEmpty()
    {
        var bodies = new List<CelestialBodyData>();
        for (int i = 0; i < 10; i++)
        {
            bodies.Add(new CelestialBodyData(
                $"body-{i}", 10f, 5f,
                new Vector2(i * 100f, 0f), Vector2.Zero));
        }

        var collisions = _detector.DetectCollisions(bodies);

        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void ManyBodies_AllOverlapping_DetectsAllPairs()
    {
        // 5 bodies all at roughly the same position with large radius
        var bodies = new List<CelestialBodyData>();
        for (int i = 0; i < 5; i++)
        {
            bodies.Add(new CelestialBodyData(
                $"body-{i}", 10f, 100f,
                new Vector2(i * 1f, 0f), Vector2.Zero));
        }

        var collisions = _detector.DetectCollisions(bodies);

        // N*(N-1)/2 = 5*4/2 = 10 pairs
        AssertThat(collisions.Count).IsEqual(10);
    }
}
