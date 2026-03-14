# Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Key Responsibilities

- Test infrastructure and test scenes
- Gameplay and physics system validation
- Edge case discovery and regression prevention
- Determinism testing for physics simulation

## Learnings

- **Repo:** https://github.com/santanor/GravityStellar — remote wiped clean on 2026-03-13, all old branches deleted, master reset to orphan commit.
- **Branch strategy:** `master` is the stable trunk. All work goes through `feature/*` branches with small PRs. No `develop` branch — we keep it simple.
- **PR #10** (`feature/godot-csharp-fresh-start`) is the foundational PR bringing in the Godot 4 C# project, squad state, CI workflows, and editor config.
- Force-pushing to master requires explicit owner approval (Jose authorized this one). Don't do it again without asking.
- **PR Review Standards:** Created `.github/copilot-review-instructions.md` (2026-03-13T22:37:47Z). Reviews enforce: small PRs (<150 lines, <5 files), Godot best practices, C# idioms, physics safety, scene structure, documentation, debug tooling. Concise, constructive, specific feedback only.
- **GdUnit4Net Setup (2026-03-14):** Integrated GdUnit4Net 5.0.0 as the testing framework. Key packages: `gdUnit4.api` 5.0.0 (test framework), `gdUnit4.test.adapter` 3.0.0 (IDE integration for VS/Rider/Code), `Microsoft.NET.Test.Sdk` 18.3.0 (test infrastructure). Created `test/` directory with subdirs: `unit/`, `integration/`, `scenes/`. Added `.gdunit4.json` config. Sample test (`ExampleTest.cs`) demonstrates GdUnit4 assertions and proper test structure. Tests use `[TestSuite]` and `[TestCase]` attributes. Note: `IsTestProject=false` in csproj because this is a Godot game project, not a pure test library.
- **CI/CD Pipeline Integration (2026-03-14):** Ripley completed `.github/workflows/ci.yml` with two-job pipeline: `test` (runs GdUnit4 tests via `MikeSchulze/gdUnit4-action@v1`) and `build` (Windows export via `firebelley/godot-export@v6`). Build depends on test success. Tests will execute automatically on every PR. Artifact upload configured for 14-day retention. This workflow will run our GdUnit4 tests in CI.
- **GitHub Epic #72 Created (2026-03-14):** Ripley created Epic #72 with 10 sub-issues (#73–#82) for AI Ops & DevOps. Brett assigned to issue #73 (Set up GdUnit4Net testing framework). Epic linked via task list in body. Ready to begin implementation.
- **Epic 1 Batch 2 — Physics Foundation Tests (2026-03-14T22:15:00Z):**
  - **Branch `squad/epic1-physics-tests`** (← master): Authored 18 comprehensive unit tests for CelestialBodyData (13 tests) and SimulationConfig (5 tests). Tests follow GdUnit4Net patterns, use fluent `AssertThat()` assertions, and are organized under `test/unit/` mirroring source structure. Build passing. Coverage includes initialization, property validation, edge cases, and configuration scenarios.
  - **Test Patterns Reference:** Lambert's physics implementations (#63, #64, #67) include 20 tests total. Brett's tests follow identical patterns, establishing consistency across the team. Cross-team learning: Use `.Free()` for Godot-derived objects in tests (GdUnit4Net cleanup pattern — see decisions.md).
  - PR needs manual creation.

