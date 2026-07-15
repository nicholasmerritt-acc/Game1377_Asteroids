using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject PlayerPrefab;
    public AsteroidSpawner asteroidSpawner;
    public int Lives = 3;

    [Header("Locations")]
    [SerializeField] private Vector3 initialSpawnLocation = Vector3.zero;

    void Start()
    {
        RespawnPlayer(initialSpawnLocation);
    }

    public void OnPlayerDeath(Vector3 currentLocation)
    {
        if (Lives > 0)
        {
            Lives--;
            RespawnPlayer(asteroidSpawner.GetRandomSafeLocation(currentLocation));
        }
    }

    /// <summary>
    /// for now, spawn in a safe spot. TODO invincible
    /// </summary>
    /// <param name="spawnLocation"></param>
    private void RespawnPlayer(Vector3 spawnLocation)
    {
        GameObject player = Instantiate(PlayerPrefab, spawnLocation, PlayerPrefab.transform.rotation);
        if (player.TryGetComponent<AsteroidsPlayerController>(out var controller))
        {
            controller.SetGameManager(this);
            controller.SetAsteroidSpawner(asteroidSpawner);
        }
    }
}
