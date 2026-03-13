# Project Decisions

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
