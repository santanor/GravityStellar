using Godot;
using System.Collections.Generic;
using GravityStellar.Visual;

public partial class PlanetSyncManager : Node
{
    private PackedScene _planetScene;
    private SimulationManager _simulationManager;
    private readonly Dictionary<string, PlanetVisual> _visuals = new();

    public override void _Ready()
    {
        _planetScene = GD.Load<PackedScene>("res://Scenes/PlanetScene.tscn");
        _simulationManager = GetNode<SimulationManager>("/root/SimulationManager");

        _simulationManager.BodyAdded += OnBodyAdded;
        _simulationManager.BodyRemoved += OnBodyRemoved;
    }

    private void OnBodyAdded(string bodyId)
    {
        var body = _simulationManager.Registry.GetById(bodyId);
        if (body is not Planet planet) return;

        var visual = _planetScene.Instantiate<PlanetVisual>();
        AddChild(visual);
        visual.Bind(planet);
        _visuals[bodyId] = visual;
    }

    private void OnBodyRemoved(string bodyId)
    {
        if (_visuals.TryGetValue(bodyId, out var visual))
        {
            visual.QueueFree();
            _visuals.Remove(bodyId);
        }
    }

    public override void _Process(double delta)
    {
        foreach (var visual in _visuals.Values)
        {
            visual.UpdateVisuals();
        }
    }
}
