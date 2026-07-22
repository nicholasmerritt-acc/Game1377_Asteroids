using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    /// <summary>
    /// Start a new game of Asteroids
    /// </summary>
    public void StartGameClick()
    {
        SceneManager.LoadScene("AsteroidsGame");
    }

    /// <summary>
    /// Open the settings menu
    /// </summary>
    public void SettingsClick()
    {

    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game.
    /// </summary>
    public void QuitGameClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
