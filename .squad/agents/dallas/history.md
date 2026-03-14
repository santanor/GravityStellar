# Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Key Responsibilities

- System architecture documentation (/docs/architecture.md)
- Gameplay systems documentation (/docs/gameplay-systems.md)
- Physics model documentation (/docs/physics-model.md)
- Architecture Decision Records (/docs/decisions/)
- README and onboarding

## Learnings

- **Repo:** https://github.com/santanor/GravityStellar — remote wiped clean on 2026-03-13, all old branches deleted, master reset to orphan commit.
- **Branch strategy:** `master` is the stable trunk. All work goes through `feature/*` branches with small PRs. No `develop` branch — we keep it simple.
- **PR #10** (`feature/godot-csharp-fresh-start`) is the foundational PR bringing in the Godot 4 C# project, squad state, CI workflows, and editor config.
- Force-pushing to master requires explicit owner approval (Jose authorized this one). Don't do it again without asking.
- Created .github/copilot-review-instructions.md for PR review standards (2026-03-13T22:37:47Z)
- File location: .github/copilot-review-instructions.md (standard GitHub Copilot path)
- Reviews enforce: small PRs (<150 lines, <5 files), Godot best practices, C# idioms, physics safety, scene structure, documentation, debug tooling. Concise, constructive, specific feedback.
- **CI/CD & Testing Documentation** (2026-03): Created comprehensive docs for new AI Ops / DevOps Epic:
  - `docs/decisions/adr-002-gdunit4net-testing.md` — Decision to use GdUnit4Net (C# port of GdUnit4), native Godot 4 integration, scene testing, fluent assertions
  - `docs/decisions/adr-003-cicd-pipeline.md` — GitHub Actions pipeline with two jobs: test (GdUnit4-action) and build (godot-export), includes caching strategy and artifact upload
  - `docs/ci-cd.md` — Detailed pipeline documentation: stage breakdown, local testing, downloading artifacts, adding new export presets, troubleshooting, performance tuning
  - Updated `README.md` with: Getting Started, Testing (GdUnit4Net local + CLI), CI/CD Overview, artifact download instructions, documentation index, development workflow, contribution guidelines
- **Directory structure:** Created `docs/` and `docs/decisions/` directories for architecture documentation
- **Epic Documentation** (2026-03-14): Orchestration complete. All ADRs merged to `.squad/decisions/decisions.md`, inbox files deleted. Ready for team review and PR submission.
- **GitHub Epic #72 Created (2026-03-14):** Ripley created Epic #72 with 10 sub-issues (#73–#82) for AI Ops & DevOps. Dallas assigned to issue #79 (Document CI/CD pipeline and testing process). Epic linked via task list in body. Ready to begin documentation work.


