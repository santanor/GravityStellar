using Godot;

public partial class SimulationConfig : Node
{
    [Export] public float GravitationalConstant { get; set; } = 6.674f;
    [Export] public float SofteningParameter { get; set; } = 10.0f;
    [Export] public float FixedTimestep { get; set; } = 1.0f / 60.0f;
    [Export] public float MaxSimulationSpeed { get; set; } = 3.0f;
    [Export(PropertyHint.Range, "0.1,5.0,0.1")] public float TimeScale { get; set; } = 2.0f;
}
