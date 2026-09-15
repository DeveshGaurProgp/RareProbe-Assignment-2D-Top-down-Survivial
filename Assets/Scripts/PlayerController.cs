using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IHealth
{
    [Header("Components")]
    private Rigidbody2D m_Rb;

    [Header("Settings")]
    private const float m_MoveSpeed = 2f;
    private const float m_XRange = 8.5f;
    private const float m_YRange = 5.5f;

    public int Health { get; set; }
    private const int m_MaxHealth = 100;

    public event Action OnHealthChange;
    public event Action OnPlayerDied;

    void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        Health = 100;
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (InputManager.Instance.Move != Vector2.zero)
        {
            Vector2 moveposition = m_Rb.position + (InputManager.Instance.Move * m_MoveSpeed * Time.fixedDeltaTime);
            moveposition.x = Mathf.Clamp(moveposition.x, -m_XRange, m_XRange);
            moveposition.y = Mathf.Clamp(moveposition.y, -m_YRange, m_YRange);

            m_Rb.MovePosition(moveposition);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, m_MaxHealth);
        OnHealthChange?.Invoke();

        if (Health == 0) PlayerDied();
    }

    public void Heal(int amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, m_MaxHealth);
        OnHealthChange?.Invoke();
    }

    private void PlayerDied()
    {
        OnPlayerDied?.Invoke();
        this.gameObject.SetActive(false);
    }
}