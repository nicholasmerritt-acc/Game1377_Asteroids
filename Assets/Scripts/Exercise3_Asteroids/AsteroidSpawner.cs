/*
 * Assignment: Asteroids Game - AstroidSpawner Script - PART 2
 * 
 * Objective: Create a functional asteroid spawning script. This script will be responsible for spawning
 * asteroids at the start of the game, as well as spawning smaller asteroids when larger asteroids are destroyed. 
 * ALL ASTEROID SPAWNING SHOULD OCCUR THROUGH THIS SCRIPT. 
 
* Requirements:
* 1. Fill in the SpawnAsteroids method to spawn an asteroid at a location specified by the position and size parameters.
*       Hint: You may need to create a variable for the prefabs you need. 
*       Hint: Use the spawnXMax, spawnXMin, spawnYMax, and spawnYMin variables to determine where the asteroids can spawn.
* 2. Spawn a variable number of asteroids at the start of the game using the SpawnInitialAsteroids() method.
*       This should be determined by a private variable that can be set in the editor (set it to 5 in the Inspector). 
*       The asteroids should spawn at random positions within the camera view, but not too close to the center (0,0)
*       where the player will be (at least 3 units away from the center in any direction).
*       Hint: Vector3.Distance can tell you how far one point is away from another. 
*/
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] AsteroidPrefabs;
    public GameManager gameManager;

    [Header("Properties")]
    [SerializeField] private int initialAsteroids = 5;
    [SerializeField] private float playerSafeDistance = 3;
    [SerializeField] private float maxLocationSearches = 10000;

    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size. 
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;

    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;
        SpawnInitialAsteroids();
    }

    /// <summary>
    /// Spawn initial asteroids at random positions. Ensure that they do not spawn where the player is located.
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        Vector3 playerLocation = Vector3.zero;
        for (int i = 0; i < initialAsteroids; i++)
        {
            SpawnAsteroid(GetRandomSafeLocation(playerLocation), Asteroid.AsteroidSize.Large);
        }
    }

    /// <summary>
    /// Find a location for the player to spawn without fear of hitting an asteroid
    /// </summary>
    /// <param name="playerLocation"></param>
    /// <returns></returns>
    public Vector3 GetRandomSafeLocation(Vector2 playerLocation)
    {
        Vector2 randomPosition;
        int searches = 0;
        do
        {
            randomPosition = new Vector2(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop));

        } while (Vector2.Distance(randomPosition, playerLocation) < playerSafeDistance && ++searches < maxLocationSearches);
        Debug.Log($"Took {searches} searches to find a safe spot");
        return randomPosition;
    }

    /// <summary>
    /// Spawn an asteroid at the location specified by position parameter with the size specified by the size parameter.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size)
    {
        GameObject spawned = Instantiate(AsteroidPrefabs[(int)size], position, Quaternion.identity);
        if (spawned.TryGetComponent<Asteroid>(out var spawnedAsteroid))
        {
            spawnedAsteroid.SetAsteroidSpawner(this);
            spawnedAsteroid.SetGameManager(gameManager);
        }
    }
}