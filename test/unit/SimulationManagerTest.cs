using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class SimulationManagerTest
{
    [TestCase]
    public void ShouldAddBodyToRegistry()
    {
        var registry = new BodyRegistry();
        var body = new CelestialBodyData("test-1", 10f, 1f, Vector2.Zero, Vector2.Zero);

        registry.Add(body);

        AssertThat(registry.Count).IsEqual(1);
        AssertThat(registry.GetById("test-1")).IsNotNull();
    }

    [TestCase]
    public void ShouldRemoveBodyFromRegistry()
    {
        var registry = new BodyRegistry();
        var body = new CelestialBodyData("test-1", 10f, 1f, Vector2.Zero, Vector2.Zero);

        registry.Add(body);
        bool removed = registry.Remove("test-1");

        AssertThat(removed).IsTrue();
        AssertThat(registry.Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldNotRemoveNonexistentBody()
    {
        var registry = new BodyRegistry();
        bool removed = registry.Remove("nonexistent");

        AssertThat(removed).IsFalse();
    }

    [TestCase]
    public void ShouldIntegrateAllComponentsTogether()
    {
        // Verify the full pipeline: registry → gravity → verlet → collision
        var registry = new BodyRegistry();
        var calculator = new GravityCalculator();
        var integrator = new VelocityVerletIntegrator(calculator);
        var detector = new CollisionDetector();

        var bodyA = new CelestialBodyData("a", 100f, 5f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 5f, new Vector2(50f, 0f), Vector2.Zero);
        registry.Add(bodyA);
        registry.Add(bodyB);

        var bodies = registry.GetAll();
        calculator.CalculateForces(bodies, 6.674f, 10f);
        integrator.Step(bodies, 1f / 60f, 6.674f, 10f);

        // After a step, bodies should have moved toward each other
        AssertThat(bodyA.Position.X).IsGreater(0f);
        AssertThat(bodyB.Position.X).IsLess(50f);

        // No collision yet at this distance
        var collisions = detector.DetectCollisions(bodies);
        AssertThat(collisions.Count).IsEqual(0);
    }

    [TestCase]
    public void ShouldDetectCollisionAfterManySteps()
    {
        var registry = new BodyRegistry();
        var calculator = new GravityCalculator();
        var integrator = new VelocityVerletIntegrator(calculator);
        var detector = new CollisionDetector();

        // Place bodies close enough that gravity pulls them together quickly
        var bodyA = new CelestialBodyData("a", 1000f, 2f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 1000f, 2f, new Vector2(10f, 0f), Vector2.Zero);
        registry.Add(bodyA);
        registry.Add(bodyB);

        var bodies = registry.GetAll();
        calculator.CalculateForces(bodies, 100f, 0.1f);

        bool collisionFound = false;
        for (int i = 0; i < 1000; i++)
        {
            integrator.Step(bodies, 0.01f, 100f, 0.1f);
            var collisions = detector.DetectCollisions(bodies);
            if (collisions.Count > 0)
            {
                collisionFound = true;
                break;
            }
        }

        AssertThat(collisionFound).IsTrue();
    }
}
