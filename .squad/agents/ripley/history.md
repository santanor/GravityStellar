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
