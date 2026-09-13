using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_GameUI;
    [SerializeField] private GameObject m_GameOverPanel;

    public void BackToGame()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        m_GameUI.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}