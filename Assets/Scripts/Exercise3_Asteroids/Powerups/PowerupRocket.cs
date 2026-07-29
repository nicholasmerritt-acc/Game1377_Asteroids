using System.Collections;
using UnityEngine;

public class PowerupRocket : Powerup
{
    [SerializeField] private float bulletSizeIncrement = .2f;

    public override void HandlePickup()
    {
        StartCoroutine(nameof(IncreaseBulletSize));
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HandlePickup();
            Destroy(gameObject);
        }
        Debug.Log("here trigger");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePickup();
            Destroy(gameObject);
        }
        Debug.Log("here collision");
    }
}
