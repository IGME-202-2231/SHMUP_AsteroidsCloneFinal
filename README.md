# Project 2 - SHMUP 

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Andrew Jameison
-   Section: 202.05-06

## Simulation Design

In this continuation of the SHMUP game, the player has been upgraded to a fully physics based movement system - but watch out, the enemies have recieved their own edge in this fight. As you fly through space you'll face larger and more frequent waves of enemies, you'll have to adapt to each enemy-type's method of attack. Your only objective is survival and to outlast the enemy armada for as long as possible.

### Controls

Mouse - Controls the direction of the ship and line of fire
Mouse1 - Fires a single projectile from your ship
Space - While held down, the ship will begin to accelerate

## Exploder

The exploders lack any weaponry, instead ramming themselves into the player ship to inflict massive damage.

### Player_Far

**Objective:** The exploder will accelerate towards the player as fast as possible.

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Pursue: The exploder is far more accurate than any ranged projectile, predicting the player's movement and accelerating towards it.
     Greater Max Speed - the exploder wants to get to the player as fast as possible
     Lower Max Force - the exploder has less turning control at this speed
     
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

The exploder far enough from the player that it doesn't have to worry about overshooting it's target
   
### Player_Close

**Objective:** To not overshoot the player, the exploder will slow down to a more controlled pursuit

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Pursue: The exploder is far more accurate than any ranged projectile, predicting the player's movement and accelerating towards it.
     Lower Max Speed - the exploder will need to slow down so it doesn't overshoot the player
     Greater Max Force - the exploder will have an easier time turning towards the player at this pace
       
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

The exploder is close enough to the player that it needs to slow down

## Artillery

The Artillery utilizes its long ranged attacks to damage the player from a safe distance. If you get too close, it'll retreat to a safer distance before continuing it's assault.

### Fire!

**Objective:** While immobile, the artillery can fire a power shot from across the map with great precision

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Stand-Still: simply reducing the velocity to zero would negate both seperation and obstacle avoidance, so when it can it'll attempt to stay still

Obstacles: The artillery will attempt to avoid roaming asteroids

Seperate: The artillery will seperate from it's allies; other artillery, flotilla-ships, and exploders
   
#### State Transistions

The artillery is at a safe enough distance from the player that it can begin firing
   
### Run Away!

**Objective:** The artillery is too close to the player to safely attack, and will retreat to a safer distance.

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Flee: The artillery will move away from the player

Obstacles: The artillery will attempt to avoid roaming asteroids

Seperate: The artillery will seperate from it's allies; other artillery, flotilla-ships, and exploders
   
#### State Transistions

The player gets too close to the artillery for it to fire effectively

## Flotilla-Ship

A flotilla is made up of many smaller ships, overwhelming the player with it's combined firepower.

### Charge!

**Objective:** The flotilla-ships will move towards the player while launching a barrage of projectiles

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Cohesion: If the flotilla-ship is part of a group, it'll stick near to the flotilla

Alignment: If the flotilla-ship is part of a group, it'll move in the direction of the flotilla

Obstacles: The flotilla-ship will attempt to avoid roaming asteroids

Seperate: The flotilla-ship will seperate from other flotilla-ships, exploders, artillery and the player  

#### State Transistions

The flotilla-ship is part of a sufficiently large group to advance upon the player with

The flotilla-ship sees no active flotilla to join, and will advance on it's own
   
### Regroup

**Objective:** Individual flotilla-ships will try and bolster the ranks of a larger group

#### Steering Behaviors

Stay-In-Bounds: The entity will avoid going outside the game-space

Seek: the flotilla-ship will navigate to the nearest active flotilla and join up with it

Obstacles: The flotilla-ship will attempt to avoid roaming asteroids

Seperate: The flotilla-ship will seperate from other flotilla-ships, exploders, artillery and the player 
   
#### State Transistions

The flotilla's number are insufficient to pursue the player

A lone flotilla-ship sees an active flotilla it can join up with

## Asteroids

These roaming obstacles come in many shapes and sizes, dealing damage to any entity that comes in contact with them corresponding to their sizes. The smaller ones might hurt, but the bigger ones will deal some real damage to your ship!

The enemy ships will try to avoid these space rocks, but they aren't always so lucky... they'll be taking damage too!

## Sources

Space Patrol Sprite Sheet - http://freegameassets.blogspot.com/search?q=space+patrol

Pixel Nebula Space Background - https://pixel-carvel.itch.io/space-background-2?download

HUD Assets - https://adwitr.itch.io/pixel-health-bar-asset-pack-2

Asteroids - https://foozlecc.itch.io/void-environment-pack?download

## Make it Your Own

This project builds upon the previous SHMUP game, replacing the previous vector movement with an upgraded physics based system.

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed
The flotilla-ships are using a stable build of code marked with //FLOTILLA BUILD, they are currently unable to regroup and find another flotilla to join because of time restraits. The potential upgrades are mostly defined in collision manager and the flotilla-ship script.

Asteroids are currently just set pieces, and do not have collisions available to them.

Enemies will not pathfind out of the way of the moving asteroids, because wander is a bane upon my existence.



