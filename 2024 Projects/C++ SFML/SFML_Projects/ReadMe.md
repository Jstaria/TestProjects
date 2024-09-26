### [< Back](https://github.com/Jstaria/TestProjects/tree/main?tab=readme-ov-file#readme)

## Crypt Keeper (2024 Projects/C++ SFML)

#### General Test Level (Light not working)
![Crypt Keeper Test Level](https://i.imgur.com/jH10KCo.png)

#### Level Editor Example
![Crypt Keeper Editor](https://i.imgur.com/W6zY8Yo.png)

- Using SFML Framework, built a 2D platformer from scratch
- Uses a custom-built level creator which includes:
  - tiles
  - hitboxes
  - camera-bounding boxes
  - entities
- Started a light system, including mesh for lights built with world collision using raycasts and triangulation

### General Mechanic/Game Design In-Depth
Player movement:
  - Coyote Time
  - Custom jump hang time
  - Clamped fall speed
  - Bonus peak jump movement
  - Falling too far shakes camera

#### Dirty Camera Shake Example
![Camera Shaking](https://i.imgur.com/rzVd78M.gif)
 
Interaction:
  - (In-works) Checkpoint
    - light and unlight fire pit
  - Camera movement
    - Bound to read-in areas
    - Smoothly moves to character
    - Has different move speeds based on movement direction
  
