using TMPro;
using UnityEngine;

public class LivesText : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SetLivesText(GetComponent<TMP_Text>());
    }
}
