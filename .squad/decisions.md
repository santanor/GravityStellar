# Squad Decisions

## Active Decisions

### Repository Reset & Branch Strategy

**Author:** Ripley (Tech Lead)  
**Date:** 2026-03-13  
**Status:** Enacted (owner-approved)  

**Context:** The remote repo `santanor/GravityStellar` contained stale Unity-era branches and history that no longer applied to the Godot 4 C# rewrite. Jose requested a complete wipe.

**Decision:**
1. All old branches deleted (develop, feature/BasicUI, feature/GravitySytem)
2. Master history reset via orphan commit + force push
3. Branch strategy going forward: trunk-based with `master` as the only long-lived branch. All work via `feature/*` branches with small PRs (<150 lines, one concern).
4. No `develop` branch — unnecessary indirection for a small team.

**Consequences:**
- Old commit history is gone. If anyone needs it, it's in GitHub's reflog for ~90 days.
- Force pushes to master are now off-limits without explicit owner approval.
- PR #10 is the new baseline: all future work builds on top of it.

### Enforce Branch-Only Workflow

**Date:** 2026-03-14  
**Owner:** Ripley (Tech Lead / Architect)  
**Status:** Active

**Context:** To maintain code quality and traceability, enforce strict policy preventing direct commits to master/main. All changes must flow through feature branches and pull requests.

**Decision:** All agents must work on branches and open PRs. No direct commits to master or main under any circumstances.

- Agents create feature branches (`feature/*` or `squad/*` naming)
- All changes go through PR review
- Master/main remains stable and production-ready
- Owner (Jose) is the only exception with explicit approval

**Changes Made:**
1. `.squad/team.md` — Added bullet under "Architecture Principles"
2. `.squad/routing.md` — Added Rule 8 to "Rules" section
3. All agent charters (ripley, kane, lambert, parker, brett, dallas) — Added branch-only requirement

**Rationale:** Prevents accidental breaks, ensures traceability, enforces review, guarantees CI passing, and establishes team discipline.

**Consequences:** All work requires branch creation before coding, slightly longer commit-to-deploy cycle, full audit trail for all changes.

### User Directive — Branch-Only Policy

**Date:** 2026-03-14T18:26:00Z  
**By:** Jose (via Copilot)  
**Status:** Captured

**Directive:** Never push to master or main directly. All work must be done on a branch (feature/*, squad/*, etc.) and merged via PR.

**Purpose:** Captured for team memory and enforcement.

### Godot Node Test Cleanup Pattern

**Author:** Brett (QA / Test Engineer)  
**Date:** 2026-03-14  
**Status:** Proposed

**Context:** GdUnit4Net v5 does not provide `AutoFree` or `Memorypool` helpers for cleaning up Godot objects. Any test instantiating `Godot.Node` or `GodotObject` must manually free to avoid memory leaks.

**Decision:**
1. Call `.Free()` at end of each test method that creates a Node instance
2. For multiple assertions, place `.Free()` after all assertions
3. For setup/teardown, store node as field and free in `[AfterTest]`
4. Pure C# data classes don't need this — only Godot-derived types

**Example:**
```csharp
[TestCase]
public void DefaultValues_AreCorrect()
{
    var config = new SimulationConfig();
    AssertThat(config.GravitationalConstant).IsEqual(6.674f);
    config.Free();
}
```

**Rationale:** Prevents orphan Godot objects from accumulating during test runs, avoiding warnings or instability.

### Epic 1 — Core Physics Simulation is Code-Complete

**Author:** Lambert (Physics Programmer)
**Date:** 2026-03-14
**Status:** Approved

**Context:** Epic 1 defined the foundational physics simulation layer for Gravity Stellar. It required 8 issues (#60–#67) covering project structure, configuration, data models, body management, gravity computation, collision detection, numerical integration, and the simulation loop.

**Decision:** All 8 issues are now implemented with branches pushed to origin:

| Issue | Branch | Component |
|-------|--------|-----------|
| #60 | `squad/60-folder-structure` | Project folder structure |
| #61 | `squad/61-simulation-config` | SimulationConfig autoload |
| #62 | `squad/62-celestial-body-data` | CelestialBodyData class |
| #63 | `squad/63-body-registry` | BodyRegistry (O(1) lookup) |
| #64 | `squad/64-gravity-calculator` | GravityCalculator (N-body) |
| #65 | `squad/65-velocity-verlet` | VelocityVerletIntegrator |
| #66 | `squad/66-simulation-manager` | SimulationManager (capstone) |
| #67 | `squad/67-collision-detector` | CollisionDetector |

**Architecture Summary:**
- All physics classes except SimulationManager are pure C# — no Godot Node inheritance
- SimulationManager is a Godot Node registered as autoload, wiring all components
- Signal-based decoupling: BodyAdded, BodyRemoved, CollisionDetected
- Velocity Verlet integration for symplectic, energy-conserving simulation
- Forces use softening parameter to prevent singularities

**Test Coverage:** ~31 GdUnit4 unit/integration tests across all physics components.

**Consequences:**
- Epic 1 is ready for PR review and merge (stacked PR chain)
- PRs #86–#88 merged; PRs for #63–#67 need manual creation
- Epic 2 can begin planning (rendering, gameplay, UI layers)
- The physics layer is fully decoupled and testable without Godot scene tree

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
