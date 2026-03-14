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
- **PR #83 Review Fixes (2026-03-14):** Addressed 19 review comments on `feature/ai-ops-cicd-pipeline`. Key patterns to watch in future PRs:
  - **Doc-workflow drift:** Docs said Windows runner/30-day retention/push+PR trigger; workflow used ubuntu-latest/14-day/PR-only. Always verify docs match the actual YAML.
  - **Path consistency:** `dist/windows/` vs `build/windows/` appeared in docs, ADR, and workflow. Single source of truth is `export_presets.cfg` (`build/windows/`).
  - **Export filter hygiene:** `*.cs` in `include_filter` ships source code. Removed it; added `exclude_filter` for test DLLs (gdUnit4*, Microsoft.NET.Test*, NUnit*).
  - **Test namespace convention:** `GravityStellar.Tests.<SystemName>` — the bare `GravityStellar.Tests` namespace was wrong. ExampleTest moved to `test/unit/` with namespace `GravityStellar.Tests.Unit`.
  - **Naming convention:** Standardized on `*Test.cs` suffix (not `Test*.cs` prefix).
  - **Conditional test packages:** Test NuGet packages wrapped in `Condition="'$(Configuration)' != 'ExportRelease'"` to prevent leaking into exports.
  - **Build output gitignored:** `build/` and `dist/` added to `.gitignore`.
- **CI Export Fix (2026-03-14):** Fixed failing Windows export step. Root causes: (1) Case-sensitivity issue — export_presets.cfg had `Build/` but CI expected `build/windows/` (Linux is case-sensitive); (2) Signal 11 (SIGSEGV) crash during Godot shutdown on headless Linux runners — export completes successfully but Godot crashes afterward, causing step failure. Solution: Changed export_path to `build/windows/GravityStellar.exe` and added `continue-on-error: true` + verification step to confirm exe exists before failing. Signal 11 is a known Godot 4.x issue on CI runners — not a real failure if files are present.
- **CI Export Path Fix (2026-03-14):** `firebelley/godot-export@v7.0.0` defaults `use_preset_export_path: false`, which ignores the export path in `export_presets.cfg` and outputs to its own internal build directory. Our verify and upload steps expect files at `build/windows/`. Fix: added `use_preset_export_path: true` and `verbose: true` to the export step, plus improved the verify step to list directory contents on failure for easier debugging.
- **GdUnit4 Addon Updated to v6.1.1 (2026-03-14):** GdUnit4 GDScript addon v6.0.0 was incompatible with Godot 4.6 — `GdUnitFileAccess.gd:199` called `file.get_as_text(true)` but Godot 4.6 removed the `skip_cr` parameter from `FileAccess.get_as_text()`. This caused cascading GDScript compilation failures when Godot loaded the addon, which was the **actual root cause of the CI signal 11 crash** (not the export path issue). Updated the entire addon from v6.0.0 to v6.1.1 which has full Godot 4.6 support. The `continue-on-error` workaround for signal 11 may now be unnecessary.
- **ExportRelease Test Source Exclusion (2026-03-14):** The csproj already excluded test NuGet packages (GdUnit4, etc.) from ExportRelease via `Condition="'$(Configuration)' != 'ExportRelease'"`, but test `.cs` files under `test/` were still compiled, causing CS0246 errors. Fix: added `<Compile Remove="test\**\*.cs" />` conditional on ExportRelease. Lesson: when conditionally excluding packages, always also exclude the source files that depend on them.
- **CI Export Path Collection Fix (2026-03-14):** `firebelley/godot-export@v7.0.0` exports to its internal directory (`~/.local/share/godot/builds/Windows Desktop/`) regardless of `use_preset_export_path: true` when the signal 11 crash prevents the action's post-processing/copy step. The `build_directory` output is empty due to the crash. Fix: removed `use_preset_export_path`, added a "Collect Export Output" step that searches for the exported `.exe` in the action's known internal paths and copies all build artifacts to `build/windows/`. Also softened `.pck` check to a warning since PCK may be embedded in the exe. Key lesson: don't rely on GitHub Action outputs or post-processing when the action crashes — collect artifacts yourself from known filesystem locations.
