# Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Key Responsibilities

- Planet spawning mechanics and player input
- Planet merging/absorption systems
- Star collection and level objectives
- Level progression logic

## Learnings

- **Repo:** https://github.com/santanor/GravityStellar — remote wiped clean on 2026-03-13, all old branches deleted, master reset to orphan commit.
- **Branch strategy:** `master` is the stable trunk. All work goes through `feature/*` branches with small PRs. No `develop` branch — we keep it simple.
- **PR #10** (`feature/godot-csharp-fresh-start`) is the foundational PR bringing in the Godot 4 C# project, squad state, CI workflows, and editor config.
- Force-pushing to master requires explicit owner approval (Jose authorized this one). Don't do it again without asking.
- **PR Review Standards:** Created `.github/copilot-review-instructions.md` (2026-03-13T22:37:47Z). Reviews enforce: small PRs (<150 lines, <5 files), Godot best practices, C# idioms, physics safety, scene structure, documentation, debug tooling. Concise, constructive, specific feedback only.
- **PR #103** (`feature/demo-gravity-scene`): Created gravity demo scene replacing Hello World. Spawns 5 planets (Sun + 4 orbiters) via SimulationManager.AddBody() in DemoScene._Ready(). Registered SimulationManager as autoload in project.godot. Scene structure: Node2D root (DemoScene.cs) → PlanetSyncManager (Node) + Camera2D. Deleted Hello.cs, Node2d.cs, main.tscn.
- **Orbital velocity formula:** For roughly circular orbits with G=6.674: v = sqrt(G * M_central / distance). Example: sqrt(6.674 * 5000 / 200) ≈ 12.9. Tangential velocity must be perpendicular to the line between the orbiter and central mass.
- **Autoload registration:** SimulationManager must be registered as autoload in project.godot (`SimulationManager="*res://Scripts/Physics/SimulationManager.cs"`) for PlanetSyncManager to find it at `/root/SimulationManager`. The `*` prefix enables the autoload.
