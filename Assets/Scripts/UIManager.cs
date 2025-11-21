using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboCountText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    public void UpdateScore(int score, int combo)
    {
        if (scoreText) scoreText.text = $"Score: {score}";
        if (comboCountText) comboCountText.text = $"Score: x{combo}";
    }

    public void ShowMatchEffect(Vector3 worldPos)
    {
        // optional: spawn a small floating text or particle; left minimal for prototype
    }

    public void ShowGameOver(int finalScore)
    {
        // optional: show a panel
        gameOverPanel.SetActive(true);
        if (finalScoreText) finalScoreText.text = $"Score: {finalScore}";
    }
}
