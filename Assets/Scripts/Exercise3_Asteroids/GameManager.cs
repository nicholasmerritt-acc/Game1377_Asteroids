using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject PlayerPrefab;
    public AsteroidSpawner asteroidSpawner;

    [Header("Respawning")]
    [SerializeField] private int lives = 3;
    [SerializeField] private float respawnDelay = .5f;
    [SerializeField] private Vector3 initialSpawnLocation = Vector3.zero;
    [SerializeField] private bool invincibleOnSpawn = false;

    void Start()
    {
        RespawnPlayer();
        invincibleOnSpawn = true;
    }

    /// <summary>
    /// when the player dies, update game state accordingly and then respawn after delay
    /// </summary>
    /// <param name="currentLocation"></param>
    public void OnPlayerDeath(Vector3 currentLocation)
    {
        if (lives > 0)
        {
            lives--;
            //Invoke(nameof(RespawnPlayer), respawnDelay); //TODO sometimes this causes 2 players to appear?!?!?! sppoooooky
            RespawnPlayer();
        }
    }

    /// <summary>
    /// respawn in the center of the screen
    /// </summary>
    /// <param name="spawnLocation"></param>
    private void RespawnPlayer()
    {
        GameObject player = Instantiate(PlayerPrefab, initialSpawnLocation, PlayerPrefab.transform.rotation);
        if (player.TryGetComponent<AsteroidsPlayerController>(out var controller))
        {
            controller.SetGameManager(this);
            controller.SetAsteroidSpawner(asteroidSpawner);
            if (invincibleOnSpawn)
            {
                controller.BecomeInvincible();
            }
        }
        else
        {
            Debug.LogError("Player prefab is missing Player Controller component!");
        }
    }
}
