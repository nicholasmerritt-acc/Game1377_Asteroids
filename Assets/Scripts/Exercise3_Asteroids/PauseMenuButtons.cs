using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject PausePanel;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void RestartGameOnClick()
    {
        audioManager.PlayButtonPressClip();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingsButtonOnClick()
    {
        audioManager.PlayButtonPressClip();
        SettingsPanel.SetActive(true);
    }

    /// <summary>
    /// Return to the main menu
    /// </summary>
    public void MainMenuOnClick()
    {
        audioManager.PlayButtonPressClip();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game, since this will only work in the Editor
    /// </summary>
    public void QuitGameOnClick()
    {
        audioManager.PlayButtonPressClip();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    /// <summary>
    /// unpause the game and hide the pause menu
    /// </summary>
    public void ReturnToGameOnClick()
    {
        audioManager.PlayButtonPressClip();
        GameManager.Instance.TogglePause();
        PausePanel.SetActive(false);
    }
}
