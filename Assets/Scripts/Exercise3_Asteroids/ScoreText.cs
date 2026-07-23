using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SetScoreText(GetComponent<TMP_Text>());
    }
}
