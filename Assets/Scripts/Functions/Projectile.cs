using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_ProjectileSpeed = 5f;
    private int m_DamagePower = 25;

    private float m_MaxXRange = 20f;
    private float m_MaxYRange = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            GiveDamage(other.gameObject);
        }
    }

    void Update()
    {
        if (GameManager.Instance.m_IsPlaying) Movement();
        OutOfScope();
    }

    private void Movement()
    {
        transform.Translate(Vector3.right * m_ProjectileSpeed * Time.deltaTime);
    }

    private void GiveDamage(GameObject enemy)
    {
        EnemyController enemyHealth;
        if(enemy.TryGetComponent<EnemyController>(out enemyHealth))
        {
            enemyHealth.TakeDamage(m_DamagePower);
        }
        ProjectileManager.Instance.DepoolProjectile(this.gameObject);
    }

    private void OutOfScope()
    {
        if(transform.localPosition.x > m_MaxXRange || transform.localPosition.x < -m_MaxXRange
            || transform.localPosition.y > m_MaxYRange || transform.localPosition.y < -m_MaxYRange)
        {
            ProjectileManager.Instance.DepoolProjectile(this.gameObject);
        }
    }
}