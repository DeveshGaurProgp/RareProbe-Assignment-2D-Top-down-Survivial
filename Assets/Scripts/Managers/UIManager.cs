using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private PlayerController m_PlayerController;

    [Header("UI Element")]
    [SerializeField] private TextMeshProUGUI m_SurvivalTime;
    [SerializeField] private TextMeshProUGUI m_Score;
    [SerializeField] private TextMeshProUGUI m_HighestScore;
    [SerializeField] private Slider m_Health;

    [SerializeField] private GameObject m_GameUI;
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private GameObject m_GameOverPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GameManager.OnScoreChange += UpdateScore;
        m_PlayerController.OnHealthChange += UpdateHealth;
        GameManager.OnGameOver += ShowGameOverPanel;
    }

    void OnDestroy()
    {
        GameManager.OnScoreChange -= UpdateScore;
        m_PlayerController.OnHealthChange -= UpdateHealth;
        GameManager.OnGameOver -= ShowGameOverPanel;
    }

    void Start()
    {
        UpdateScore();
        DisplayHighestScore();
        UpdateHealth();
    }

    void Update()
    {
        if(GameManager.Instance.m_IsPlaying) UpdateSurvivalTime();
        if(InputManager.Instance.Pause) ShowPauseMenu();
    }
    
    private void UpdateSurvivalTime()
    {
        int convertedTime = (int)GameManager.Instance.GameTimer;
        m_SurvivalTime.text = convertedTime.ToString() + " Sec";
    }

    private void UpdateScore()
    {
        m_Score.text = GameManager.Instance.GameScore.ToString();
    }

    private void UpdateHealth()
    {
        m_Health.value = m_PlayerController.Health;
    }

    private void DisplayHighestScore()
    {
        m_HighestScore.text = GameManager.Instance.GameHighestScore.ToString();
    }

    public void ShowPauseMenu()
    {
        m_GameUI.SetActive(false);
        m_PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ShowGameOverPanel()
    {
        if(m_PauseMenu.activeSelf) m_PauseMenu.SetActive(false);
        m_GameUI.SetActive(false);
        m_GameOverPanel.SetActive(true);
    }
}