using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;

    /// <summary>
    /// Start a new game of Asteroids
    /// </summary>
    public void StartGameClick()
    {
        AudioManager.Instance.PlayButtonPressClip();
        SceneManager.LoadScene("AsteroidsGame");
    }

    /// <summary>
    /// Open the settings menu
    /// </summary>
    public void SettingsClick()
    {
        AudioManager.Instance.PlayButtonPressClip();
        SettingsPanel.SetActive(true);
    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game.
    /// </summary>
    public void QuitGameClick()
    {
        AudioManager.Instance.PlayButtonPressClip();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
