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
    public float BulletSize = .1f;
    [SerializeField] private float initialBulletSize = .1f;

    [Header("Thrust")]
    [SerializeField] private float initialThrustForce = 10f;
    public float ThrustForce = 10f;
    [SerializeField] private float thrustInput;

    [Header("Rotation")]
    [SerializeField] private float initialRotationSpeed = 360f;
    public float RotationSpeed = 360f;
    [SerializeField] private float rotationInput;

    [Header("Teleporting")]
    [SerializeField] private float asteroidDetectionRadius = 1f;
    [SerializeField] private int maxLocationSearches = 100;
    [SerializeField] private Vector2 teleportDestination;
    [SerializeField] private LayerMask avoidAsteroidLayerMask;
    [SerializeField] private Vector2 initialSpawnLocation;

    [Header("Powerups / Effects")]
    [SerializeField] private float invincibleTimeout = 4f;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool destructionInProgress = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = false;
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
        audioManager = AudioManager.Instance;
        OnRespawn();
    }

    /// <summary>
    /// Set all variables to their initial state, on game start or respawn.
    /// This seems much more clunky and error-prone than destroying the object and Instantiating a new one.
    /// But it is less expensive, since we are not calling Destroy and Instantiate on every respawn.
    /// </summary>
    public void OnRespawn()
    {
        ThrustForce = initialThrustForce;
        thrustInput = 0;
        rotationInput = 0;
        RotationSpeed = initialRotationSpeed;
        BulletSize = initialBulletSize;
        lastFireTime = 0f;
        destructionInProgress = false;
        teleportDestination = Vector2.zero;
        transform.SetPositionAndRotation(initialSpawnLocation, Quaternion.identity);
        animator.SetTrigger("SpaceshipRespawn");
        animator.SetBool("SpaceshipDead", false);
        rb.angularVelocity = 0;
        rb.linearVelocity = Vector3.zero;
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
        transform.Rotate(Vector3.back * (rotationInput * RotationSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Thrust forward only, using rigidbody to apply force.
    /// </summary>
    private void HandleThrust()
    {
        if (thrustInput > 0)
        {
            rb.AddRelativeForce(Vector2.up * (ThrustForce * thrustInput * Time.deltaTime), ForceMode2D.Impulse);
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
        bulletObject.transform.localScale = Vector3.one * BulletSize;
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
            Collider2D hit = Physics2D.OverlapCircle(teleportDestination, asteroidDetectionRadius, avoidAsteroidLayerMask);
            if (hit == null)
            {
                break;
            }

        } while (++searches < maxLocationSearches);
        // Begin playing the first half of the teleport animation.
        animator.SetTrigger("TeleportBegin");
        audioManager.PlaySpaceshipTeleportClip();
        StartCoroutine(nameof(FinishTeleporting));
    }

    /// <summary>
    /// Play the last half of the animation once we have actually moved
    /// </summary>
    private IEnumerator FinishTeleporting()
    {
        yield return new WaitForSeconds(animator.GetCurrentClipLength());
        transform.position = teleportDestination;
        animator.SetTrigger("TeleportEnd");
    }

    /// <summary>
    /// Make the player invincible, for a short duration
    /// </summary>
    public void BecomeInvincible()
    {
        invincible = true;
        StartCoroutine(nameof(BecomeNotInvincible));
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
    private IEnumerator BecomeNotInvincible()
    {
        yield return new WaitForSeconds(invincibleTimeout);
        invincible = false;
    }

    /// <summary>
    /// Play animation of the player dying, then cleanup
    /// </summary>
    public void Die()
    {
        destructionInProgress = true;
        animator.SetTrigger("SpaceshipDied");
        animator.SetBool("SpaceshipDead", true);
        audioManager.PlaySpaceshipExplodeClip();
        DoDeathCleanup();
    }

    /// <summary>
    /// After the spaceship dies, we need to wait for animation to finish before destroying object and spawning new one
    /// </summary>
    public void DoDeathCleanup()
    {
        GameManager.Instance.OnPlayerDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destructionInProgress)
        {
            return;
        }

        if (!invincible && collision.gameObject.CompareTag("Asteroid"))
        {
            Die();
        }

        if (collision.CompareTag("Powerup"))
        {
            if (collision.TryGetComponent<Powerup>(out Powerup powerup))
            {
                powerup.HandlePickup();
            } 
            else
            {
                Debug.LogError("Powerup prefab is missing Powerup component!");
            }
        }
    }
}
