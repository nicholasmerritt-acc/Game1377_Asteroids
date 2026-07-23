using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool IsPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void TogglePauseMenu()
    {
        IsPaused = !IsPaused;
        PauseMenu.SetActive(IsPaused);
    }
}
