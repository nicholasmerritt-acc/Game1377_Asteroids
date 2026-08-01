using UnityEngine;

public class PowerupLife : Powerup
{
    protected override void Start()
    {
        base.Start();
    }
    public override void HandlePickup()
    {
        GameManager.Instance.AddLife();
        Destroy(gameObject);
    }
}
