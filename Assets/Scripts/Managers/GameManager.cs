using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
    private GameState m_GameState;

    [SerializeField] private PlayerController m_PlayerController;

    public bool m_IsPlaying;
    public float GameTimer { get; private set; } = 0f;
    public int GameScore { get; set; } = 0;
    public int GameHighestScore { get; private set; }

    public static event Action OnScoreChange;
    public static event Action OnGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GameHighestScore = PlayerPrefs.GetInt("Highest Score", 0);
        m_PlayerController.OnPlayerDied += GameOver;
    }

    void OnDestroy()
    {
        m_PlayerController.OnPlayerDied -= GameOver;
    }

    void Start()
    {
        Playing();
    }

    void Update()
    {
        if(m_IsPlaying) GameTimer += Time.deltaTime;
    }

    private void SwitchState(GameState state) => m_GameState = state;

    private void Playing()
    {
        SwitchState(GameState.Playing);
        m_IsPlaying = true;
    }

    private void Paused()
    {
        SwitchState(GameState.Paused);
        m_IsPlaying = false;
    }

    private void GameOver()
    {
        SwitchState(GameState.GameOver);
        m_IsPlaying = false;

        if(GameHighestScore < GameScore)
        {
            PlayerPrefs.SetInt("Highest Score", GameScore);
            PlayerPrefs.Save();
        }
        OnGameOver?.Invoke();
    }

    public void AddScore(int score)
    {
        GameScore += score;
        OnScoreChange?.Invoke();
    }
}