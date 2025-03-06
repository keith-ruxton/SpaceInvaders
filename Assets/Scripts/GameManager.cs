using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;
    public Text highScoreText;

    private int currentScore = 0;
    private int sessionHighScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Keeps GameManager alive within the session

        LoadHighScore();
        UpdateUI();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateHighScore();
        UpdateUI();
    }

    private void UpdateHighScore()
    {
        if (currentScore > sessionHighScore)
        {
            sessionHighScore = currentScore;
            PlayerPrefs.SetInt("HighScore", sessionHighScore); 
            PlayerPrefs.Save(); 
        }
    }

    private void LoadHighScore()
    {
        sessionHighScore = PlayerPrefs.GetInt("HighScore", 1000); 
    }

    private void UpdateUI()
    {
        scoreText.text = $"SCORE: {currentScore:D4}";
        highScoreText.text = $"HIGH SCORE: {sessionHighScore:D4}";
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }
}
