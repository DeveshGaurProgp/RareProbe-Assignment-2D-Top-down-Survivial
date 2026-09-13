using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Components")]
    private PlayerInput m_PlayerInput;

    [Header("Actions")]
    private InputAction m_MovePlayer;
    private InputAction m_AttackEnemy;
    private InputAction m_PauseGame;

    [Header("Inputs")]
    public Vector2 Move { get; private set; }
    public Vector2 Attack { get; private set; }
    public bool Pause { get; private set; } = false;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        m_PlayerInput = GetComponent<PlayerInput>();

        m_MovePlayer = m_PlayerInput.actions.FindAction("Move");
        m_AttackEnemy = m_PlayerInput.actions.FindAction("Attack");
        m_PauseGame = m_PlayerInput.actions.FindAction("Pause");
    }

    void Update()
    {
        if(GameManager.Instance.m_IsPlaying) Input();
    }

    private void Input()
    {
        Move = m_MovePlayer.ReadValue<Vector2>();
        Attack = m_AttackEnemy.ReadValue<Vector2>();
        Pause = m_PauseGame.WasPressedThisFrame();
    }
}