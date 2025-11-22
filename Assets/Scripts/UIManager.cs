using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboCountText;

    public GameObject mainMenuPanel;
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    public void UpdateScore(int score, int combo)
    {
        if (scoreText)
            scoreText.text = $"Score: {score}";

        if (comboCountText)
            comboCountText.text = $"Combo: x{combo}";
    }

    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);
        if (finalScoreText)
            finalScoreText.text = $"Score: {finalScore}";
    }

    public void LoadGameIfAny()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }
}
