# Gravity Stellar

A mobile puzzle physics game built with Godot 4 and C#. Players spawn planetary bodies that interact via gravity, forming orbital systems that can merge into larger bodies. Planets leave particle trails. Visual style is minimalist and meditative, inspired by Osmos.

## Getting Started

### Prerequisites

- **Godot 4.6** (or latest stable)
- **.NET 8.0** or later
- **Visual Studio Code** or **JetBrains Rider** (recommended for C# editing)
- **Git**

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/santanor/GravityStellar.git
   cd GravityStellar
   ```

2. Open the project in Godot 4:
   ```bash
   godot
   ```
   Godot will auto-load the project and build the C# runtime.

3. Open scenes from `main.tscn` or explore the `test/` directory to run unit tests.

## Testing

GravityStellar uses **GdUnit4Net**, the C# port of GdUnit4, for all unit tests.

### Running Tests Locally

**Option A: GdUnit4 Test Runner (Godot Editor)**
1. Open Godot
2. Look for the **Test Explorer** panel (usually on the right side)
3. Click **Run All** or run individual test suites
4. Green checkmarks indicate passing tests; red X indicates failures

**Option B: Command Line**
```bash
dotnet test
```
This runs all tests in the `test/` directory via the standard .NET test runner with GdUnit4Net support.

### Test Location

All unit tests live in the `test/` directory at the project root. Test files use the naming convention `*Test.cs` (e.g., `PhysicsModelTest.cs`).

### CI Testing

Every pull request automatically runs tests via GitHub Actions. See [CI/CD Pipeline](#cicd-pipeline) below.

## CI/CD Pipeline

### Overview

Every push to a pull request triggers a **two-stage CI pipeline**:

1. **Test** — GdUnit4Net unit tests run on Linux
2. **Build** — Godot project is exported into a Windows executable

If tests pass, a downloadable Windows build artifact is attached to the PR. You can download and test the build immediately without running the full development environment.

### Pipeline Stages

**Stage 1: Test** (~3–5 min)
- Runs on Ubuntu
- Executes all C# unit tests via GdUnit4Net
- If any test fails, the pipeline stops
- If all tests pass, moves to Stage 2

**Stage 2: Build** (~5–10 min, or ~2–3 min with cache)
- Runs on Ubuntu (Linux)
- Downloads Godot 4.6 (or restores from cache)
- Exports the project to a Windows `.exe` (cross-compilation)
- Caches Godot binary and export templates for faster future builds
- Uploads the `.exe` as a downloadable artifact

### Downloading Build Artifacts

1. Go to the pull request page on GitHub
2. Scroll to the **Checks** section
3. Click the **build** job
4. In the **Artifacts** section, download the Windows build
5. Unzip and run `GravityStellar.exe`

Artifacts are kept for **14 days**.

### Workflow Details

See **[docs/ci-cd.md](docs/ci-cd.md)** for detailed pipeline documentation, including:
- How caching works
- Adding new export presets (Mac, Linux, Web, etc.)
- Troubleshooting common CI failures
- Performance tuning

### Architecture Decisions

The CI/CD pipeline and testing framework choices are documented as **Architecture Decision Records (ADRs)**:

- **[ADR-002: GdUnit4Net Testing Framework](docs/decisions/adr-002-gdunit4net-testing.md)** — Why we chose GdUnit4Net
- **[ADR-003: CI/CD Pipeline with GitHub Actions](docs/decisions/adr-003-cicd-pipeline.md)** — Why we use GitHub Actions, godot-export, and caching

## Project Structure

```
GravityStellar/
├── .github/
│   ├── workflows/           # GitHub Actions CI/CD
│   └── copilot-review-instructions.md
├── docs/                    # Project documentation
│   ├── architecture.md
│   ├── gameplay-systems.md
│   ├── physics-model.md
│   ├── ci-cd.md            # CI/CD pipeline docs
│   └── decisions/          # Architecture Decision Records
│       ├── adr-002-gdunit4net-testing.md
│       └── adr-003-cicd-pipeline.md
├── src/                     # C# game code
├── test/                    # C# unit tests (GdUnit4Net)
├── GravityStellar.csproj    # C# project file
├── project.godot            # Godot project config
└── main.tscn                # Main game scene
```

## Development Workflow

1. Create a feature branch: `git checkout -b feature/my-feature`
2. Make your changes and commit: `git add . && git commit -m "description"`
3. Push to GitHub: `git push origin feature/my-feature`
4. Open a pull request against `master`
5. CI pipeline runs automatically; tests and build artifacts appear in the PR
6. After review and approval, merge into `master`

**PR Guidelines:**
- Keep PRs small (<150 lines, <5 files) for faster reviews
- All tests must pass before merging
- Include relevant documentation updates
- See [.github/copilot-review-instructions.md](.github/copilot-review-instructions.md) for review standards

## Documentation

All system design decisions and architecture docs live in the `docs/` directory:

- **[docs/architecture.md](docs/architecture.md)** — System architecture overview
- **[docs/gameplay-systems.md](docs/gameplay-systems.md)** — Gameplay mechanics and systems
- **[docs/physics-model.md](docs/physics-model.md)** — Physics simulation details
- **[docs/ci-cd.md](docs/ci-cd.md)** — CI/CD pipeline documentation
- **[docs/decisions/](docs/decisions/)** — Architecture Decision Records (ADRs)

## Contributing

Contributions welcome! Please:
1. Write tests for new features
2. Follow C# idioms and Godot best practices
3. Keep commits small and focused
4. Update documentation as needed
5. Ensure all tests pass before submitting a PR

See [.github/copilot-review-instructions.md](.github/copilot-review-instructions.md) for detailed contribution and review guidelines.

## License

TODO — Add license information

## Contact

**Project Owner:** Jose