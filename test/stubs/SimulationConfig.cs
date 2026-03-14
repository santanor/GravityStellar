using Godot;
using System;

namespace GravityStellar.Stubs;

/// <summary>
/// Stub for Scripts/Physics/SimulationConfig.cs (issue #61).
/// The real implementation extends Node and uses [Export] properties.
/// This stub defines the contract: property defaults and validation.
/// </summary>
public class SimulationConfig
{
    private float _gravitationalConstant = 6.674f;
    private float _softeningParameter = 10.0f;
    private float _fixedTimestep = 1.0f / 60.0f;
    private float _maxSimulationSpeed = 3.0f;

    public float GravitationalConstant
    {
        get => _gravitationalConstant;
        set
        {
            if (value <= 0f)
                throw new ArgumentOutOfRangeException(nameof(GravitationalConstant),
                    "Gravitational constant must be positive.");
            _gravitationalConstant = value;
        }
    }

    public float SofteningParameter
    {
        get => _softeningParameter;
        set
        {
            if (value < 0f)
                throw new ArgumentOutOfRangeException(nameof(SofteningParameter),
                    "Softening parameter cannot be negative.");
            _softeningParameter = value;
        }
    }

    public float FixedTimestep
    {
        get => _fixedTimestep;
        set
        {
            if (value <= 0f)
                throw new ArgumentOutOfRangeException(nameof(FixedTimestep),
                    "Fixed timestep must be positive.");
            if (value > 1.0f)
                throw new ArgumentOutOfRangeException(nameof(FixedTimestep),
                    "Fixed timestep must not exceed 1 second.");
            _fixedTimestep = value;
        }
    }

    public float MaxSimulationSpeed
    {
        get => _maxSimulationSpeed;
        set
        {
            if (value <= 0f)
                throw new ArgumentOutOfRangeException(nameof(MaxSimulationSpeed),
                    "Max simulation speed must be positive.");
            _maxSimulationSpeed = value;
        }
    }
}
