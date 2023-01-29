using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 5;
    [SerializeField] int playerScore = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject statsCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject exitButton;

    public bool isPaused = true;

    void Awake()
    {
        int gameSessionsCount = FindObjectsOfType<GameSession>().Length;

        if (gameSessionsCount > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        ShowMenu();
    }

    void Start()
    {
        UpdateLivesText();
        UpdateScoreText();
    }

    void OnExit()
    {

        if (!isPaused)
            ShowMenu();
        else
            CloseMenu();
    }

    public void OnPlayButtonClick()
    {
        CloseMenu();
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
            TakeLife();
        else
            ResetSession();
    }

    public void ProcessPlayerScore()
    {
        playerScore += 50;
        UpdateScoreText();
    }

    void TakeLife()
    {
        playerLives--;
        ReloadCurrentLevel();
        UpdateLivesText();
    }

    void ReloadCurrentLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    void ResetSession()
    {
        FindObjectOfType<LevelPersist>().ResetLevelPersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void UpdateLivesText()
    {
        livesText.text = "Lives : " + playerLives;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + playerScore;
    }

    void ShowMenu()
    {
        isPaused = true;
        statsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    void CloseMenu()
    {
        isPaused = false;
        statsCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }
}