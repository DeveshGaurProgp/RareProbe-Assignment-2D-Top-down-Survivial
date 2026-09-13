using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] private GameObject[] m_Enemies;
    private AudioSource m_AS;

    [Header("Pools")]
    private Queue<GameObject> m_WalkersPool = new Queue<GameObject>();
    private Queue<GameObject> m_RunnerPool = new Queue<GameObject>();
    private Queue<GameObject> m_BigBullPool = new Queue<GameObject>();

    [Header("Settings")]
    private float m_SpawnTimer = 0f;
    private float m_RiskZoneTiming = 45f;
    private float m_DeathZoneTiming = 90f;

    private float m_MaxSpawnX = 14f;
    private float m_MaxSpawnY = 8f;
    private float m_ZPosition = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        m_AS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.Instance.m_IsPlaying) SpawnEnemies();
    }

    private Vector3 SpawnLocationFinder()
    {
        float RandomX = UnityEngine.Random.Range(-m_MaxSpawnX, m_MaxSpawnX);
        float RandomY = UnityEngine.Random.Range(-m_MaxSpawnY, m_MaxSpawnY);
        Vector3 spawnLocation = new Vector3(RandomX, RandomY, m_ZPosition);

        return spawnLocation;
    }

    private void SpawnEnemies()
    {
        m_SpawnTimer += Time.deltaTime;

        if(m_SpawnTimer >= 1f)
        {
            if (GameManager.Instance.GameTimer < m_RiskZoneTiming)
            {
                PoolEnemy(m_WalkersPool , 0);
            }

            else if (GameManager.Instance.GameTimer < m_DeathZoneTiming)
            {
                PoolEnemy(m_WalkersPool, 0);
                PoolEnemy(m_RunnerPool, 1);
            }
            else if (GameManager.Instance.GameTimer >= m_DeathZoneTiming)
            {
                PoolEnemy(m_WalkersPool, 0);
                PoolEnemy(m_RunnerPool, 1);
                PoolEnemy(m_BigBullPool, 2);
            }
            m_SpawnTimer = 0f;
        }
    }

    private void PoolEnemy(Queue<GameObject> pool , int enemyIndex)
    {
        GameObject enemy;

        if(pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else enemy = Instantiate(m_Enemies[enemyIndex]);

        enemy.transform.position = SpawnLocationFinder();

        EnemyController controller = enemy.GetComponent<EnemyController>();
        controller.EnemyIndex = enemyIndex;

        enemy.transform.SetParent(transform);
        enemy.SetActive(true);
    }

    public void DepoolEnemy(GameObject enemy, int enemyIndex)
    {
        enemy.SetActive(false);
        switch(enemyIndex)
        {
            case 0:
                m_WalkersPool.Enqueue(enemy);
                break;
            case 1:
                m_RunnerPool.Enqueue(enemy);
                break;
            case 2:
                m_BigBullPool.Enqueue(enemy);
                break;
            default:
                break;
        }
        m_AS.Play();
    }
}