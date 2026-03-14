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

### CI Export Reliability Strategy

**Author:** Ripley (Tech Lead)  
**Date:** 2026-03-14  
**Status:** Enacted  
**Branch:** `fix/ci-windows-export`

**Context:** The CI Windows export step was failing with signal 11 (SIGSEGV) during Godot shutdown. Investigation revealed two issues:
1. **Path mismatch:** `export_presets.cfg` had `Build/GravityStellar.exe` but CI upload expected `build/windows/`. Linux filesystem is case-sensitive, so `Build/` ≠ `build/`.
2. **Godot shutdown crash:** Godot 4.x on headless Linux runners consistently crashes with signal 11 during shutdown AFTER the export completes successfully. This is a known issue with Godot on CI runners, not an actual export failure.

**Decision:**
1. Fix path consistency: Change export_path to `build/windows/GravityStellar.exe` to match CI upload expectations.
2. Separate export success from process exit: Add `continue-on-error: true` to the export step, then add a verification step that checks if the exe exists. If the exe is present, the export succeeded despite the crash.
3. Explicit failure on true errors: The verification step fails with a clear message if the exe is missing, distinguishing real failures from benign shutdown crashes.

**Consequences:**
- CI builds will pass despite signal 11 crashes, as long as the export artifacts exist
- Clear separation between "export succeeded" (files present) and "process exited cleanly" (exit code 0)
- Future maintainers understand signal 11 is expected behavior, not a regression
- If Godot fixes the shutdown crash in future versions, the continue-on-error can be removed

**Files Changed:**
- `export_presets.cfg` — export_path corrected
- `.github/workflows/ci.yml` — continue-on-error + verification step added

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
