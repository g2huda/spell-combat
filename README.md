# Spell Combat (prototype)

A lightweight C#/.NET 8 prototype for spell-based combat. The repository contains gameplay systems, UI, characters, and effects implemented as C# scripts under `Scripts/` for rapid iteration.

## Features

- Player spell casting and ability system that can switch spells
- Enemy spawning and simple AI
- UI for health, spells, and win popup
- Visual/audio effects for hits

## What this demonstrates

- Event-driven gameplay architecture
- Modular ability system design (extensible spell switching)
- Clean separation of systems (UI, gameplay, effects)
- Rapid prototyping workflows using C#/.NET

## Requirements

- .NET 8 SDK
- Visual Studio 2022 (recommended) or any editor that supports .NET 8
- Windows/macOS/Linux supported by .NET 8

## Setup

1. Clone the repository:

   git clone https://github.com/g2huda/spell-combat.git
   cd spell-combat

2. Restore and build (if a solution/project exists at the repo root):

   dotnet restore
   dotnet build

3. Open the solution in Visual Studio 2022 and press F5 to run, or use `dotnet run` from the appropriate project directory.

## Controls / Gameplay

- Left/Right movement using Left/Right arrows or A/D keyboard keys
- Forward/Backward movement using Up/Down arrows or W/S keyboard keys
- Cast spells using Space bar
- Switch spells using X
- Defeat enemies spawned by the `EnemySpawner` to progress.
- UI elements such as `GameUi` and `WinPopup` present game state and win conditions.

## Project structure (important folders)

- `Scripts/Systems` - core game systems (ability system, game controller, spawner)
- `Scripts/Characters` - character and enemy logic
- `Scripts/UI` - user interface components
- `Scripts/Effects` - visual/audio effect scripts

## Architecture Notes

The project follows a modular architecture where gameplay systems are decoupled and communicate through events, allowing for flexible extension of abilities, UI, and effects without tight coupling.

## Future Improvements

- More advanced enemy AI (state machines / behavior trees)
- Networked Co-op multiplayer support
