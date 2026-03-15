using Godot;

public class Planet : CelestialBodyData
{
    public string DisplayName { get; set; }
    public Color PlanetColor { get; set; }
    public int Tier { get; set; }
    public bool IsPlayerSpawned { get; set; }

    public Planet(
        string id,
        float mass,
        float radius,
        Vector2 position,
        Vector2 velocity,
        string displayName,
        Color planetColor,
        int tier,
        bool isPlayerSpawned)
        : base(id, mass, radius, position, velocity)
    {
        DisplayName = displayName;
        PlanetColor = planetColor;
        Tier = tier;
        IsPlayerSpawned = isPlayerSpawned;
    }

    public Planet(
        string id,
        float mass,
        float radius,
        Vector2 position,
        Vector2 velocity)
        : base(id, mass, radius, position, velocity)
    {
        DisplayName = string.Empty;
        PlanetColor = Colors.White;
        Tier = 0;
        IsPlayerSpawned = false;
    }
}
