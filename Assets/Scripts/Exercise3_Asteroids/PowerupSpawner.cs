using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private float initialSpawnWaitTime = 3f;
    [SerializeField] private float spawnRepeatInterval = 4f;
    [SerializeField] private int maxPowerups = 3;
    public GameObject[] PowerupPrefabs;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerup), initialSpawnWaitTime, spawnRepeatInterval);
    }

    /// <summary>
    /// create a powerup within the bounds of the screen
    /// </summary>
    void SpawnPowerup()
    {
        if (Powerup.Count < maxPowerups)
        {
            Vector2 powerupPosition = new Vector2(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop));
            Instantiate(PowerupPrefabs[Random.Range(0, PowerupPrefabs.Length)], powerupPosition, Quaternion.identity);
        }
    }
}
