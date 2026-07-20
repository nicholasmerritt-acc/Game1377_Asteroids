using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        Rocket,
        Life,
        Move
    }

    //total number of powerups that exist. if too many exist, more will not spawn until some are collected
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
