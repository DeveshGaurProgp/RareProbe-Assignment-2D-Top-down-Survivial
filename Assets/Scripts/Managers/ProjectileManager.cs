using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [SerializeField] private GameObject m_ProjectilePrefab;
    private AudioSource m_AS;

    [Header("Pools")]
    private Queue<GameObject> m_ProjectilePool = new Queue<GameObject>();

    [Header("Settings")]
    [SerializeField] private Transform m_Player;
    private float m_SpawnTimer = 0f;
    private float m_SpawnInterval = 0.5f;

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
        if (GameManager.Instance.m_IsPlaying) SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        m_SpawnTimer += Time.deltaTime;
        if(m_SpawnTimer > m_SpawnInterval)
        {
            PoolProjectile();
            m_AS.Play();
            m_SpawnTimer = 0f;
        }
    }

    private void PoolProjectile()
    {
        GameObject projectile;

        if(m_ProjectilePool.Count > 0)
        {
            projectile = m_ProjectilePool.Dequeue();
        }
        else projectile = Instantiate(m_ProjectilePrefab);

        projectile.transform.position = m_Player.position;
        if (InputManager.Instance.Attack != Vector2.zero)
        {
            float angle = Mathf.Atan2(InputManager.Instance.Attack.y, InputManager.Instance.Attack.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        projectile.transform.SetParent(transform);
        projectile.SetActive(true);
    }

    public void DepoolProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        m_ProjectilePool.Enqueue(projectile);
    }
}