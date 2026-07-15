/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidsPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    [Header("Properties")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float invincibleTimeout = 4f;
    [SerializeField] private bool invincible = false;

    private float rotationInput;
    private float thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    /// <summary>
    /// Rotate the spaceship left or right in 2D space.
    /// </summary>
    private void HandleRotation()
    {
        transform.Rotate(Vector3.back * (rotationInput * rotationSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Thrust forward only, using rigidbody to apply force.
    /// </summary>
    private void HandleThrust()
    {
        if (thrustInput > 0)
        {
            rb.AddRelativeForce(Vector2.up * (thrustForce * thrustInput * Time.deltaTime), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Handle input related to firing bullets
    /// </summary>
    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire"))
        {
            FireBullet();
        }
    }

    /// <summary>
    /// Create a bullet at our fire point and orient it correctly. The bullet itself should handle its own movement.
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Handle input related to hyperspace-jumping
    /// </summary>
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Hyperspace"))
        {
            TeleportToRandomLocation();
        }
    }

    /// <summary>
    /// Staying within screen bounds, instantly transport the ship to a random location. Does not check if area is asteroid-occupied.
    /// </summary>
    private void TeleportToRandomLocation()
    {
        transform.position = asteroidSpawner.GetRandomSafeLocation(transform.position);
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }
    public void SetAsteroidSpawner(AsteroidSpawner newSpawner)
    {
        asteroidSpawner = newSpawner;
    }

    /// <summary>
    /// make the player invincible, for a short duration
    /// </summary>
    public void BecomeInvincible()
    {
        invincible = true;
        Invoke(nameof(BecomeNotInvincible), invincibleTimeout);
    }

    public bool IsInvincible()
    {
        return invincible;
    }

    /// <summary>
    /// make the player vincible
    /// </summary>
    private void BecomeNotInvincible()
    {
        invincible = false;
    }
}
