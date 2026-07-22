using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public GameObject PlayerPrefab;
    public AsteroidSpawner AsteroidSpawner;
    public TMP_Text ScoreText;
    public TMP_Text LivesText;

    [Header("Player Stats")]
    [SerializeField] private int lives = 3;
    [SerializeField] private int score = 0;

    [Header("Respawning")]
    [SerializeField] private Vector3 initialSpawnLocation = Vector3.zero;
    [SerializeField] private bool invincibleOnSpawn = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RespawnPlayer();
        UpdateLivesDisplay();
        UpdateScoreDisplay();
        invincibleOnSpawn = true;
    }

    /// <summary>
    /// when the player dies, update game state accordingly and then respawn after delay
    /// </summary>
    /// <param name="currentLocation"></param>
    public void OnPlayerDeath(Vector3 currentLocation)
    {
        lives--;
        UpdateLivesDisplay();
        if (lives >= 0)
        {
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

    /// <summary>
    /// give the player an extra life
    /// </summary>
    public void AddLife()
    {
        lives++;
        UpdateLivesDisplay();
    }

    public void AddScore(int scoreIn)
    {
        score += scoreIn;
        UpdateScoreDisplay();
    }

    private void UpdateLivesDisplay()
    {
        LivesText.text = $"Lives: {lives}";
    }

    private void UpdateScoreDisplay()
    {
        ScoreText.text = $"Score: {score}";
    }
}
