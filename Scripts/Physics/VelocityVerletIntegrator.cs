using Godot;
using System.Collections.Generic;

public class VelocityVerletIntegrator
{
    private readonly GravityCalculator _gravityCalculator;

    public VelocityVerletIntegrator(GravityCalculator gravityCalculator)
    {
        _gravityCalculator = gravityCalculator;
    }

    public void Step(IReadOnlyCollection<CelestialBodyData> bodies, float dt, float g, float softening)
    {
        // Phase 1: Half-step velocity update using current acceleration
        foreach (var body in bodies)
        {
            Vector2 acceleration = body.AccumulatedForce / body.Mass;
            body.Velocity += 0.5f * acceleration * dt;
        }

        // Phase 2: Full-step position update
        foreach (var body in bodies)
        {
            body.Position += body.Velocity * dt;
        }

        // Phase 3: Recalculate forces at new positions
        _gravityCalculator.CalculateForces(bodies, g, softening);

        // Phase 4: Complete velocity update with new acceleration
        foreach (var body in bodies)
        {
            Vector2 acceleration = body.AccumulatedForce / body.Mass;
            body.Velocity += 0.5f * acceleration * dt;
        }
    }
}
