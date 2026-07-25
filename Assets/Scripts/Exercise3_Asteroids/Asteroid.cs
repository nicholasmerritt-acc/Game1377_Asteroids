/*
 * Assignment: Asteroids Game - Asteroid Script - PART 2
 * 
 * Objective: Create a functional asteroid script. This script will be responsible for the functionality of the asteroids.
 * this should include initial velocity, angular velocity, and breaking into smaller asteroids when destroyed.
 * Remember, asteroids should only spawn through the AsteroidSpawner script. 
 
* Requirements:
* 1. The asteroid should start with a set velocity but a random angular velocity. Both of these are set in the Rigidbody2D
*       Hint: All movement for the asteroid should be done via a Rigidbody2D and should be able to be set at Start.
* 2. When the asteroid is destroyed, it should spawn two smaller asteroids if it is not already the smallest size. 
*       Hint: How can you use a function to set the AsteroidSpawner variable from a different script?
* 3. When the asteroid hits the player, it should destroy the player. 
*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }
    public int Score = 20;

    [SerializeField] private AsteroidSize size;
    [SerializeField] private int childrenToSpawn = 2;
    [SerializeField] private bool destructionInProgress = false;

    [Header("Velocity / Rotation")]
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;
    [SerializeField] private float minVelocity = -1.0f;
    [SerializeField] private float maxVelocity = 1.0f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        SetInitialVelocities();
    }

    /// <summary>
    /// set random linear and angular velocities that will stay constant through the asteroid's life
    /// </summary>
    private void SetInitialVelocities()
    {
        Vector2 normalizedDirection = new Vector2(Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity)).normalized;
        rb.linearVelocity = normalizedDirection * speed;
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    /// <summary>
    /// Break large asteroid into smaller ones, then destroy self
    /// </summary>
    private void BreakAsteroid()
    {
        if (size != AsteroidSize.Small)
        {
            SpawnChildren(size - 1);
        }
        //disable collisions while animating
        destructionInProgress = true;

        //play explode animation and then destroy after animating is over
        animator.SetTrigger("AsteroidExplode");
        AudioManager.Instance.PlayAsteroidExplodeClip();
        Destroy(gameObject, animator.GetCurrentClipLength());
    }

    /// <summary>
    /// Spawn "children" of this asteroid of a certain size at our current position
    /// </summary>
    /// <param name="childSize"></param>
    private void SpawnChildren(AsteroidSize childSize)
    {
        for (int i = 0; i < childrenToSpawn; i++)
        {
            AsteroidSpawner.Instance.SpawnAsteroid(transform.position, childSize);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destructionInProgress)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.TryGetComponent<AsteroidsPlayerController>(out AsteroidsPlayerController controller))
            {
                if (!controller.IsInvincible())
                {
                    controller.Die();
                }
            } 
            else
            {
                Debug.LogError("GameObject with Player tag does not have correct Player Controller component!");
            }
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.Instance.AddScore(Score);
            Destroy(collision.gameObject);
            BreakAsteroid();
        }
    }
}