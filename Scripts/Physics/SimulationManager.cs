using Godot;

public partial class SimulationManager : Node
{
    private BodyRegistry _registry;
    private GravityCalculator _gravityCalculator;
    private VelocityVerletIntegrator _integrator;
    private SimulationConfig _config;
    private CollisionDetector _collisionDetector;

    [Signal] public delegate void BodyAddedEventHandler(string bodyId);
    [Signal] public delegate void BodyRemovedEventHandler(string bodyId);
    [Signal] public delegate void CollisionDetectedEventHandler(string bodyAId, string bodyBId);

    public BodyRegistry Registry => _registry;

    public override void _Ready()
    {
        _config = GetNode<SimulationConfig>("/root/SimulationConfig");
        _registry = new BodyRegistry();
        _gravityCalculator = new GravityCalculator();
        _collisionDetector = new CollisionDetector();
        _integrator = new VelocityVerletIntegrator(_gravityCalculator);

        _gravityCalculator.CalculateForces(
            _registry.GetAll(),
            _config.GravitationalConstant,
            _config.SofteningParameter);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_registry.Count < 2) return;

        float dt = _config.FixedTimestep;

        _integrator.Step(
            _registry.GetAll(), dt,
            _config.GravitationalConstant,
            _config.SofteningParameter);

        var collisions = _collisionDetector.DetectCollisions(_registry.GetAll());
        foreach (var (idA, idB) in collisions)
        {
            EmitSignal(SignalName.CollisionDetected, idA, idB);
        }
    }

    public void AddBody(CelestialBodyData body)
    {
        _registry.Add(body);
        EmitSignal(SignalName.BodyAdded, body.Id);
    }

    public void RemoveBody(string id)
    {
        if (_registry.Remove(id))
        {
            EmitSignal(SignalName.BodyRemoved, id);
        }
    }
}
