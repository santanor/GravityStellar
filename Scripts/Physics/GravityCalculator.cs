using Godot;
using System.Collections.Generic;
using System.Linq;

public class GravityCalculator
{
    public void CalculateForces(IReadOnlyCollection<CelestialBodyData> bodies, float g, float softening)
    {
        var bodyList = bodies.ToList();

        foreach (var body in bodyList)
        {
            body.ResetForce();
        }

        for (int i = 0; i < bodyList.Count; i++)
        {
            for (int j = i + 1; j < bodyList.Count; j++)
            {
                var bodyA = bodyList[i];
                var bodyB = bodyList[j];

                Vector2 direction = bodyB.Position - bodyA.Position;
                float distanceSq = direction.LengthSquared();
                float softenedDistSq = distanceSq + softening * softening;

                float forceMagnitude = g * bodyA.Mass * bodyB.Mass / softenedDistSq;

                Vector2 forceDirection = direction.Normalized();
                Vector2 force = forceDirection * forceMagnitude;

                bodyA.ApplyForce(force);
                bodyB.ApplyForce(-force);  // Newton's 3rd law
            }
        }
    }
}
