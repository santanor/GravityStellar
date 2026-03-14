# Project Decisions

## AI Ops & DevOps Epic — 2026-03-14

**Author:** Ripley (Tech Lead / Architect)  
**Status:** Approved  
**Epic:** Internal Tooling, Frameworks, and Testing

### Context

Gravity Stellar has a clean Godot 4 C# codebase with squad workflows but zero CI for code quality or builds. Before gameplay gets complex, we need automated testing, build pipelines, and developer tooling in place.

### Decision

Stand up a complete CI/CD pipeline and testing framework with 10 issues across 5 priority tiers:

**Priority 1 — Foundation:**
- Set up GdUnit4Net testing framework (Brett) ✅
- Create export_presets.cfg for Windows Desktop (Ripley) ✅

**Priority 2 — Pipeline:**
- Create CI/CD pipeline — GitHub Actions workflow (Ripley) ✅
- Add Godot + export template caching to CI (Ripley) ✅
- Add PR artifact upload for Windows builds (Ripley) ✅

**Priority 3 — Developer Experience:**
- Create copilot-instructions.md with GdUnit4Net guidelines (Ripley) ✅
- Document CI/CD pipeline and testing process (Brett/Ripley) ✅

**Priority 4 — Enforcement:**
- Add branch protection rules requiring CI to pass (Jose) — pending owner action

**Priority 5 — Future:**
- Add mobile export presets (Android/iOS)
- Add automated release workflow on tag push

### Artifacts

- `.github/workflows/ci.yml` — Two-job pipeline (test via GdUnit4-action, build via godot-export)
- `export_presets.cfg` — Windows Desktop preset, export path: `build/windows/GravityStellar.exe`
- `.github/copilot-instructions.md` — Testing conventions, local test running, CI overview
- `test/` directory — GdUnit4Net structure with examples
- `.gdunit4.json` — Test runner configuration

### Consequence

Every PR gets automated testing before merge. Reviewers can download and play Windows builds from PRs. Testing conventions are documented. Foundation is in place for mobile exports and release automation.

---

## AI Ops & DevOps GitHub Issues Created — 2026-03-14

**Author:** Ripley (Tech Lead)  
**Status:** Enacted

### Context

The AI Ops & DevOps epic was previously planned and decomposed into 10 issues across 5 priority tiers (see above). The actual GitHub issues needed to be created on the `santanor/GravityStellar` repository with proper labels, descriptions, acceptance criteria, and squad assignments.

### Decision

Created Epic #72 and 10 sub-issues (#73–#82) with full descriptions, acceptance criteria, and dependency references:

| # | Title | Priority | Squad |
|---|-------|----------|-------|
| 72 | [Epic] AI Ops & DevOps | — | — |
| 73 | Set up GdUnit4Net testing framework | P1 | Brett |
| 74 | Create export_presets.cfg for Windows Desktop | P1 | Ripley |
| 75 | Create CI/CD pipeline — GitHub Actions workflow | P2 | Ripley |
| 76 | Add Godot + export template caching to CI | P2 | Ripley |
| 77 | Add PR artifact upload for Windows builds | P2 | Ripley |
| 78 | Create copilot-instructions.md with GdUnit4Net testing guidelines | P3 | Ripley |
| 79 | Document CI/CD pipeline and testing process | P3 | Dallas |
| 80 | Add branch protection rules requiring CI to pass | P4 | Ripley |
| 81 | Add mobile export presets (Android/iOS) | P5 | — |
| 82 | Add automated release workflow on tag push | P5 | — |

### Labels Created

15 labels created with `--force` (idempotent): `devops`, `testing`, `ci-cd`, `documentation`, `dx`, `priority:high`, `priority:medium`, `future`, `epic`, `build`, `governance`, `squad`, `squad:ripley`, `squad:brett`, `squad:dallas`.

### Notes

- `gh` CLI v2.87.2 does not support `--add-sub-issue`, so sub-issues are linked via a task list in the Epic body.
- P5 issues (#81, #82) have no squad assignment — they're future work.
- Issue #80 (branch protection) requires Jose's admin access.

### Consequence

Epic and all sub-issues now trackable on GitHub. Squad members can filter issues by assignee label. Dependency graph visible in Epic task list. Team can begin implementation work.

---

## GdUnit4Net as Testing Framework — 2026-03-14

**Author:** Brett (QA/Test Engineer)  
**Status:** Approved

### Context

Project lacked automated testing infrastructure. Jose requested setup of GdUnit4Net to enable unit, integration, and scene testing.

### Decision

Adopted **GdUnit4Net 5.0.0** as the primary testing framework:

**NuGet Packages:**
- `gdUnit4.api` 5.0.0 — Core test framework
- `gdUnit4.test.adapter` 3.0.0 — IDE integration
- `Microsoft.NET.Test.Sdk` 18.3.0 — Test infrastructure

**Conventions:**
- Test files: `*Test.cs`
- Test classes: `[TestSuite]` attribute
- Test methods: `[TestCase]` attribute
- Namespace: `GravityStellar.Tests.<System>`
- Assertions: GdUnit4 `Assertions.AssertThat()` API

### Rationale

Native Godot 4 support, IDE integration, modern API with fluent assertions, compatibility with .NET 8.0/9.0.

### Consequence

Enables TDD workflow, IDE test runners provide fast feedback, clear test structure enforces consistency, can test both logic and scene behaviors.

---

## CI/CD & Testing Documentation — 2026-03-14

**Author:** Dallas (Documentation Engineer)  
**Status:** Approved

### Context

Epic established testing framework and pipeline. Documentation required for team adoption and onboarding.

### Decision

Created comprehensive documentation package:

**ADRs:**
- `docs/decisions/adr-002-gdunit4net-testing.md` — GdUnit4Net decision rationale and consequences
- `docs/decisions/adr-003-cicd-pipeline.md` — GitHub Actions pipeline design and rationale

**Guides:**
- `docs/ci-cd.md` — Pipeline overview, local testing, artifact download, troubleshooting, performance tuning
- `README.md` — Updated with Getting Started, Testing, CI/CD Overview, documentation index

### Consequence

Reduces onboarding friction, documents *why* tools were chosen, provides troubleshooting guides, establishes ADR pattern for future decisions.

---

## PR Review Standards — 2026-03-13T22:37:47Z

**Owner:** User directive (Jose)  
**Source:** Copilot user input via Dallas agent  
**Decision:** Establish comprehensive PR review custom instructions for GitHub Copilot

### Details

Established custom instructions for PR reviews enforcing:
- Small PR philosophy (< 150 lines, < 5 files)
- Godot architecture best practices (small scripts, signals, composition over inheritance)
- Clean C# idioms and conventions
- Physics/gameplay safety considerations
- Scene structure quality and consistency
- Documentation requirements for significant changes
- Debug tooling encouragement for development

Reviews must be concise, constructive, and specific — no vague feedback.

### Artifact

**File:** `.github/copilot-review-instructions.md`  
**Location:** Repository root (standard GitHub path)  
**Status:** Active, team-wide

### Rationale

Codifies user expectations and team review standards. Enables consistent, high-quality PR reviews aligned with gravity-stellar's architecture and physics game requirements.

---

## GitHub CLI Auth Workaround — 2026-03-14T22:53

**By:** Jose (via Copilot)  
**Status:** Active

### Context

All agents must manage GitHub CLI token scope when running write operations inside Copilot sessions.

### Decision

When calling `gh` CLI for write operations (`pr create`, `issue create`, etc.):
1. Unset `GH_TOKEN` environment variable before the command
2. This allows `gh` to fall back to keyring token with full `repo` scope
3. Copilot-injected token lacks `repo` scope and causes silent failures

### Rationale

The MCP server injects a limited token into the environment. This token does not have write permissions. By unsetting it, `gh` CLI uses the authenticated keyring entry, which has full repository access.

### Consequence

All agents can reliably perform GitHub write operations. This pattern applies to any tooling that depends on GitHub authentication within Copilot sessions.

---

## GdUnit4 GDScript Addon Update: v6.0.0 → v6.1.1 — 2026-03-14

**Author:** Ripley (Tech Lead)  
**Status:** Enacted  
**Branch:** `fix/ci-windows-export`

### Context

The CI pipeline was crashing with signal 11 (SIGSEGV) during Godot export. Previous fixes addressed path mismatches and added `continue-on-error` as a workaround, but the **root cause** was the GdUnit4 GDScript addon (v6.0.0) being incompatible with Godot 4.6.

Specifically, `addons/gdUnit4/src/core/GdUnitFileAccess.gd:199` called `file.get_as_text(true)`, but Godot 4.6 removed the `skip_cr` parameter from `FileAccess.get_as_text()`. This caused cascading GDScript compilation failures when Godot loaded, leading to the crash.

### Decision

Replace the entire GdUnit4 GDScript addon directory (`addons/gdUnit4/`) with v6.1.1, which has full Godot 4.6 compatibility. Downloaded from the official GitHub release (`MikeSchulze/gdUnit4` tag v6.1.1).

### Consequence

- CI export should no longer crash due to GDScript compilation errors
- The `continue-on-error` workaround on the export step may now be removable (test first)
- 317 files changed — this is a vendor dependency update, not custom code
- Future Godot version upgrades should include checking GdUnit4 compatibility

---

## Exclude Test Sources from ExportRelease Builds — 2026-03-14

**Author:** Ripley (Tech Lead)  
**Status:** Enacted  
**Branch:** `fix/ci-windows-export`  
**PR:** #96

### Context

The `GravityStellar.csproj` conditionally excludes test NuGet packages (GdUnit4, Microsoft.NET.Test.Sdk, etc.) from the `ExportRelease` configuration. However, test `.cs` files under `test/` were still included in compilation. When `dotnet publish -c ExportRelease` ran, it tried to compile test files that referenced GdUnit4 types — but the GdUnit4 package wasn't available, causing CS0246 errors.

### Decision

Add a conditional `<Compile Remove>` in the csproj to exclude test sources from ExportRelease:

```xml
<ItemGroup Condition="'$(Configuration)' == 'ExportRelease'">
    <Compile Remove="test\**\*.cs" />
</ItemGroup>
```

### Rationale

Package exclusion and source exclusion must stay in sync. If a package is conditionally removed, any source files that depend on it must also be conditionally removed — otherwise the build breaks.

### Consequences

- ExportRelease builds no longer fail with missing type errors from test code
- Test files are still compiled in Debug and other configurations (including `dotnet test`)
- Future test files added under `test/` are automatically excluded from ExportRelease via the glob pattern
