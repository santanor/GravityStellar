# GitHub CLI Authentication in Copilot Sessions

## Summary
When running inside Copilot CLI, `GH_TOKEN` is set with limited scopes. Unset it before `gh` write operations.

## Confidence: high

## Pattern

### Problem
Copilot CLI sets `GH_TOKEN` with only `user:email` scope. This overrides the user's full-access keyring token, causing `gh pr create` and other write operations to fail with "insufficient scopes."

### Solution
Before any `gh` write operation (pr create, issue create, label create, etc.), unset `GH_TOKEN`:

**PowerShell:**
```powershell
$env:GH_TOKEN = $null
gh pr create --title "..." --body "..." --base master --head my-branch
```

**Bash:**
```bash
unset GH_TOKEN
gh pr create --title "..." --body "..." --base master --head my-branch
```

### When to Apply
- Creating PRs (`gh pr create`)
- Creating issues (`gh issue create`)
- Adding labels (`gh label create`)
- Any `gh` command that writes to GitHub

### When NOT Needed
- Reading PRs/issues (`gh pr list`, `gh issue list`) — these work with limited scope
- Git push/pull — uses git protocol auth, not `GH_TOKEN`

### MCP Server Note
GitHub MCP server tools (`github-mcp-server-*`) inherit the limited `GH_TOKEN`. For write operations, always prefer `gh` CLI with `GH_TOKEN` unset over MCP tools.

## Origin
Discovered 2026-03-14. Confirmed by testing both tokens. The keyring token has full `repo` scope; the Copilot-injected `GH_TOKEN` has only `user:email`.
