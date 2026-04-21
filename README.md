# Bad Dream
### First-Person Horror Game | Unity 6 · C#

>  This project is an early prototype. Core mechanics are implemented; enemy AI and full gameplay loop are in active development.

---

## Overview

Bad Dream is a first-person horror game built in Unity 6. The project focuses on building a strong mechanical foundation — player controls, interactive systems, inventory management, and atmospheric UI — before layering in enemies and narrative content.

---

## Current Features

### Player
- First-person movement with walking, sprinting, and jumping
- Smooth camera control and collision handling

### Interaction & Inventory
- Object pickup and inventory management system
- Interactive doors with key-based progression
- Crank-powered flashlight with battery drain and management

### UI & Game State
- HUD displaying player health and flashlight battery level
- Game state management: main menu, gameplay, and pause screen

---

## In Progress

- [ ] Enemy AI — pathfinding, detection, and chase behavior
- [ ] Expanded level design and environmental storytelling
- [ ] Sound design and atmospheric audio
- [ ] Additional puzzles and gameplay mechanics

---

## Getting Started

### Required Asset Store Packages
The following packages are **not included** in the repository and must be downloaded from the [Unity Asset Store](https://assetstore.unity.com/) before opening the project:

| Package | Publisher |
|---------|-----------|
| Realistic Terrain Textures Lite | ALP Assets |
| Basic Bedroom Pack | Mavi3D |
| House Furniture Pack | (search by name) |
| Cartoon Texture Pack | (search by name) |
| Fantasy Skybox FREE | (search by name) |
| Wallpaper, Carpet and CurtainFabric Materials Pack | Phoenix3D |

After importing all packages, reimport the project if any assets appear pink/purple.

### Requirements
- [Unity 6](https://unity.com/releases/unity-6) (6000.x)

### Running the Project
1. Clone the repository
   ```
   git clone https://github.com/DustinDavidson/bad-dream.git
   ```
2. Open Unity Hub and click **Add project from disk**
3. Select the cloned folder
4. Open the project in Unity 6
5. In the Project window, navigate to `Assets/Scenes` and open the main scene
6. Press **Play** to run

> Note: This is a development build. Some assets and systems may be incomplete.

---

## Tech Stack

| Tool | Purpose |
|------|---------|
| Unity 6 | Game engine |
| C# | Game logic and scripting |
| Unity Input System | Player controls |
| Unity UI Toolkit | HUD and menus |

---

## About

Developed by **Dustin Davidson** as a personal game development project, exploring Unity systems architecture, first-person mechanics, and horror game design.
