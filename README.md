# Game1377 Exercise 5: Asteroids Part 3

**New Classes in this Part:**

- [GameManager](Assets/Scripts/Exercise3_Asteroids/GameManager.cs) for handling game state
- [ExtensionMethods](Assets/Scripts/Exercise3_Asteroids/ExtensionMethods.cs) for extending the unity Animation class (and maybe other stuff in the future. it's fun.) sure sure there's definitely a simpler way, but this way was fancy and cool.
- [EngineAudio](Assets/Scripts/Exercise3_Asteroids/EngineAudio.cs) for separating constant engine thrust sound from OneShots
- [Powerup](Assets/Scripts/Exercise3_Asteroids/Powerup.cs) for handling the Powerups count and enum
- [PowerupSpawner](Assets/Scripts/Exercise3_Asteroids/PowerupSpawner.cs) for spawning powerups at an interval.

**Feature checklist (everything done)**

**Lives**

- [x] Add lives so the player can respawn once they die.
- [x] They should respawn at the center (0,0,0) on the screen.
- [x] They should be invincible for a few seconds to allow them to avoid asteroids.
- [x] Add an animation
- [x] Add sound effect for when the player dies.

**Hyperspace**

- [x] Improve the Hyperspace method so the player will not spawn where they can potentially hit an asteroid.
- [x] Add an animation for hyperspace
- [x] Add sound effect for hyperspace.

**Fire**

- [x] Add an animation for bullet
- [x] Add sound effect for the bullet firing.
- [x] Add a cooldown to firing to prevent spamming the fire button.

**Thrust**

- [x] Add an animation for thrust
- [x] Add sound effect for the ship moving forward.

**Asteroid/Asteroid Spawner**

- [x] Add an animation for explode
- [x] Add sound effect for the asteroids exploding.

**Power Ups**

- [x] One power up should increase the number of lives you have.
- [x] One power up should increase the spaceship movement and rotation speed for a limited amount of time.
- [x] One power up should increase the size of your bullets for a limited amount of time.
