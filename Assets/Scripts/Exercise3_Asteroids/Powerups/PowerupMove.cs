using System.Collections;
using UnityEngine;

public class PowerupMove : Powerup
{
    [SerializeField] private float thrustForceIncrement = 4f;
    [SerializeField] private float rotationSpeedIncrement = 30f;

    protected override void Start()
    {
        base.Start();
    }
    public override void HandlePickup()
    {
        StartCoroutine(nameof(IncreaseSpeedAndRotation));
        Destroy(gameObject);
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
}
