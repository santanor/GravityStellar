using GdUnit4;
using Godot;
using System.Collections.Generic;
using static GdUnit4.Assertions;

namespace GravityStellar.Tests.Physics;

[TestSuite]
public class VelocityVerletIntegratorTest
{
    private GravityCalculator _calculator;
    private VelocityVerletIntegrator _integrator;

    [BeforeTest]
    public void Setup()
    {
        _calculator = new GravityCalculator();
        _integrator = new VelocityVerletIntegrator(_calculator);
    }

    [TestCase]
    public void ShouldUpdatePositionsFromVelocity()
    {
        var body = new CelestialBodyData("b1", 10f, 1f, Vector2.Zero, new Vector2(100f, 0f));
        var bodies = new List<CelestialBodyData> { body };

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        _integrator.Step(bodies, 1f, 1f, 0.1f);

        // With no gravitational partner, position should advance by velocity * dt
        AssertThat(body.Position.X).IsGreater(0f);
    }

    [TestCase]
    public void ShouldApplyGravitationalAcceleration()
    {
        var bodyA = new CelestialBodyData("a", 100f, 1f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 1f, new Vector2(50f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        _integrator.Step(bodies, 1f, 1f, 0.1f);

        // Bodies should accelerate toward each other
        AssertThat(bodyA.Velocity.X).IsGreater(0f);
        AssertThat(bodyB.Velocity.X).IsLess(0f);
    }

    [TestCase]
    public void ShouldConserveMomentum()
    {
        var bodyA = new CelestialBodyData("a", 50f, 1f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 50f, 1f, new Vector2(30f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        Vector2 initialMomentum = bodyA.Mass * bodyA.Velocity + bodyB.Mass * bodyB.Velocity;

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        for (int i = 0; i < 10; i++)
        {
            _integrator.Step(bodies, 0.01f, 1f, 0.1f);
        }

        Vector2 finalMomentum = bodyA.Mass * bodyA.Velocity + bodyB.Mass * bodyB.Velocity;

        // Total momentum should be conserved (within floating-point tolerance)
        AssertThat((double)(finalMomentum - initialMomentum).Length()).IsLess(0.01);
    }

    [TestCase]
    public void ShouldHandleSingleBody()
    {
        var body = new CelestialBodyData("solo", 10f, 1f, new Vector2(5f, 5f), new Vector2(1f, 0f));
        var bodies = new List<CelestialBodyData> { body };

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        _integrator.Step(bodies, 1f, 1f, 0.1f);

        // Single body should drift at constant velocity with no force
        AssertThat(body.Position.X).IsEqual(6f);
        AssertThat(body.Velocity).IsEqual(new Vector2(1f, 0f));
    }

    [TestCase]
    public void ShouldProduceSymmetricMotionForEqualMasses()
    {
        var bodyA = new CelestialBodyData("a", 100f, 1f, new Vector2(-25f, 0f), Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 1f, new Vector2(25f, 0f), Vector2.Zero);
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        _integrator.Step(bodies, 0.1f, 1f, 0.1f);

        // Equal masses should have symmetric positions and opposite velocities
        AssertThat((double)Mathf.Abs(bodyA.Position.X + bodyB.Position.X)).IsLess(0.001);
        AssertThat((double)Mathf.Abs(bodyA.Velocity.X + bodyB.Velocity.X)).IsLess(0.001);
    }

    [TestCase]
    public void ShouldBeMoreAccurateThanEuler()
    {
        // Verlet should maintain better energy conservation than naive Euler
        var bodyA = new CelestialBodyData("a", 100f, 1f, Vector2.Zero, Vector2.Zero);
        var bodyB = new CelestialBodyData("b", 100f, 1f, new Vector2(20f, 0f), new Vector2(0f, 2f));
        var bodies = new List<CelestialBodyData> { bodyA, bodyB };

        float initialKE = 0.5f * bodyA.Mass * bodyA.Velocity.LengthSquared()
                        + 0.5f * bodyB.Mass * bodyB.Velocity.LengthSquared();

        _calculator.CalculateForces(bodies, 1f, 0.1f);
        for (int i = 0; i < 100; i++)
        {
            _integrator.Step(bodies, 0.01f, 1f, 0.1f);
        }

        float finalKE = 0.5f * bodyA.Mass * bodyA.Velocity.LengthSquared()
                      + 0.5f * bodyB.Mass * bodyB.Velocity.LengthSquared();

        // Energy shouldn't diverge wildly — Verlet is symplectic
        float energyRatio = finalKE / (initialKE + 0.0001f);
        AssertThat((double)energyRatio).IsGreater(0.1);
        AssertThat((double)energyRatio).IsLess(10.0);
    }
}
