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
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;

    [Header("Firing")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float lastFireTime;
    [SerializeField] private float fireTimeout = 1f;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float rotationInput;
    [SerializeField] private float thrustInput;

    [Header("Teleporting")]
    [SerializeField] private float asteroidSafeDistance = 1.0f;
    [SerializeField] private float asteroidDetectionRadius = .5f;
    [SerializeField] private int maxLocationSearches = 100;
    [SerializeField] private Vector2 teleportDestination;

    [Header("Powerups / Effects")]
    [SerializeField] private float invincibleTimeout = 4f;
    [SerializeField] private bool invincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    private void HandleInput()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
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
            animator.SetBool("Thrusting", true);
        }
        else
        {
            animator.SetBool("Thrusting", false);
        }
    }

    /// <summary>
    /// Handle input and timing related to firing bullets
    /// </summary>
    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire"))
        {
            if (Time.time - lastFireTime > fireTimeout)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
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
            BeginTeleportToRandomLocation();
        }
    }

    /// <summary>
    /// Staying within screen bounds, instantly transport the ship to a random safe location, with no asteroids within asteroidSafeDistance.
    /// </summary>
    private void BeginTeleportToRandomLocation()
    {
        teleportDestination = Vector2.zero;
        int searches = 0;
        do
        {
            teleportDestination = new Vector2(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop));
            RaycastHit2D hit = Physics2D.CircleCast(teleportDestination, asteroidDetectionRadius, Vector2.right, asteroidSafeDistance, ~LayerMask.NameToLayer("Asteroid"));
            if (hit.collider != null)
            {
                if (gameManager.DebugMode)
                {
                    Debug.Log($"we hit an asteroid: {hit.collider.gameObject.name} at location: {teleportDestination}");
                }
            }
            else
            {
                if (gameManager.DebugMode)
                {
                    Debug.Log("no asteroids nearby");
                }
                break;
            }

        } while (++searches < maxLocationSearches);

        if (gameManager.DebugMode)
        {
            Debug.Log($"Took {searches} searches to find a safe spot for the player");
        }

        // Begin playing the first half of the teleport animation.
        animator.SetTrigger("TeleportBegin");
        Invoke(nameof(FinishTeleporting), animator.GetCurrentClipLength());
    }

    /// <summary>
    /// Play the last half of the animation once we have actually moved
    /// </summary>
    private void FinishTeleporting()
    {
        transform.position = teleportDestination;
        animator.SetTrigger("TeleportEnd");
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    /// <summary>
    /// make the player invincible, for a short duration
    /// </summary>
    public void BecomeInvincible()
    {
        invincible = true;
        Invoke(nameof(BecomeNotInvincible), invincibleTimeout);
    }

    /// <summary>
    /// will the player ignore impact with an asteroid?
    /// </summary>
    /// <returns></returns>
    public bool IsInvincible()
    {
        return invincible;
    }

    /// <summary>
    /// make the player vincible again
    /// </summary>
    private void BecomeNotInvincible()
    {
        invincible = false;
    }

    /// <summary>
    /// play animation of the player dying, then cleanup
    /// </summary>
    public void Die()
    {
        animator.SetTrigger("SpaceshipDied");
        Invoke(nameof(DoDeathCleanup), animator.GetCurrentClipLength());
    }

    /// <summary>
    /// after the spaceship dies, we need to wait for animation to finish before destroying object and spawning new one
    /// </summary>
    private void DoDeathCleanup()
    {
        gameManager.OnPlayerDeath(transform.position);
        Destroy(gameObject);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, asteroidDetectionRadius);

    //    Gizmos.color = Color.yellow;
    //    Vector2 endpos = transform.position + transform.right * asteroidSafeDistance;
    //    Gizmos.DrawWireSphere(endpos, asteroidDetectionRadius);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(transform.position, endpos);
    //}
}
