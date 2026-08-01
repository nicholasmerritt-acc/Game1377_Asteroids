using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected AsteroidsPlayerController player;
    [SerializeField] protected float timeout = 4f;

    /// <summary>
    /// total number of powerups that exist. if too many exist, more will not spawn until some are collected
    /// </summary>
    public static int Count = 0;

    protected virtual void Start()
    {
        Count++;
    }

    protected virtual void OnDestroy()
    {
        Count--;
    }

    public void SetPlayerController(AsteroidsPlayerController playerIn)
    {
        player = playerIn;
    }

    /// <summary>
    /// Player has picked up this powerup
    /// </summary>
    public abstract void HandlePickup();
}
