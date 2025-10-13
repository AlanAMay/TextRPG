# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TextRPGOne is a console-based text RPG game built with C# and .NET 8.0. It features turn-based combat, location exploration, character classes, and ASCII art rendering using Spectre.Console for rich terminal UI.

## Build and Run Commands

```bash
# Build the project
dotnet build

# Run the game
dotnet run

# Clean build artifacts
dotnet clean
```

## Architecture

### Partial Class Structure
The entire application uses a single `partial class Program` pattern, with functionality split across multiple files:
- `Program.cs` - Main entry point, game loop, combat system, menu handling
- `PlayerCharacter.cs` - Player character class (nested within Program)
- `NPC.cs` - Enemy/NPC class and enemy definitions
- `Location.cs` - Location class and location graph initialization
- `Move.cs` - Combat move/ability class
- `CharacterSpec.cs` - Character class templates (Warrior, Rogue, Mage)
- `Item.cs` - Item system (stub implementation)
- `Visuals.cs` - Spectre.Console UI rendering (status bars, combat display)
- `ImageHandler.cs` - ASCII art display via external `ascii-image-converter` tool

### Key Design Patterns

**Location Graph**: Locations are connected via `ConnectedLocations` lists, forming a traversable graph initialized in `InitializeLocations()` in Location.cs:90-107. Players travel between connected locations.

**Enemy Encounters**: Each location has a `PossibleEnemies` list. During exploration, enemies are randomly selected and cloned (NPC.cs:43-46) to create combat instances with independent state.

**Combat System**: Turn-based combat with:
- Initiative rolls at combat start (Program.cs:58-70) determine turn order for entire battle
- Damage calculation: `(PrimaryStat + Move.Damage) / 2 + random(-3, 4)` for players (Program.cs:113-114)
- Players select moves via Spectre.Console selection prompt (Visuals.cs:108-134)
- Mana costs enforced before move execution (Program.cs:107-110)

**Primary Stat System**: Each character/NPC has one primary stat (Strength/Dexterity/Intelligence) used for damage calculations. Set in character creation (Program.cs:384-393) and NPC constructor (NPC.cs:63-68).

### External Dependencies

**ascii-image-converter**: Required external CLI tool for rendering location images as ASCII art. Called via `Process.Start()` in ImageHandler.cs:11. Images stored in `images/` directory with threshold parameters in Location definitions (Location.cs:46-88).

**Spectre.Console**: Used extensively for:
- Rich status bars with health/mana visualization (Visuals.cs:8-107)
- Selection prompts for combat moves (Visuals.cs:127-132)
- Colored markup text rendering throughout UI
- Loading animations via `AnsiConsole.Status()` (Program.cs:52-56)

### Notable Implementation Details

**PlayerCharacter Access**: The game uses a static nullable `playerCharacter` variable (PlayerCharacter.cs:53). Many methods check for null before accessing, since the character is only created after selecting "Play Game".

**Move Display Format**: Combat moves with mana costs display as `[ManaCost] MoveName` in the UI (Visuals.cs:114-118), requiring string parsing to extract the actual move name (Program.cs:98-104).

**Safe Zones**: Locations can be marked as `IsSafeZone` (Location.cs:14) - currently CrossRoad and Town are safe zones where no combat occurs.

## Important Notes

- The PlayerCharacter constructor is private (PlayerCharacter.cs:36) - instantiate only through character creation flow
- All classes are nested within the `Program` partial class, so fully qualified names aren't needed within the namespace
- Initiative is calculated once per combat encounter, not per turn
- The inventory system is stubbed (Item.cs:20-23) but not implemented
