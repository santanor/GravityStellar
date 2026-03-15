# Decision: Global TimeScale Multiplier for Physics Simulation

**Author:** Lambert (Physics Programmer)  
**Date:** 2026-03-15  
**Status:** Proposed  
**Branch:** `feature/demo-gravity-scene`

## Context

The simulation ran at real-time speed (TimeScale=1.0), which was too slow for gameplay — planets took a long time to visibly orbit. A global speed multiplier was needed.

## Decision

- Added `TimeScale` export property to `SimulationConfig` (default 2.0).
- `TimeScale` is clamped to `[0.1, MaxSimulationSpeed]` before use.
- `MaxSimulationSpeed` (already defined at 3.0 but unused) now serves as the upper bound.
- The multiplier is applied in `SimulationManager._PhysicsProcess()` by scaling the fixed timestep: `dt = FixedTimestep * clamp(TimeScale, 0.1, MaxSimulationSpeed)`.
- This uniformly affects all physics (gravity, integration, collision timing) — no per-system scaling needed.

## Consequences

- Adjusting `TimeScale` in the editor or at runtime speeds up or slows down the entire simulation.
- Values above `MaxSimulationSpeed` (3.0) are clamped — increase `MaxSimulationSpeed` if higher values are needed.
- Very high time scales may reduce integration accuracy; the Velocity Verlet integrator remains stable at moderate values (2–3x).
- Gameplay designers can tune feel without touching physics code.
