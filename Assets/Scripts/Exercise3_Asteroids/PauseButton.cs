using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject PauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PauseButtonEnable()
    {
        PauseMenu.SetActive(true);
    }

    public void PauseButtonDisable()
    {
        PauseMenu.SetActive(false);
    }
}
