using UnityEngine;

public class PowerupLife : Powerup
{
    public override void HandlePickup()
    {
        GameManager.Instance.AddLife();
        Destroy(gameObject);
    }
}
