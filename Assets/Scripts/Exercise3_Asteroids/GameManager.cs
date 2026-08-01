using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const string GAME_SCENE_NAME = "AsteroidsGame";

    [Header("References")]
    public GameObject PlayerPrefab;
    public GameObject PlayerGameObject;
    [SerializeField] private AsteroidsPlayerController playerController;
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
    [SerializeField] private float respawnDelay = 1.0f;

    [Header("Pausing")]
    public bool GameIsPaused = false;

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

    private void Start()
    {
        SetupReferences(); //TODO comment out
    }

    private void SetupReferences()
    {
        if (ScoreText == null)
        {
            ScoreText = FindAnyObjectByType<ScoreText>().GetComponent<TMP_Text>();
        }
        if (LivesText == null)
        {
            LivesText = FindAnyObjectByType<LivesText>().GetComponent<TMP_Text>();
        }
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<AsteroidsPlayerController>();
            if (playerController == null)
            {
                //only instantiate if we truly have no player. otherwise, use the existing player in the scene
                playerController = Instantiate(PlayerPrefab, initialSpawnLocation, PlayerPrefab.transform.rotation).GetComponent<AsteroidsPlayerController>();
            }
            PlayerGameObject = playerController.gameObject;
        }
    }

    /// <summary>
    /// Setup a new game of Asteroids.
    /// </summary>
    private void InitializeGame()
    {
        SetupReferences();
        ResetInitialVariables();
        RespawnPlayer();
        UpdateLivesDisplay();
        UpdateScoreDisplay();
        AsteroidSpawner.Instance.SpawnInitialAsteroids();
    }

    /// <summary>
    /// Set all game state variables to their initial value, i.e. how they should be at the beginning of a new game.
    /// </summary>
    private void ResetInitialVariables()
    {
        Time.timeScale = 1.0f;
        lives = initialLives;
        score = initialScore;
        GameIsPaused = false;

        //make every respawn after the first one grant invincibility
        invincibleOnSpawn = true;
    }

    /// <summary>
    /// When the player dies, update game state accordingly and then respawn after delay
    /// </summary>
    /// <param name="currentLocation"></param>
    public void OnPlayerDeath()
    {
        lives--;
        UpdateLivesDisplay();
        PlayerGameObject.SetActive(false);
        if (lives >= 0)
        {
            StartCoroutine(nameof(RespawnAfterDelay));
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    /// <summary>
    /// Respawn the player in the center of the screen
    /// </summary>
    /// <param name="spawnLocation"></param>
    private void RespawnPlayer()
    {
        PlayerGameObject.SetActive(true);

        playerController = PlayerGameObject.GetComponent<AsteroidsPlayerController>();
        playerController.OnRespawn();
        if (invincibleOnSpawn)
        {
            playerController.BecomeInvincible();
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
        if (lives >= 0)
        {
            LivesText.text = $"Lives: {lives}";
        }
        else
        {
            LivesText.text = "Game Over!";
        }
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

    /// <summary>
    /// Pause or Unpause the game, and then return the current pause state
    /// </summary>
    /// <returns></returns>
    public bool TogglePause()
    {
        GameIsPaused = !GameIsPaused;

        if (GameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        return GameIsPaused;
    }
}
