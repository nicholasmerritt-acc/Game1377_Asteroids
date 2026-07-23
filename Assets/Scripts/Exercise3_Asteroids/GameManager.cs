using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const string GAME_SCENE_NAME = "AsteroidsGame";

    [Header("References")]
    public GameObject PlayerPrefab;
    public TMP_Text ScoreText;
    public TMP_Text LivesText;

    [Header("Player Stats")]
    [SerializeField] private int lives = 3;
    [SerializeField] private int initialLives = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private int initialScore = 0;

    [Header("Respawning")]
    [SerializeField] private Vector3 initialSpawnLocation = Vector3.zero;
    [SerializeField] private bool invincibleOnSpawn = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GAME_SCENE_NAME)
        {
            InitializeGame();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeGame()
    {
        Debug.Log("Gamemanager init...");
        lives = initialLives;
        score = initialScore;
        RespawnPlayer();
        UpdateLivesDisplay();
        UpdateScoreDisplay();
        invincibleOnSpawn = true;
    }

    /// <summary>
    /// When the player dies, update game state accordingly and then respawn after delay
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
    /// Respawn the player in the center of the screen
    /// </summary>
    /// <param name="spawnLocation"></param>
    private void RespawnPlayer()
    {
        GameObject player = Instantiate(PlayerPrefab, initialSpawnLocation, PlayerPrefab.transform.rotation);
        if (player.TryGetComponent<AsteroidsPlayerController>(out var controller))
        {
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
    /// Give the player an extra life
    /// </summary>
    public void AddLife()
    {
        lives++;
        UpdateLivesDisplay();
    }

    /// <summary>
    /// Score points and update UI
    /// </summary>
    /// <param name="scoreIn"></param>
    public void AddScore(int scoreIn)
    {
        score += scoreIn;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Update UI for Lives
    /// </summary>
    private void UpdateLivesDisplay()
    {
        if (LivesText == null)
        {
            return;
        }
        LivesText.text = $"Lives: {lives}";
    }

    /// <summary>
    /// Update UI for Score
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (ScoreText == null)
        {
            return;
        }
        ScoreText.text = $"Score: {score}";
    }

    public void SetScoreText(TMP_Text scoreTextIn)
    {
        ScoreText = scoreTextIn;
        UpdateScoreDisplay();
    }

    public void SetLivesText(TMP_Text livesTextIn)
    {
        LivesText = livesTextIn;
        UpdateLivesDisplay();
    }
}
