# Architecture

```
    Framework
    /       \
Gameplay    Platform
```

## Overview

In a nutshell, these are the main parts:

- `Platform` is the platform layer. It handles OS-specific calls like creating a window, rendering and input buffering.
- `Framework` is the abstraction layer. This is the toolbox to build the game with.
- `Gameplay` is the actual game. It contains entities and systems and manages the game state. It is supported by `Framework` to deal with graphics and user input.

## Gameplay

The game is structured as follows:
- The game loop processes input and updates the game simulation with a time delta
- The simulation loops through systems that operate on the game state
- Game state consists of both entities and global state

For simplicity, there are no components (like in [ECS](https://en.wikipedia.org/wiki/Entity_component_system)). Instead, the entities have flags to indicate types and behaviors.

```
          Gameplay
              |
          Simulation
        /            \
   Systems --Uses--> Game state
                          |
                       Entities
```
