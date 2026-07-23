using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject SettingsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGameOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateSettingsPanel()
    {
        SettingsPanel.SetActive(true);
    }

    public void MainMenuOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quit the game. Need to add alternate version if we ever want to build the game.
    /// </summary>
    public void QuitGameOnClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
