# Scripts/Physics

Plain C# classes for the physics simulation layer.

**What belongs here:**
- `CelestialBodyData` - data model for simulated bodies
- `GravityCalculator` - gravitational force computation
- `Integrator` - numerical integration (Euler, Verlet, etc.)
- `SimulationConfig` - simulation constants and tuning parameters
- Collision detection and merging logic

**What does NOT belong here:**
- Godot node scripts (use `Scripts/Visual/`)
- Gameplay logic like scoring or spawning (use `Scripts/Gameplay/`)
