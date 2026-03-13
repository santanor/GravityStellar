# Gravity Stellar

## Local Setup

### VS Code — Godot editor path

The `godot-tools` VS Code extension needs to know where your Godot 4 editor binary is. This path is machine-specific, so it is **not** committed to the repository.

Add the following to your local `.vscode/settings.json` (or VS Code user settings), adjusting the path for your OS and Godot version:

```json
{
  "godotTools.editorPath.godot4": "/path/to/Godot_v4.x_mono"
}
```

Examples:
- **Windows:** `C:\\Program Files\\Godot\\Godot_v4.6.1-stable_mono_win64.exe`
- **macOS:** `/Applications/Godot_mono.app/Contents/MacOS/Godot`
- **Linux:** `/usr/local/bin/godot4`