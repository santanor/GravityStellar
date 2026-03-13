# Work Routing

How to decide who handles what.

## Routing Table

| Work Type | Route To | Examples |
|-----------|----------|----------|
| Architecture, system design, task decomposition | Ripley | System boundaries, PR review, design decisions |
| Gameplay features, spawning, merging, objectives | Kane | Planet spawning, star collection, level progression |
| Gravity, physics, orbital mechanics, simulation | Lambert | Gravity calculations, orbit stability, collision physics |
| Visual effects, particles, game feel, aesthetics | Parker | Particle trails, visual feedback, motion readability |
| Testing, QA, edge cases, regression | Brett | Test scenes, system validation, determinism checks |
| Documentation, ADRs, architecture notes | Dallas | /docs/ files, decision records, README |
| Code review | Ripley | Review PRs, check quality, enforce small PR discipline |
| Testing | Brett | Write tests, find edge cases, verify fixes |
| Scope & priorities | Ripley | What to build next, trade-offs, decisions |
| Async issue work (bugs, tests, small features) | Appropriate specialist | Route based on domain — Kane for gameplay, Lambert for physics, etc. |
| Session logging | Scribe | Automatic — never needs routing |

## Issue Routing

| Label | Action | Who |
|-------|--------|-----|
| `squad` | Triage: analyze issue, assign `squad:{member}` label | Ripley |
| `squad:ripley` | Architecture, review, decomposition | Ripley |
| `squad:kane` | Gameplay features, spawning, merging | Kane |
| `squad:lambert` | Physics, gravity, orbital mechanics | Lambert |
| `squad:parker` | Visual effects, particles, game feel | Parker |
| `squad:brett` | Testing, QA, edge cases | Brett |
| `squad:dallas` | Documentation, ADRs | Dallas |

### How Issue Assignment Works

1. When a GitHub issue gets the `squad` label, **Ripley** triages it — analyzing content, assigning the right `squad:{member}` label, and commenting with triage notes.
2. When a `squad:{member}` label is applied, that member picks up the issue in their next session.
3. Members can reassign by removing their label and adding another member's label.
4. The `squad` label is the "inbox" — untriaged issues waiting for Ripley's review.

### Lead Triage Guidance

When triaging, Ripley should ask:

1. **Is this well-defined?** Clear title, reproduction steps or acceptance criteria, bounded scope
2. **Which system does it touch?** Route to the specialist: physics → Lambert, gameplay → Kane, visuals → Parker
3. **Does it need tests?** Tag Brett for test coverage alongside the implementer
4. **Does it need documentation?** Tag Dallas for ADR or docs update
5. **Is it too big?** If the issue would result in >150 lines changed, decompose into sub-issues first

## Rules

1. **Eager by default** — spawn all agents who could usefully start work, including anticipatory downstream work.
2. **Scribe always runs** after substantial work, always as `mode: "background"`. Never blocks.
3. **Quick facts → coordinator answers directly.** Don't spawn an agent for "what port does the server run on?"
4. **When two agents could handle it**, pick the one whose domain is the primary concern.
5. **"Team, ..." → fan-out.** Spawn all relevant agents in parallel as `mode: "background"`.
6. **Anticipate downstream work.** If a feature is being built, spawn the tester to write test cases from requirements simultaneously.
7. **Issue-labeled work** — when a `squad:{member}` label is applied to an issue, route to that member. Ripley handles all `squad` (base label) triage.
