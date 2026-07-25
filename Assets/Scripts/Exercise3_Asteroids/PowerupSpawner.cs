using System.Collections;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private float initialSpawnWaitTime = 3f;
    [SerializeField] private float spawnRepeatInterval = 4f;
    [SerializeField] private int maxPowerups = 3;
    public GameObject[] PowerupPrefabs;

    void Start()
    {
        StartCoroutine(nameof(SpawnPowerup));
    }

    /// <summary>
    /// Coroutine that creates a powerup within the bounds of the screen
    /// </summary>
    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(initialSpawnWaitTime);
        //while (GameManager.Instance.GameIsActive)
        while (true)
        {
            Vector2 powerupPosition = new Vector2(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop));
            Instantiate(PowerupPrefabs[Random.Range(0, PowerupPrefabs.Length)], powerupPosition, Quaternion.identity);

            yield return new WaitUntil(PowerupsLessThanMax);
            yield return new WaitForSeconds(spawnRepeatInterval);
        }
    }

    /// <summary>
    /// Returns true if we have capacity to spawn another powerup
    /// </summary>
    /// <returns></returns>
    private bool PowerupsLessThanMax()
    {
        return Powerup.Count < maxPowerups;
    }
}
