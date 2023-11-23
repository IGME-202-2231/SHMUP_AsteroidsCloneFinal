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

Pursue: 
     
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   - _eg, When this agent gets within range of Agent2_
   - _eg, When this agent has reached target of State2_
   
### Player_Close

**Objective:** 

#### Steering Behaviors

- _List all behaviors used by this state_
  
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## Artillery

The Artillery utilizes its long ranged attacks to damage the player from a safe distance. If you get too close, it'll retreat to a safer distance before continuing it's assault.

### Fire!

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   
### Run Away!

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## Flotilla-Ship

The flotilla is made up of many smaller ships, choosing to overwhelm the player with it's combined firepower.

### Charge

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   
### Regroup

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
Obstacles: The exploder will attempt to avoid roaming asteroids

Seperate: The exploder will seperate from it's allies; other exploders, flotilla-ships, and artillery
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## Sources

Space Patrol Sprite Sheet - http://freegameassets.blogspot.com/search?q=space+patrol

Pixel Nebula Space Background - https://pixel-carvel.itch.io/space-background-2?download

HUD Assets - https://adwitr.itch.io/pixel-health-bar-asset-pack-2

## Make it Your Own

This project builds upon the previous SHMUP game, replacing the previous vector movement with an upgraded physics based system.

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

