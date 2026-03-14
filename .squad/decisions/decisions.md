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
