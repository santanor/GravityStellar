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

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
