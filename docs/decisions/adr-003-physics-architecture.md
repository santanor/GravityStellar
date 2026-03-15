# ADR-003: Physics Architecture (Data-Driven, Velocity Verlet, Softened Gravity)

**Date:** 2026-03  
**Status:** Accepted  
**Author:** Dallas (Documentation Engineer)  
**Epic:** #26 (Core Physics Simulation)

## Context

GravityStellar is a mobile puzzle physics game where players spawn planetary bodies that interact via gravity, forming orbital systems that merge into larger bodies. The game requires:

- **Determinism:** Physics simulation must produce consistent results (for replay, testing, and cross-platform play)
- **Testability:** Physics logic must be independently testable without requiring the Godot runtime
- **Performance:** Mobile targets demand efficient per-frame calculations
- **Stability:** Orbital mechanics must remain stable over many frames without energy drift or singularities
- **Clean Separation:** Physics logic must be decoupled from rendering and gameplay decision-making

The architecture must support both unit testing (deterministic, repeatable) and scene-based integration testing while keeping the physics simulation free from Godot node dependencies.

### Options Considered

1. **Physics State in Godot Nodes (Monolithic Approach)**
   - Store position, velocity, mass directly on Node subclasses
   - Integrate physics in `_PhysicsProcess()`
   - Fast prototyping, tightly coupled to engine
   - Not unit-testable without scene runtime; non-deterministic

2. **Data-Driven Physics with Stateless Calculators (Chosen)**
   - Physics state in plain C# classes (CelestialBodyData, BodyRegistry)
   - Nodes are visual shells that sync from data each frame
   - Orchestrator (SimulationManager) owns all state and drives simulation
   - Calculators are pure, stateless utility classes
   - Unit-testable, deterministic, extensible

3. **Separate Physics Engine (Jolt/Bullet Integration)**
   - Offload physics to external library
   - Good for rigid bodies, less suitable for softened gravity / orbital mechanics
   - Additional dependency overhead

## Decision

Adopt a **data-driven physics architecture**:

1. **Physics State:** Store all state in plain C# classes (`CelestialBodyData`, `BodyRegistry`), not Godot nodes. Nodes are stateless visual representations.

2. **Integration Method:** Use **Velocity Verlet integration** over Euler:
   ```
   x(t+dt) = 2*x(t) - x(t-dt) + a(t)*dt²
   v(t+dt) = (x(t+dt) - x(t-dt)) / (2*dt)
   ```
   Advantages: Energy conservation, stability for orbital systems, second-order accuracy.

3. **Gravity Model:** Implement **softened gravity** with softening parameter ε:
   ```
   F = G * m1 * m2 / (r² + ε²)
   ```
   Softening prevents singularities at close range and improves numerical stability.

4. **Orchestration:** Single `SimulationManager` node owns:
   - All physics state (BodyRegistry)
   - All calculator dependencies (GravityCalculator, CollisionDetector)
   - Drives `_PhysicsProcess()` loop
   - No other scripts run physics calculations

5. **Collision Detection:** Detect overlaps; gameplay layer decides response (merge, bounce, etc.). Physics does not resolve collisions — it reports them.

6. **Stateless Calculators:** `GravityCalculator` and `CollisionDetector` are utility classes with static or pure methods. No instance state, no side effects.

## Rationale

- **Determinism:** Pure state + stateless functions = reproducible physics (critical for testing and replay)
- **Testability:** Physics logic tested independently in unit tests; no Godot runtime required
- **Stability:** Velocity Verlet preserves energy better than Euler; softening prevents edge-case blowups
- **Separation of Concerns:** Physics, rendering, and gameplay are independent layers
- **Extensibility:** New calculators (electromagnetism, wind) can be added without modifying core simulation
- **Performance:** Stateless calculators are cache-friendly; single orchestrator reduces per-frame overhead

## Consequences

### Enables

- **Unit Testing Without Runtime:** Physics tests run via GdUnit4Net or standard .NET testing
- **Deterministic Replay:** Same inputs → same physics outputs, every time
- **Easy Debugging:** Physics can be stepped through in isolation
- **Clear Interfaces:** Calculators define exactly what they consume and produce
- **Cross-Platform Consistency:** Same physics code runs identically on all platforms

### Constrains

- **All Physics Must Go Through SimulationManager:** No ad-hoc physics calculations in gameplay scripts
- **Nodes Are Visual Shells:** Any node that displays a body must sync state from BodyRegistry each frame
- **No Per-Node Physics Logic:** Physics behavior cannot be customized per-node; use parameters instead
- **Velocity Verlet Setup:** Integration requires storing previous position; initialization must be careful

### Trade-Offs

- **Slightly More Ceremony:** More classes required (Data, Registry, Calculator, Manager) than monolithic approach
- **Sync Overhead:** Nodes must read state each frame, but cost is negligible (simple field access)

## Extension Points

- **New Forces:** Add new calculator (e.g., `WindCalculator`, `ElectromagneticCalculator`) and register in `SimulationManager`
- **New Collision Behaviors:** Gameplay layer interprets collision reports; add new response logic without modifying physics
- **Integration Methods:** Replace Velocity Verlet if needed; interface remains the same
- **Softening Parameter:** Tunable per-game or per-scenario; stored in simulation config

## See Also

- [../physics-model.md](../physics-model.md) — Detailed physics equations and constants
- [../architecture.md](../architecture.md) — System-level architecture overview
- [ADR-002: GdUnit4Net Testing](adr-002-gdunit4net-testing.md) — How physics is tested
