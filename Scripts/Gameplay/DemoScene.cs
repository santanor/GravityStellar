using Godot;

namespace GravityStellar.Gameplay;

public partial class DemoScene : Node2D
{
    public override void _Ready()
    {
        var sim = GetNode<SimulationManager>("/root/SimulationManager");

        // Central sun — massive, stationary, anchors the system
        var sun = new Planet(
            id: "sun",
            mass: 5000f,
            radius: 30f,
            position: new Vector2(512, 300),
            velocity: Vector2.Zero,
            displayName: "Sol",
            planetColor: Colors.Yellow,
            tier: 3,
            isPlayerSpawned: false);
        sim.AddBody(sun);

        // Blue orbiter — circular orbit to the right
        // v = sqrt(G * M / r) = sqrt(6.674 * 5000 / 200) ≈ 12.9
        var orbiter1 = new Planet(
            id: "orbiter-blue",
            mass: 50f,
            radius: 10f,
            position: new Vector2(712, 300),
            velocity: new Vector2(0, -12.9f),
            displayName: "Azure",
            planetColor: Colors.CornflowerBlue,
            tier: 1,
            isPlayerSpawned: false);
        sim.AddBody(orbiter1);

        // Orange orbiter — opposite side, slightly faster for an elliptical orbit
        var orbiter2 = new Planet(
            id: "orbiter-orange",
            mass: 80f,
            radius: 12f,
            position: new Vector2(312, 300),
            velocity: new Vector2(0, 15f),
            displayName: "Ember",
            planetColor: Colors.Orange,
            tier: 1,
            isPlayerSpawned: false);
        sim.AddBody(orbiter2);

        // Green orbiter — approaches from above
        // v = sqrt(6.674 * 5000 / 150) ≈ 14.9
        var orbiter3 = new Planet(
            id: "orbiter-green",
            mass: 30f,
            radius: 8f,
            position: new Vector2(512, 150),
            velocity: new Vector2(14.9f, 0),
            displayName: "Verdant",
            planetColor: Colors.LimeGreen,
            tier: 1,
            isPlayerSpawned: false);
        sim.AddBody(orbiter3);

        // Tiny fast moon — close orbit around the system
        var moon = new Planet(
            id: "moon-tiny",
            mass: 10f,
            radius: 5f,
            position: new Vector2(612, 300),
            velocity: new Vector2(0, -18f),
            displayName: "Pebble",
            planetColor: Colors.Cyan,
            tier: 1,
            isPlayerSpawned: false);
        sim.AddBody(moon);
    }
}
