using UnityEngine;

public class EnemyController : MonoBehaviour, IHealth
{
    [Header("Data")]
    [SerializeField] private SO_EnemyData m_EnemyData;
    private Transform m_Player;

    public int EnemyIndex { get; set; }
    public int Health { get; set; }

    void Start()
    {
        Health = m_EnemyData.m_EnemyHealth;
        m_Player = GameObject.Find("Player").transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    void Update()
    {
        if (GameManager.Instance.m_IsPlaying) ChaseTarget();
    }

    private void ChaseTarget()
    {
        Vector3 direction = (m_Player.position - transform.position).normalized;
        transform.Translate(direction * m_EnemyData.m_EnemySpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        PlayerController playerHealth;
        if(m_Player.gameObject.TryGetComponent<PlayerController>(out playerHealth))
        {
            playerHealth.TakeDamage(m_EnemyData.m_Damage);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, m_EnemyData.m_EnemyHealth);

        if (Health == 0) EnemyDied();
    }

    private void EnemyDied()
    {
        GameManager.Instance.AddScore(m_EnemyData.m_KillScore);
        EnemyManager.Instance.DepoolEnemy(this.gameObject, EnemyIndex);
    }
}