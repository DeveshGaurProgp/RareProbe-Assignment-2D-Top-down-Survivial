using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private GameObject m_HealthPowerUp;
    private AudioSource m_AS;

    [Header("Pools")]
    private Queue<GameObject> m_HealthPowerUpsPool = new Queue<GameObject>();

    [Header("Settings")]
    private const float m_MaxSpawnX = 8.4f;
    private const float m_MaxSpawnY = 5f;
    private const float m_ZPosition = 0f;

    private int m_PowerUpsCount = 0;
    private const int m_MaxPowerUp = 3;

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
        if (GameManager.Instance.m_IsPlaying) SpawnPowerUps();
    }

    private Vector3 SpawnLocationFinder()
    {
        float RandomX = UnityEngine.Random.Range(-m_MaxSpawnX, m_MaxSpawnX);
        float RandomY = UnityEngine.Random.Range(-m_MaxSpawnY, m_MaxSpawnY);
        Vector3 spawnLocation = new Vector3(RandomX, RandomY, m_ZPosition);

        return spawnLocation;
    }

    private void SpawnPowerUps()
    {
        while(m_PowerUpsCount < m_MaxPowerUp)
        {
            PoolPowerUp(m_HealthPowerUpsPool);
            m_PowerUpsCount++;
        }
    }

    private void PoolPowerUp(Queue<GameObject> pool)
    {
        GameObject PowerUp;

        if (pool.Count > 0)
        {
            PowerUp = pool.Dequeue();
        }
        else PowerUp = Instantiate(m_HealthPowerUp);

        PowerUp.transform.position = SpawnLocationFinder();

        PowerUp.transform.SetParent(transform);
        PowerUp.SetActive(true);
    }

    public void DepoolPowerUp(GameObject PowerUp)
    {
        m_PowerUpsCount--;
        PowerUp.SetActive(false);
        m_HealthPowerUpsPool.Enqueue(PowerUp);
        m_AS.Play();
    }
}