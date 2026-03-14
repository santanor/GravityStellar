# Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Key Architecture Principles

- Extremely small PRs (<150 lines, <5 files, one concern)
- GitHub-first workflow (issues → PRs → review → merge)
- Physics logic separate from rendering separate from gameplay
- Composition over inheritance
- Signals for decoupling
- No god classes
- Keep scripts small and focused
- Keep physics logic deterministic where possible

## System Boundaries

- **Game Systems:** Gravity simulation, orbital mechanics, planet merging
- **Gameplay Systems:** Spawning bodies, star collection logic, level objectives
- **Visual Systems:** Particle trails, visual feedback
- **UI:** Menus, HUD, debug overlays

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

- **Repo:** https://github.com/santanor/GravityStellar — remote wiped clean on 2026-03-13, all old branches deleted, master reset to orphan commit.
- **Branch strategy:** `master` is the stable trunk. All work goes through `feature/*` branches with small PRs. No `develop` branch — we keep it simple.
- **PR #10** (`feature/godot-csharp-fresh-start`) is the foundational PR bringing in the Godot 4 C# project, squad state, CI workflows, and editor config.
- Force-pushing to master requires explicit owner approval (Jose authorized this one). Don't do it again without asking.
- **PR Review Standards:** Created `.github/copilot-review-instructions.md` (2026-03-13T22:37:47Z). Reviews enforce: small PRs (<150 lines, <5 files), Godot best practices, C# idioms, physics safety, scene structure, documentation, debug tooling. Concise, constructive, specific feedback only.
- **CI/CD Pipeline:** Created `.github/workflows/ci.yml` (2026-03-14). Two-job pipeline (test → build) on every PR to master. Tests via `MikeSchulze/gdUnit4-action@v1` (GdUnit4Net C#), Windows export via `firebelley/godot-export@v6`, artifact upload via `actions/upload-artifact@v4`, caching via `actions/cache@v4`. Godot 4.6, .NET 8.0, ubuntu-latest.
- **Export Presets:** Created `export_presets.cfg` with "Windows Desktop" preset. Export path: `build/windows/GravityStellar.exe`. Embed PCK, S3TC/BPTC textures, all resources included.
- **Copilot Instructions:** Created `.github/copilot-instructions.md` with project overview, GdUnit4Net testing conventions (namespace: `GravityStellar.Tests.<System>`, class naming: `<Class>Test`, attributes: `[TestSuite]`/`[TestCase]`), local test running, CI/CD overview, code style.
- **AI Ops Epic:** Decomposed DevOps epic into 10 issues across 5 priority tiers. Decision doc merged to `.squad/decisions/decisions.md`. Dependency graph: GdUnit4Net setup + export presets → CI pipeline → branch protection + docs. All Priority 1-3 issues completed; Priority 4 (branch protection) requires Jose approval; Priority 5 (mobile exports, release automation) planned for future.
- **Test Directory Created (2026-03-14):** Brett established `test/` directory structure with subdirectories: `unit/`, `integration/`, `scenes/`. This structure mirrors the source code organization and enables organized growth of the test suite as the project scales.
- **GitHub Issues Created (2026-03-14):** Created AI Ops & DevOps Epic #72 with 10 sub-issues (#73–#82) on `santanor/GravityStellar`. Sub-issues linked via task list in Epic body (gh CLI v2.87.2 doesn't support `--add-sub-issue`). Labels created: `devops`, `testing`, `ci-cd`, `documentation`, `dx`, `priority:high`, `priority:medium`, `future`, `epic`, `build`, `governance`, `squad`, `squad:ripley`, `squad:brett`, `squad:dallas`. Squad assignments: Brett → #73, Ripley → #74-78,#80, Dallas → #79. Issues #81-#82 are P5/future with no squad assignment yet.
