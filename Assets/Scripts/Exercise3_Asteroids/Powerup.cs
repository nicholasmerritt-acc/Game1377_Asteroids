using UnityEngine;

public class Powerup : MonoBehaviour
{
    public static int Count = 0;

    void Start()
    {
        Count++;
    }

    private void OnDestroy()
    {
        Count--;
    }
}
