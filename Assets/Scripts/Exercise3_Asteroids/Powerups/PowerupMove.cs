using System.Collections;
using UnityEngine;

public class PowerupMove : Powerup
{
    [SerializeField] private float thrustForceIncrement = 4f;
    [SerializeField] private float rotationSpeedIncrement = 30f;

    public override void HandlePickup()
    {
        StartCoroutine(nameof(IncreaseSpeedAndRotation));
    }

    /// <summary>
    /// Apply powerup that increases rotation speed and movement speed for a limited time
    /// </summary>
    private IEnumerator IncreaseSpeedAndRotation()
    {
        player.RotationSpeed += rotationSpeedIncrement;
        player.ThrustForce += thrustForceIncrement;
        yield return new WaitForSeconds(timeout);
        player.RotationSpeed -= rotationSpeedIncrement;
        player.ThrustForce -= thrustForceIncrement;
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
