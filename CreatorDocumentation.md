# Documentation
In the project, there exists two tools:

- [An enemy creator](#enemy-creator)
- [A level creator](#level-creator)

You can access these by clicking the Tools on the top, and clicking their respective buttons.
<p align="left">
  <img src="https://i.imgur.com/XEpq2Vf.png" />
</p>

# Enemy Creator

<p align="left">
  <img src="https://i.imgur.com/7GpQEgq.png" />
</p>

## About:

This tool is meant for creating the enemies that you face in the levels. When you press create, it will create folders for the enemy and connect all its animation files together.


## Requirements:
- Spritesheet
- Knowledge of Unity's animation system


## How to use:

1. Add an initial sprite
2. Fill the values
3. Name the enemy.
4. Press Create Enemy
5. Read below

## Other important info:

**(THIS IS REQUIRED FOR THE ENEMY TO WORK PROPERLY)**

The animation states that are given to the enemy are:
- Run
- Attack
- Heal
- Cast
- Hit
- Death

Using the animator in Unity, all that is needed to be done from the user is to import the spritesheet and add the sprites animations in accordance to the states.

There are also event triggers (functions) that must be added to the animations wherever the user deems fit to do so. This can also be done through the animator. If you do not know how to do this, follow this [tutorial](https://www.youtube.com/watch?v=-IuvXTnQS4U).

The animations that are needed to have triggers are:
- Attack
  - DoDamage
  - AnimationFinish
- Heal
  - MagicAnimationFinish
- Cast
  - DoMagicDamage
  - MagicAnimationFinish
- Death
  - FinishDeath

# Level Creator

<p align="left">
  <img src="https://i.imgur.com/0PgxURw.png" />
</p>

## About:
This tool is for creating levels.

## Requirements:
- Enemies to add into the level.

## How to use:

1. Add background
2. Choose the number of enemies that you would like.
3. Add the enemies in the order you would like.
4. Name level.
5. Press Make Level!
6. Save the Scene.
7. Go into File -> Build Settings and add the open scene.



 


