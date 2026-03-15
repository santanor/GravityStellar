# Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Key Responsibilities

- Gravity simulation (N-body or simplified model)
- Orbital mechanics and stability
- Collision detection and merging physics
- Simulation performance
- Physics debug visualization tools

## Learnings

- **Repo:** https://github.com/santanor/GravityStellar — remote wiped clean on 2026-03-13, all old branches deleted, master reset to orphan commit.
- **Branch strategy:** `master` is the stable trunk. All work goes through `feature/*` branches with small PRs. No `develop` branch — we keep it simple.
- **PR #10** (`feature/godot-csharp-fresh-start`) is the foundational PR bringing in the Godot 4 C# project, squad state, CI workflows, and editor config.
- Force-pushing to master requires explicit owner approval (Jose authorized this one). Don't do it again without asking.
- **PR Review Standards:** Created `.github/copilot-review-instructions.md` (2026-03-13T22:37:47Z). Reviews enforce: small PRs (<150 lines, <5 files), Godot best practices, C# idioms, physics safety, scene structure, documentation, debug tooling. Concise, constructive, specific feedback only.
- **Epic 1 — Core Physics Simulation (2026-03-14):**
  - **PR #86** (`squad/60-folder-structure` → master): Project folder structure. Closes #60.
  - **PR #87** (`squad/61-simulation-config` → squad/60): SimulationConfig autoload. Closes #61.
  - **PR #88** (`squad/62-celestial-body-data` → squad/60): CelestialBodyData plain C# class. Closes #62.
  - **Branch `squad/63-body-registry`** (→ squad/62): BodyRegistry — Dictionary-keyed O(1) lookup for CelestialBodyData. 134 lines, 2 files. 8 GdUnit4 tests. Closes #63.
  - **Branch `squad/64-gravity-calculator`** (→ squad/63): GravityCalculator — N-body force calc with softening, F=G*m1*m2/(r²+ε²), Newton's 3rd law. 134 lines, 2 files. 6 GdUnit4 tests. Closes #64.
  - **Branch `squad/67-collision-detector`** (→ squad/63): CollisionDetector — squared-distance pairwise collision detection, returns ID pairs. 119 lines, 2 files. 6 GdUnit4 tests. Closes #67.
  - PRs for #63, #64, #67 could not be created via `gh` due to token scope (`public_repo` missing). Branches are pushed, coordinator handles PR creation.
  - All physics classes are stateless and pure C# (no Godot node inheritance) — keeps simulation decoupled from scene tree.
  - Stacking: #64 and #67 both branch from #63 (parallel). #63 branches from #62.
- **Epic 1 Batch 2 — Physics Foundation Complete (2026-03-14T22:15:00Z):**
  - **Outcome:** 3 physics branches pushed with implementations + unit tests (8 + 6 + 6 = 20 tests). Each PR < 150 lines, atomic scope. All code follows project patterns (composition, no Godot inheritance, deterministic logic). Ready for review.
  - **Test Quality:** Brett co-executed with Lambert to validate implementations. 18 comprehensive tests on squad/epic1-physics-tests branch now available for cross-check. All tests follow GdUnit4Net patterns and AssertThat() fluent API.
  - **Cross-team Learning:** Godot Node cleanup pattern documented in decisions.md. Test patterns established by Brett are consistent with Lambert's test implementations — team has aligned on testing standards.
  - PRs for all 4 branches need manual creation.
- **Epic 1 Final — Issues #65 and #66 (2026-03-14):**
  - **Branch `squad/65-velocity-verlet`** (→ squad/64): VelocityVerletIntegrator — 4-phase symplectic integrator (half-step v, full-step x, recalc forces, complete v). Pure C#, zero allocs per step, constructor-injected GravityCalculator. 38 lines impl, 6 GdUnit4 tests. Closes #65.
  - **Branch `squad/66-simulation-manager`** (→ squad/65, merges squad/61 + squad/67): SimulationManager — Godot Node autoload that wires BodyRegistry, GravityCalculator, VelocityVerletIntegrator, CollisionDetector, and SimulationConfig into a deterministic physics loop. Emits BodyAdded/BodyRemoved/CollisionDetected signals. 58 lines impl, 5 integration tests. Closes #66.
  - SimulationManager is the only Godot Node in the physics layer — everything else is pure C#.
  - PRs could not be created via `gh` (token scope). Branches pushed, coordinator handles PR creation.
  - **Epic 1 is now code-complete.** All 8 issues (#60-#67) implemented across 8 branches with full test coverage.
- **Epic 1 Session Complete (2026-03-14T22:20:00Z):**
   - **Scribe:** Orchestration log written to `.squad/orchestration-log/2026-03-14T22-20-lambert-finale.md`
   - **Scribe:** Session log written to `.squad/log/2026-03-14T22-20-epic1-complete.md`
   - **Scribe:** Decision inbox merged: `lambert-epic1-complete.md` → `.squad/decisions.md` (approved status). Inbox file deleted.
   - **Scribe:** All squad files staged for commit.
   - **Status:** Epic 1 code-complete, ready for PR review chain. All 8 issues on branches. PRs #86–88 merged to master. Branches #63–67 await manual PR creation.
- **TimeScale Multiplier (2026-03-15):**
  - Added `TimeScale` export property to `SimulationConfig` (default 2.0, range 0.1–5.0 in editor).
  - Wired into `SimulationManager._PhysicsProcess()`: effective `dt = FixedTimestep * clamp(TimeScale, 0.1, MaxSimulationSpeed)`.
  - `MaxSimulationSpeed` (3.0) was already defined but unused — now serves as the upper clamp for `TimeScale`.
  - This scales ALL physics uniformly (gravity, integration, collision timing) via a single global knob.
  - Committed on `feature/demo-gravity-scene` branch. Build passes, 2 files changed (+3 lines).
