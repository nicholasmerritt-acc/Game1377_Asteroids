using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject PausePanel;

    private AudioManager audioManager;

    public AudioManager AudioManagerInstance {
        set
        {
            audioManager = value;
        }
        get
        {
            if (audioManager == null)
            {
                audioManager = AudioManager.Instance;
            }
            return audioManager;
        }
    }

    public void RestartGameOnClick()
    {
        AudioManagerInstance.PlayButtonPressClip();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateSettingsPanel()
    {
        AudioManagerInstance.PlayButtonPressClip();
        SettingsPanel.SetActive(true);
    }

    public void MainMenuOnClick()
    {
        AudioManagerInstance.PlayButtonPressClip();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game.
    /// </summary>
    public void QuitGameOnClick()
    {
        AudioManagerInstance.PlayButtonPressClip();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ReturnToGameOnClick()
    {
        AudioManagerInstance.PlayButtonPressClip();
        GameManager.Instance.TogglePause();
        PausePanel.SetActive(false);
    }
}
