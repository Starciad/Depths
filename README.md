# Depths

![NOKIA_3310_JAM_7]

> The main banner of the 7th edition of the Nokia 3310 Jam.

## Introduction

> A mining game made for the Nokia 3310 Jam - 7th Edition.

Depths is a mining game where your ultimate goal is to reach the great idol hidden deep within the cave. To achieve this, you'll need to mine ores, sell resources, and upgrade your tools.

Explore procedurally generated caves, face hidden dangers, and manage your supplies carefully to descend as deep as possible without losing the game.

## How to Play

You can play Depths directly in your browser via the game's page on Itch.io. Alternatively, you can download the desktop version to play offline on your computer.

The WebGL version is experimental and may present issues that do not occur in the desktop version. For the best experience, it is recommended to play the desktop version.

The WebGL build is provided for convenience, allowing players to quickly try the game without downloading, and to make it easier for Game Jam judges to access and evaluate the entry.

Play Depths on Itch.io: <https://starciad.itch.io/depths>.

## Nokia Jam

The Nokia 3310 Jam - 7th Edition can be accessed at the following link: [Itch.io Nokia Jam 7](https://itch.io/jam/nokiajam7).

This competition has some unique restrictions, such as using only two colors, a low frame rate, an 84x48 pixel resolution, a single sound channel, and more.

This game was developed specifically for the jam, following all the required limitations and rules. The theme for the 7th edition was **THE GRIND**.

## About the Game

In Depths, you must use strategy to navigate the environment, managing resources like ladders, platforms, and mining robots. Since you can't jump, every movement decision matters and can determine your success or failure.

As you descend, the challenges increase: rocks become tougher, valuable ores require stronger tools, and new dangers emerge. Plan your upgrades wisely and gather the right resources to survive long enough to reach the idol.

## Controls

| KEYS (NORMAL) | KEYS (NUM PAD) | ACTION                                    |
| ------------- | -------------- | ----------------------------------------- |
| W, A, S and D | 8, 4, 2 and 6  | Move character, navigate menus, and mine. |
| K             | 7              | Place a ladder.                           |
| J             | 9              | Place a platform.                         |
| H             | 1              | Deploy a mining robot.                    |
| ENTER         | 5              | Confirm action / buy items (menus).       |
| ESC           | 3              | Cancel action / go back (menus).          |
| Q             | 0              | Access player info.                       |
| E             | Decimal        | Open the shop (only on the surface).      |

## Requirements

Here are the necessary requirements to run the game properly:

- **Operating System**: Windows 10 or higher
- **DirectX**: Version 9.0c or higher
- **RAM**: At least 100 MB
- **Disk Space**: Minimum of 100 MB available
- **System Architecture**: x64 (64-bit)

## Features

- Explore and mine procedurally generated caves full of challenges.
- Use ladders, platforms, and mining robots to progress.
- Upgrade your tools to deal with tougher terrain.
- Watch out for traps hidden in the caves.
- Manage your energy to avoid losing the game from exhaustion.

## Gameplay

### Mining

To mine, simply walk up to a tile and press the movement key in that direction to strike it. Tiles have different durability levels, meaning some will take multiple hits to break.

Once you collect an ore, it will be stored in your bag. If your bag is full, you won’t be able to pick up any more ores. To empty it, return to the surface, where ores will be automatically sold. The money you earn can be used to buy items and upgrade your tools.

### Camera

When you move beyond the camera's edge, it will automatically adjust to reveal a new section of the map.  The map has limits, with indestructible walls on both the left and right sides.

### Energy System

Mining requires effort, and your character has a limited amount of energy. Every block you break consumes some of it, and if your energy reaches zero, the game is over.  Energy is always reset upon returning to the surface. Plan your actions carefully to avoid running out of energy too quickly!

### Player Resources

- **Ladders**: Allow vertical movement.
- **Platforms**: Help you reach distant areas horizontally.
- **Mining Robots**: Automatically mine blocks in front of them. When summoned, the robot copies your attributes in order to mine. This means that it will only destroy blocks that you can also destroy, with the same damage that you have at the time of summoning.

You can buy these items on the surface using the money earned from selling ores, or find them inside boxes scattered throughout the cave.

### Tile Types

- **Ground**: Weak blocks that break with a single hit.
- **Stone**: Becomes tougher as you go deeper.
- **Ores**: Valuable resources that can be collected.
- **Boxes**: Contain random items when broken.
- **Walls**: Indestructible blocks that limit exploration.

### Hazards

The cave is full of dangers that can quickly end your journey if you're not careful:

- **Spikes**: Touching them results in instant death.
- **Rolling Boulders**: Move horizontally and can crush the player, but you can safely stand on top of them.

Boulders can also destroy ladders and platforms.

## Compiling

To compile the game, follow these steps to ensure a smooth setup:

1. **Prerequisites**  
    Make sure you have the following installed on your development environment:
    - [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - An IDE or text editor of your choice (e.g., [Visual Studio Code](https://code.visualstudio.com/), [Visual Studio](https://visualstudio.microsoft.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/))
    - [Git](https://git-scm.com/)
    - A tool to extract compressed files (e.g., [7-Zip](https://www.7-zip.org/))

2. **Clone the Repository**  
    Open your terminal and run:

    ```bash
        git clone "https://github.com/Starciad/Depths.git"
    ```

    Choose an accessible directory for the project.

3. **Download the Assets**  
    Game assets are distributed separately to avoid storing binaries in the repository.  
    - Go to the [Releases page](https://github.com/Starciad/Depths/releases).
    - Download the latest asset archive.
    - Extract the contents and move them to:  
      `Depths\src\Projects\Depths.Core\Assets\`

4. **Choose a Build Target**  
    The Depths project provides two main build targets:
    - **WindowsDX**: Native Windows build using DirectX.
    - **WebGL**: Browser build using [Kni](https://github.com/kniEngine/kni) and WebAssembly.  
      For WebGL, ensure you have the required [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) and web development tools installed.

5. **Open the Solution**  
    In `Depths\src\Projects`, you’ll find:
    - `Depths.WindowsDX.Project.sln` (for Windows/DirectX)
    - `Depths.WebGL.Project.sln` (for WebGL/Browser)

    Open the appropriate solution in your IDE and build the project. Both solutions share the same codebase, but target different platforms.

For more details on .NET project structure or troubleshooting, refer to the [official .NET documentation](https://learn.microsoft.com/en-us/dotnet/core/introduction).


## Screenshots

![screenshot_01]
![screenshot_02]
![screenshot_03]
![screenshot_04]
![screenshot_05]
![screenshot_06]
![screenshot_07]
![screenshot_08]
![screenshot_09]
![screenshot_10]
![screenshot_11]
![screenshot_12]
![screenshot_13]
![screenshot_14]
![screenshot_15]
![screenshot_16]
![screenshot_17]
![screenshot_18]
![screenshot_19]

## Credits

This project was entirely developed by me, **Starciad**.

When I came across this Game Jam, I was eager to challenge myself by creating something new. The limitations caught my attention, so I decided to participate. Even though I was worried about not finishing in time, I believe I managed to create something interesting.

At least... I hope you enjoy the game!

Some additional resources, such as fonts and sounds, are provided by the organizers of the Nokia Jam and can be found on the [Nokia Game Jam Resources](https://phillipp.itch.io/nokiajamresources) page.

## Licensing

### Project Code

All game source code is licensed under the MIT License.

### Assets

All game assets (graphics, music, sound effects, and other content) are licensed under a **Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License**.

<!-- IMAGES & ASSETS -->
[NOKIA_3310_JAM_7]: ./.github/Banners/NOKIA_3310_JAM_7.webp
[screenshot_01]: ./.github/Screenshots/Screenshot_1.webp
[screenshot_02]: ./.github/Screenshots/Screenshot_2.webp
[screenshot_03]: ./.github/Screenshots/Screenshot_3.webp
[screenshot_04]: ./.github/Screenshots/Screenshot_4.webp
[screenshot_05]: ./.github/Screenshots/Screenshot_5.webp
[screenshot_06]: ./.github/Screenshots/Screenshot_6.webp
[screenshot_07]: ./.github/Screenshots/Screenshot_7.webp
[screenshot_08]: ./.github/Screenshots/Screenshot_8.webp
[screenshot_09]: ./.github/Screenshots/Screenshot_9.webp
[screenshot_10]: ./.github/Screenshots/Screenshot_10.webp
[screenshot_11]: ./.github/Screenshots/Screenshot_11.webp
[screenshot_12]: ./.github/Screenshots/Screenshot_12.webp
[screenshot_13]: ./.github/Screenshots/Screenshot_13.webp
[screenshot_14]: ./.github/Screenshots/Screenshot_14.webp
[screenshot_15]: ./.github/Screenshots/Screenshot_15.webp
[screenshot_16]: ./.github/Screenshots/Screenshot_16.webp
[screenshot_17]: ./.github/Screenshots/Screenshot_17.webp
[screenshot_18]: ./.github/Screenshots/Screenshot_18.webp
[screenshot_19]: ./.github/Screenshots/Screenshot_19.webp
