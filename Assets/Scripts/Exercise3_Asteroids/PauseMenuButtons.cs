using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject PausePanel;
    public AudioManager AudioManager {
        set;
        get
        {
            if (field == null)
            {
                field = AudioManager.Instance;
            }
            return field;
        }
    }

    public void RestartGameOnClick()
    {
        AudioManager.PlayButtonPressClip();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateSettingsPanel()
    {
        AudioManager.PlayButtonPressClip();
        SettingsPanel.SetActive(true);
    }

    public void MainMenuOnClick()
    {
        AudioManager.PlayButtonPressClip();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game.
    /// </summary>
    public void QuitGameOnClick()
    {
        AudioManager.PlayButtonPressClip();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ReturnToGameOnClick()
    {
        AudioManager.PlayButtonPressClip();
        GameManager.Instance.TogglePause();
        PausePanel.SetActive(false);
    }
}
