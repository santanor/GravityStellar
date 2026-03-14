using Godot;
using System;

namespace GravityStellar.Stubs;

/// <summary>
/// Stub for Scripts/Physics/CelestialBodyData.cs (issue #62).
/// Plain C# class — no Godot Node dependency.
/// Holds physics state: mass, radius, position, velocity, accumulated force.
/// </summary>
public class CelestialBodyData
{
    public string Id { get; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 AccumulatedForce { get; set; }

    public CelestialBodyData(string id, float mass, float radius, Vector2 position, Vector2 velocity)
    {
        if (mass < 0f)
            throw new ArgumentOutOfRangeException(nameof(mass), "Mass cannot be negative.");
        if (radius < 0f)
            throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative.");

        Id = id ?? Guid.NewGuid().ToString();
        Mass = mass;
        Radius = radius;
        Position = position;
        Velocity = velocity;
        AccumulatedForce = Vector2.Zero;
    }

    public void ResetForce()
    {
        AccumulatedForce = Vector2.Zero;
    }

    public void ApplyForce(Vector2 force)
    {
        AccumulatedForce += force;
    }
}
