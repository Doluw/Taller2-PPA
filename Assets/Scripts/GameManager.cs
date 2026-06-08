using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private bool startGameOnAwake = true;

    private bool isPlaying;
    private bool isGameOver;

    public bool IsPlaying => isPlaying;
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (startGameOnAwake)
        {
            StartGame();
        }
        else if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        isGameOver = false;
        Time.timeScale = 1f;

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBackgroundMusic();
        }
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isPlaying = false;
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverMusic();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
