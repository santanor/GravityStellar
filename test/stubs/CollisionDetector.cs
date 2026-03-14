using System.Collections.Generic;
using System.Linq;

namespace GravityStellar.Stubs;

/// <summary>
/// Stub for Scripts/Physics/CollisionDetector.cs (issue #67).
/// Stateless radius-based collision detection.
/// Detection only — resolution is a gameplay concern.
/// </summary>
public class CollisionDetector
{
    public List<(string IdA, string IdB)> DetectCollisions(IReadOnlyCollection<CelestialBodyData> bodies)
    {
        var collisions = new List<(string, string)>();
        var bodyList = bodies.ToList();

        for (int i = 0; i < bodyList.Count; i++)
        {
            for (int j = i + 1; j < bodyList.Count; j++)
            {
                var bodyA = bodyList[i];
                var bodyB = bodyList[j];

                float distanceSq = (bodyB.Position - bodyA.Position).LengthSquared();
                float combinedRadii = bodyA.Radius + bodyB.Radius;

                if (distanceSq < combinedRadii * combinedRadii)
                {
                    collisions.Add((bodyA.Id, bodyB.Id));
                }
            }
        }

        return collisions;
    }
}
