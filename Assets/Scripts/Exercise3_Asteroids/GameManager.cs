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
        RespawnPlayer();
    }

    public void OnPlayerDeath(Vector3 currentLocation)
    {
        if (Lives > 0)
        {
            Lives--;
            RespawnPlayer(true);
        }
    }

    /// <summary>
    /// for now, spawn in a safe spot. TODO invincible
    /// </summary>
    /// <param name="spawnLocation"></param>
    private void RespawnPlayer(bool invincibleOnSpawn = false)
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
