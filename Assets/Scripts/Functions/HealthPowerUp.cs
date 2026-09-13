using UnityEngine;

public class HealthPowerUp : MonoBehaviour , IPowerUp
{
    private int m_HealingAmount = 50;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Effect(other.gameObject);
        }
    }

    public void Effect(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.Heal(m_HealingAmount);
        PowerUpManager.Instance.DepoolPowerUp(this.gameObject);
    }
}