# Brick n Balls
Simple Game using Unity ECS

## Description:
The player shoots a limited number of balls that bounce off the environment, destroy bricks, and disappear after leaving the bottom of the play area.
Bricks have between 1 and 3 hit points, and each successful hit awards one point. The goal is to manage the limited number of shots and achieve the highest possible score before all balls are lost.

## Architecture (Unity DOTS / ECS)

The project uses **Unity DOTS (Data-Oriented Technology Stack)** to implement gameplay logic and physics in a performant and scalable way.

- Core gameplay logic and all physics simulation are built with **Unity ECS and Unity Physics**, without relying on MonoBehaviour-based physics.
- Gameplay entities such as balls, bricks, walls, and dead zones are represented as ECS entities with data-only components.
- Systems and Burst-compiled jobs are used to process movement, collisions, scoring, and entity lifetime in a data-oriented manner.
- The **presentation layer** (meshes, visuals, animations, UI) is handled separately using classic GameObjects.
- UI is placed in a dedicated scene, while ECS-driven gameplay runs independently in gameplay scenes.

This architecture provides a clear separation between data, logic, and presentation, while ensuring high performance and full compatibility with **URP, the New Input System, and IL2CPP with maximum optimization**.

## Gameplay:
<img src="images/gameplay.gif" width="800"/>
<img src="images/screen1.png" width="800"/>
<img src="images/screen2.png" width="800"/>
<img src="images/screen3.png" width="800"/>

## What I’m Learning:
1. Implementing physics-based gameplay using Unity ECS and Unity Physics
2. Handling input and game mechanics with the new Input System
3. Structuring a game with multiple scenes and UI managed separately from gameplay
4. Managing score, game state, and limited resources in an ECS-driven environment
5. Integrating 3D physics with 2D visuals for a hybrid presentation layer
