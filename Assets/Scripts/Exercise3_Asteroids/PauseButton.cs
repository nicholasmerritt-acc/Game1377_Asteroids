using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsPanel;
    public bool IsPaused = false;

    void Start()
    {
        IsPaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("PauseButton"))
        {
            TogglePauseMenu();
        }
    }

    /// <summary>
    /// Switch the pause menu on and off, and make sure settings panel is disabled if we are unpaused just in case.
    /// </summary>
    public void TogglePauseMenu()
    {
        IsPaused = !IsPaused;
        PauseMenu.SetActive(IsPaused);
        if (!IsPaused)
        {
            SettingsPanel.SetActive(false);
        }
    }
}
