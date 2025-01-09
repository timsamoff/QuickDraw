using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text aiScoreText;

    private int playerScore;
    private int aiScore;

    public void IncreasePlayerScore()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString("D4");
    }

    public void IncreaseAIScore()
    {
        aiScore++;
        aiScoreText.text = aiScore.ToString("D4");
    }

    public void ResetScores()
    {
        playerScore = 0;
        aiScore = 0;
        playerScoreText.text = playerScore.ToString("D4");
        aiScoreText.text = aiScore.ToString("D4");
    }
}