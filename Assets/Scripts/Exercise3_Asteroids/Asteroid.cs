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
public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private int childrenToSpawn = 2;

    [Header("Velocities")]
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;
    [SerializeField] private float minVelocity = -1.0f;
    [SerializeField] private float maxVelocity = 1.0f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //set random velocity that will stay constant through the asteroid's life
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
        Destroy(gameObject);
    }

    /// <summary>
    /// Spawn "children" of this asteroid of a certain size at our current position
    /// </summary>
    /// <param name="childSize"></param>
    private void SpawnChildren(AsteroidSize childSize)
    {
        if (asteroidSpawner == null)
        {
            Debug.LogError("No asteroid spawner found, child asteroids will not spawn.");
            return;
        }
        for (int i = 0; i < childrenToSpawn; i++)
        {
            asteroidSpawner.SpawnAsteroid(transform.position, childSize);
        }
    }

    /// <summary>
    /// Handle collisions with only player and bullets
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(collision.gameObject);
            gameManager.OnPlayerDeath(transform.position);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            BreakAsteroid();
        }
    }

    public void SetAsteroidSpawner(AsteroidSpawner newSpawner)
    {
        asteroidSpawner = newSpawner;
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }
}