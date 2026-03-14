# ADR-003: CI/CD Pipeline with GitHub Actions

**Date:** 2026-03  
**Status:** Accepted  
**Author:** Dallas (Documentation Engineer)

## Context

Every pull request to the GravityStellar repository needs automated validation to ensure:
1. Code changes don't break tests
2. The game can still be built into executable builds
3. Build artifacts are available for testing/preview

We needed a CI/CD solution that integrates with GitHub, supports Godot 4, and requires minimal maintenance.

## Decision

**Implement a GitHub Actions CI pipeline with two primary jobs:**
1. **test** — Runs unit tests via GdUnit4-action
2. **build** — Builds export presets via godot-export action

**Supporting tools:**
- `actions/cache` — Cache Godot binary and export templates to speed up builds
- `actions/upload-artifact` — Attach build artifacts (Windows executable) to PR

## Rationale

### GitHub Actions
- Native to GitHub; no external infrastructure
- Free for public repositories
- Integrates directly with PR status checks
- Scheduled workflows supported for future nightly builds

### GdUnit4-action
- Purpose-built for GdUnit4 / GdUnit4Net
- Runs C# unit tests in Godot context
- Provides clear failure reporting
- Maintained alongside GdUnit4Net

### godot-export
- Exports Godot 4 projects via export presets
- Supports multiple platforms (Windows, Mac, Linux, Web)
- Works with C# projects (.NET framework)
- Caches Godot binary for faster builds

### Caching Strategy
- **Godot Binary:** Cache Godot executable to avoid 500MB+ downloads per run
- **Export Templates:** Cache platform-specific export templates
- Cache hit rate: ~90% for builds within same job, significantly reducing runtime

### Artifact Upload
- Developers and QA can download built artifacts from PR checks
- No manual build steps needed for preview testing
- Artifacts retain for 14 days

## Consequences

### Benefits
- **Every PR is automatically tested** — immediate feedback to developers
- **Every PR has a downloadable Windows build** — testers can validate gameplay without building locally
- **Faster builds via caching** — 2nd+ build of same commit takes ~2 min vs ~8 min uncached
- **No manual CI/CD maintenance** — YAML workflow is source-controlled and self-documenting

### Constraints
- **Workflow files must be committed to `.github/workflows/`** — any change requires PR
- **GitHub Actions concurrency limits** — free tier has reasonable limits but may queue during high activity
- **Godot export presets must be defined in `export_presets.cfg`** — export settings live in project, not CI
- **Platform-specific secrets** — signing, code signing certs would require GitHub Secrets setup

## Implementation Details

**Workflow Structure:**
```
On: pull_request to master
├─ test (runs on ubuntu-latest)
│  └─ GdUnit4-action (runs GdUnit4Net tests)
├─ build (runs on ubuntu-latest, depends on test)
│  ├─ actions/cache (restore Godot + templates)
│  ├─ godot-export (build Windows preset)
│  └─ actions/upload-artifact (attach .exe to PR)
└─ cleanup (optional, runs after all jobs)
```

**Cache Keys:**
- Godot binary: keyed by Godot version (e.g., `godot-v4.6-windows.zip`)
- Export templates: keyed by Godot version + platform (e.g., `godot-v4.6-export-templates-windows`)

**Artifact Upload:**
- Path: `build/windows/GravityStellar.exe`
- Retention: 14 days
- Visible in: PR → Checks → build → Artifacts

## See Also

- [ADR-002: GdUnit4Net Testing Framework](adr-002-gdunit4net-testing.md) — testing framework used in CI
- [../ci-cd.md](../ci-cd.md) — detailed CI/CD pipeline documentation with troubleshooting
- GitHub Actions Docs: https://docs.github.com/en/actions
