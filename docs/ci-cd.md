# CI/CD Pipeline Documentation

This document explains how the GravityStellar CI/CD pipeline works, what happens on pull requests, and how to troubleshoot common issues.

## Overview

Every pull request to the `master` branch triggers a GitHub Actions workflow that:
1. **Tests** — Runs GdUnit4Net unit tests to validate code
2. **Builds** — Exports the Godot project into a Windows executable
3. **Artifacts** — Uploads the executable so you can download and test it

The workflow is defined in `.github/workflows/ci.yml` (or similar).

## Pipeline Stages

### Stage 1: Test

**Job:** `test`  
**Runs on:** Ubuntu (Linux) VM  
**Time:** ~3–5 minutes

**What happens:**
1. GitHub checks out your PR branch
2. Godot 4 and .NET 8.0 are installed
3. GdUnit4-action runs all C# unit tests via GdUnit4Net
4. If tests fail, the entire workflow stops
5. If tests pass, the workflow continues to the build stage

**Test Location:** All tests live in the `test/` directory.

**Failure Reason:** Tests are failing. Check:
- Local test run: `dotnet test` or use the GdUnit4 runner in the Godot editor
- PR diff: Did you add a breaking change to the physics or gameplay API?
- Dependencies: Did you add a new NuGet package that conflicts?

### Stage 2: Build

**Job:** `build`  
**Runs on:** Ubuntu (Linux) VM  
**Time:** ~5–10 minutes (first run), ~2–3 minutes (cached)  
**Depends on:** `test` job must pass first

**What happens:**
1. GitHub checks out your PR branch on an Ubuntu runner
2. Cache layer attempts to restore cached Godot binary and export templates
   - If cache hit: Godot is already downloaded (~2 MB instead of 500 MB)
   - If cache miss: Godot binary and templates are downloaded and cached for future runs
3. godot-export action exports the project using `export_presets.cfg`
4. Windows executable is built to `build/windows/GravityStellar.exe`
5. `actions/upload-artifact` uploads the `.exe` so you can download it from the PR

**Caching:**
- Godot binary (e.g., `godot-v4.6-windows.zip`) is cached for 7 days
- Export templates are cached for 7 days
- Cache keys are based on Godot version, so upgrading Godot will auto-invalidate old cache

**Failure Reason:** Build is failing. Check:
- Export presets: Is `export_presets.cfg` valid? (Can you export locally?)
- New files: Did you add a `.cs` file that doesn't compile? (Check "test" job logs)
- Missing assets: Did you add scenes/resources that aren't checked in?
- Dependency versions: Does your `GravityStellar.csproj` have version conflicts?

## Downloading Artifacts

1. **On the PR page:** Go to the PR, scroll to "Checks" section
2. **Find the build job:** Click on the `build` job
3. **Artifacts section:** Scroll down, you'll see "Artifacts" with a download icon
4. **Download:** Click the artifact name (e.g., `windows-build`) to download the `.zip`
5. **Extract & Run:** Unzip and run `GravityStellar.exe`

Artifacts are retained for **14 days**. You can adjust this in the workflow file if needed.

## Local Testing vs CI

### Running Tests Locally

**Option A: GdUnit4 Test Runner (Godot Editor)**
1. Open the Godot editor
2. Open the Test Explorer panel (usually on the right)
3. Run all tests or individual test suites
4. Green checkmark = pass, red X = fail

**Option B: Command Line**
```bash
dotnet test
```
This runs all tests in `test/` using the standard .NET test runner with GdUnit4Net.

### Running Builds Locally

**Option A: Godot Editor**
1. Open Godot
2. Project → Export → Select "Windows Desktop" preset
3. Click "Export Project"
4. Choose output directory
5. Built `.exe` will be created

**Option B: Command Line (with Godot CLI)**
```bash
godot --export-release "Windows Desktop" "path/to/output.exe"
```
(Requires Godot CLI installed and in PATH)

## Adding New Export Presets

To export a new platform (Mac, Linux, Web, Android, etc.):

1. **In Godot Editor:**
   - Project → Export
   - Click "Add Preset"
   - Choose platform (e.g., "macOS")
   - Configure settings (icon, resources, permissions)
   - Save

2. **In CI Workflow:**
   - Edit `.github/workflows/ci.yml`
   - Add a new build job (e.g., `build-macos`) that mirrors the Windows build job
   - Update `godot-export` action to export for the new platform
   - Commit and push

3. **Example:** To add a Mac build:
   ```yaml
   build-macos:
     needs: test
     runs-on: macos-latest  # Requires macOS runner
     steps:
       - uses: actions/checkout@v4
       - uses: firebelley/godot-export@v6
         with:
            godot_executable_download_url: https://github.com/godotengine/godot/releases/download/4.6-stable/Godot_v4.6-stable_mono_macos.universal.zip
            export_template_download_url: https://github.com/godotengine/godot/releases/download/4.6-stable/Godot_v4.6-stable_mono_export_templates.tpz
            relative_project_path: ./
   ```

## Troubleshooting

### "Tests are failing but pass locally"

**Possible causes:**
- Different OS behavior (Linux runner vs Windows dev machine)
- Different .NET versions
- Relative path issues (use absolute paths in tests)
- Race conditions or timing issues

**Solution:**
1. Check the exact error in the PR workflow logs
2. Reproduce on your local Linux VM (or use Docker)
3. Look for OS-specific code paths in the failing test

### "Build artifact is huge" (>500 MB)

**Possible cause:** You included uncompressed assets, debug symbols, or the entire Godot install in the artifact.

**Solution:**
- Check `export_presets.cfg` → "General" tab → ensure "Debug" is OFF
- Ensure export optimizations are enabled
- Artifact size should be 50–150 MB for a typical Godot game; if larger, investigate what's being packed

### "Workflow is slow (taking >15 min)"

**Possible cause:** Cache miss (first run) or GitHub runner queue is busy.

**Solution:**
- First run always takes ~10 min due to Godot download. Subsequent runs hit cache.
- If queued for >5 min, GitHub Actions infrastructure might be under load. Try re-running or check GitHub status page.

### "Secret or credential failed"

**Possible cause:** If you added code signing or authentication, you're missing GitHub Secrets.

**Solution:**
- For now, GravityStellar only builds unsigned executables
- To add signing in future, add secrets to your GitHub repository settings and reference them in the workflow

### "Artifact upload failed"

**Possible cause:** Export path doesn't exist or permissions issue.

**Solution:**
- Check that godot-export actually created the file
- Verify the `export_path` in the workflow matches the path where the executable lands
- Check workflow logs for godot-export errors

## Performance Tuning

### Cache Warming

The cache is automatically warmed on the first run. Subsequent runs reuse cached binaries. To manually clear cache:
- GitHub Settings → Actions → Caches → Select and delete cache entries
- Next workflow run will rebuild cache

### Parallel Jobs

If you add more build jobs (e.g., for multiple platforms), GitHub can run them in parallel. They all depend on `test`, but not on each other:
```yaml
build-windows:
  needs: test
  ...

build-macos:
  needs: test
  ...

build-linux:
  needs: test
  ...
```
All three build jobs run in parallel, each taking ~5–10 min. Total pipeline time = test time + max build time, not sum of all builds.

## References

- **ADR-002:** [GdUnit4Net Testing Framework](decisions/adr-002-gdunit4net-testing.md)
- **ADR-003:** [CI/CD Pipeline Decision](decisions/adr-003-cicd-pipeline.md)
- **Workflow File:** `.github/workflows/ci.yml` (source of truth for actual steps)
- **GdUnit4 Docs:** https://github.com/MikeSchulze/gdUnit4
- **Godot Export:** https://docs.godotengine.org/en/stable/tutorials/export/exporting_projects.html
- **GitHub Actions:** https://docs.github.com/en/actions
