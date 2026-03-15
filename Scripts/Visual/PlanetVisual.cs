using Godot;

namespace GravityStellar.Visual;

public partial class PlanetVisual : Node2D
{
    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public Planet BoundPlanet { get; private set; }

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void Bind(Planet planet)
    {
        BoundPlanet = planet;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (BoundPlanet == null)
            return;

        Position = BoundPlanet.Position;

        float scaleFactor = BoundPlanet.Radius / 50.0f;
        Scale = new Vector2(scaleFactor, scaleFactor);

        Modulate = BoundPlanet.PlanetColor;
    }
}
