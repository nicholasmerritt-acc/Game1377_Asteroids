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

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class AsteroidsPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private InputSystem_Actions inputActions;
    private AudioManager audioManager;

    [Header("Firing")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float lastFireTime;
    [SerializeField] private float fireTimeout = 1f;
    [SerializeField] private float initialBulletSize = .1f;
    [SerializeField] private float bulletSize = .1f;
    [SerializeField] private float bulletSizeIncrement = .2f;

    [Header("Thrust")]
    [SerializeField] private float initialThrustForce = 10f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float thrustForceIncrement = 4f;
    [SerializeField] private float thrustInput;

    [Header("Rotation")]
    [SerializeField] private float initialRotationSpeed = 360f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float rotationSpeedIncrement = 30f;
    [SerializeField] private float rotationInput;

    [Header("Teleporting")]
    [SerializeField] private float asteroidSafeDistance = 1.0f;
    [SerializeField] private float asteroidDetectionRadius = .5f;
    [SerializeField] private int maxLocationSearches = 100;
    [SerializeField] private Vector2 teleportDestination;

    [Header("Powerups / Effects")]
    [SerializeField] private float invincibleTimeout = 4f;
    [SerializeField] private float powerupTimeout = 4f;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool destructionInProgress = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Hyperspace.performed += OnHyperspace;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Hyperspace.performed -= OnHyperspace;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = AudioManager.Instance;
        thrustForce = initialThrustForce;
        rotationSpeed = initialRotationSpeed;
    }

    void Update()
    {
        HandleRotation();
    }

    public void OnHyperspace(InputAction.CallbackContext context)
    {
        BeginTeleportToRandomLocation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        rotationInput = moveInput.x;
        thrustInput = moveInput.y;
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
            audioManager.PlayEngineAudio();
        }
        else
        {
            animator.SetBool("Thrusting", false);
            audioManager.PauseEngineAudio();
        }
    }

    /// <summary>
    /// Handle input and timing related to firing bullets
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time - lastFireTime > fireTimeout)
            {
                FireBullet();
                lastFireTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Create a bullet at our fire point and orient and size it correctly. The bullet itself should handle its own movement.
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletObject.transform.localScale = Vector3.one * bulletSize;
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
            if (hit.collider == null)
            {
                break;
            }

        } while (++searches < maxLocationSearches);

        // Begin playing the first half of the teleport animation.
        animator.SetTrigger("TeleportBegin");
        audioManager.PlaySpaceshipTeleportClip();
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

    /// <summary>
    /// Make the player invincible, for a short duration
    /// </summary>
    public void BecomeInvincible()
    {
        invincible = true;
        Invoke(nameof(BecomeNotInvincible), invincibleTimeout);
    }

    /// <summary>
    /// Will the player ignore impact with an asteroid?
    /// </summary>
    /// <returns></returns>
    public bool IsInvincible()
    {
        return invincible;
    }

    /// <summary>
    /// Make the player vincible again
    /// </summary>
    private void BecomeNotInvincible()
    {
        invincible = false;
    }

    /// <summary>
    /// Play animation of the player dying, then cleanup
    /// </summary>
    public void Die()
    {
        destructionInProgress = true;
        animator.SetTrigger("SpaceshipDied");
        audioManager.PlaySpaceshipExplodeClip();
        Invoke(nameof(DoDeathCleanup), animator.GetCurrentClipLength());
    }

    /// <summary>
    /// After the spaceship dies, we need to wait for animation to finish before destroying object and spawning new one
    /// </summary>
    private void DoDeathCleanup()
    {
        GameManager.Instance.OnPlayerDeath(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destructionInProgress)
        {
            return;
        }
        if (collision.gameObject.CompareTag("PowerupRocket"))
        {
            StartCoroutine(PowerupRocket());
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PowerupLife"))
        {
            GameManager.Instance.AddLife();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PowerupMove"))
        {
            StartCoroutine(PowerupMove());
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Apply powerup that increases rotation speed and movement speed for a limited time
    /// </summary>
    private IEnumerator PowerupMove()
    {
        rotationSpeed += rotationSpeedIncrement;
        thrustForce += thrustForceIncrement;
        yield return new WaitForSeconds(powerupTimeout);
        rotationSpeed -= rotationSpeedIncrement;
        thrustForce -= thrustForceIncrement;
    }

    /// <summary>
    /// Apply powerup which increases bullet size for a limited time
    /// </summary>
    private IEnumerator PowerupRocket()
    {
        bulletSize += bulletSizeIncrement;
        yield return new WaitForSeconds(powerupTimeout);
        bulletSize = initialBulletSize;
    }
}
