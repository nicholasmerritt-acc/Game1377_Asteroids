using UnityEngine;

public class PowerupLife : Powerup
{
    public override void HandlePickup()
    {
        GameManager.Instance.AddLife();
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
