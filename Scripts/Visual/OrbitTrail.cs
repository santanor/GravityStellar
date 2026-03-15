using Godot;

namespace GravityStellar.Visual;

public partial class OrbitTrail : Line2D
{
    [Export] public int MaxPoints { get; set; } = 100;
    [Export] public float MinDistance { get; set; } = 2.0f;

    private Vector2 _lastRecordedPosition;

    public override void _Ready()
    {
        Width = 2.0f;
        DefaultColor = new Color(1, 1, 1, 0.5f);
        TopLevel = true;

        var gradient = new Gradient();
        gradient.SetColor(0, new Color(1, 1, 1, 0.0f));
        gradient.SetColor(1, new Color(1, 1, 1, 0.7f));
        Gradient = gradient;
    }

    public void RecordPosition(Vector2 globalPos)
    {
        if (GetPointCount() > 0 && globalPos.DistanceTo(_lastRecordedPosition) < MinDistance)
            return;

        AddPoint(globalPos);
        _lastRecordedPosition = globalPos;

        while (GetPointCount() > MaxPoints)
        {
            RemovePoint(0);
        }
    }

    public void ClearTrail()
    {
        ClearPoints();
    }
}
