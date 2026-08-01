using System.Collections;
using UnityEngine;

public class PowerupRocket : Powerup
{
    [SerializeField] private float bulletSizeIncrement = .2f;

    protected override void Start()
    {
        base.Start();
    }

    public override void HandlePickup()
    {
        StartCoroutine(nameof(IncreaseBulletSize));
        Destroy(gameObject);
    }

    /// <summary>
    /// Apply powerup which increases bullet size for a limited time
    /// </summary>
    private IEnumerator IncreaseBulletSize()
    {
        player.BulletSize += bulletSizeIncrement;
        yield return new WaitForSeconds(timeout);
        player.BulletSize -= bulletSizeIncrement;
    }
}
