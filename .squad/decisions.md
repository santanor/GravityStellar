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

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
