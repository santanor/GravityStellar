# Squad Team

> gravity-stellar — Mobile puzzle physics game in Godot + C#

## Coordinator

| Name | Role | Notes |
|------|------|-------|
| Squad | Coordinator | Routes work, enforces handoffs and reviewer gates. |

## Members

| Name | Role | Charter | Status |
|------|------|---------|--------|
| Ripley | Tech Lead / Architect | `.squad/agents/ripley/charter.md` | 🏗️ Lead |
| Kane | Gameplay Programmer | `.squad/agents/kane/charter.md` | 🎮 Active |
| Lambert | Physics Programmer | `.squad/agents/lambert/charter.md` | ⚛️ Active |
| Parker | Game Feel / UX Engineer | `.squad/agents/parker/charter.md` | 🎨 Active |
| Brett | QA / Test Engineer | `.squad/agents/brett/charter.md` | 🧪 Active |
| Dallas | Documentation Engineer | `.squad/agents/dallas/charter.md` | 📝 Active |
| Scribe | Session Logger | `.squad/agents/scribe/charter.md` | 📋 Silent |
| Ralph | Work Monitor | — | 🔄 Monitor |

## Project Context

- **Owner:** Jose
- **Project:** Gravity Stellar — a mobile puzzle physics game built with Godot (latest stable) using C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.
- **Stack:** Godot 4 (latest stable), C#, .NET
- **Created:** 2026-03-13

## Architecture Principles

- Extremely small PRs (<150 lines, <5 files, one concern)
- GitHub-first workflow (issues → PRs → review → merge)
- Physics logic separate from rendering separate from gameplay
- Composition over inheritance
- Signals for decoupling
- No god classes
- Keep physics logic deterministic where possible
