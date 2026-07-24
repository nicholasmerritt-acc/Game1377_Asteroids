using UnityEngine;
using UnityEngine.InputSystem;

public class PauseButton : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsPanel;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnPause;
        inputActions.Player.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
    }

    public void OnPauseButtonClick()
    {
        AudioManager.Instance.PlayButtonPressClip();
        TogglePauseMenu();
    }

    /// <summary>
    /// Pause or unpause the game. Make sure all panels are disabled if we are unpaused.
    /// </summary>
    public void TogglePauseMenu()
    {
        bool isNowPaused = GameManager.Instance.TogglePause();
        PauseMenu.SetActive(isNowPaused);

        if (!isNowPaused)
            SettingsPanel.SetActive(false);
        }
    }
}
