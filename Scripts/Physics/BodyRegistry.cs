using System.Collections.Generic;

public class BodyRegistry
{
    private readonly Dictionary<string, CelestialBodyData> _bodies = new();

    public int Count => _bodies.Count;

    public void Add(CelestialBodyData body)
    {
        _bodies[body.Id] = body;
    }

    public bool Remove(string id)
    {
        return _bodies.Remove(id);
    }

    public CelestialBodyData GetById(string id)
    {
        return _bodies.TryGetValue(id, out var body) ? body : null;
    }

    public IReadOnlyCollection<CelestialBodyData> GetAll()
    {
        return _bodies.Values;
    }

    public void Clear()
    {
        _bodies.Clear();
    }
}
