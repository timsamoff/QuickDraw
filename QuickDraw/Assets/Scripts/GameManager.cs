using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] private float countdownDuration = 3f;
    [SerializeField] private float drawDuration = 1f;

    [Header("UI Elements")]
    [SerializeField] private Slider countdownSlider;
    [SerializeField] private Button drawButton;
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text aiScoreText;

    [Header("Game Settings")]
    [SerializeField] private DifficultyLevel difficulty = DifficultyLevel.Medium;

    private float countdownTimer;
    private float drawTimer;
    private bool isDrawPhase;
    private bool hasWinner;
    private int playerScore;
    private int aiScore;

    private float aiDrawThreshold;

    private enum DifficultyLevel { Easy, Medium, Hard }

    private void Start()
    {
        ResetGame();
        SetDifficultyThreshold();
        drawButton.onClick.AddListener(PlayerDraw);
        drawButton.interactable = false;
    }

    private void Update()
    {
        if (!isDrawPhase)
        {
            CountdownPhase();
        }
        else
        {
            DrawPhase();
        }
    }

    private void CountdownPhase()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            countdownSlider.value = countdownTimer / countdownDuration;
        }
        else
        {
            isDrawPhase = true;
            drawTimer = 0;
            drawButton.interactable = true;
        }
    }

    private void DrawPhase()
    {
        if (drawTimer < drawDuration)
        {
            drawTimer += Time.deltaTime;

            // AI "draws"
            if (drawTimer >= aiDrawThreshold && aiDrawThreshold > 0 && !hasWinner)
            {
                // Debug.Log($"AI Draw Time: {drawTimer:F3} seconds");
                aiDrawThreshold = -1; // Prevent AI from drawing multiple times
                CheckWinner(drawTimer, "AI");
            }
        }
        else if (!hasWinner)
        {
            // No winner if neither player nor AI draws within the time
            CheckWinner(drawDuration, "None");
        }

        // Reset game once the draw phase completes
        if (drawTimer >= drawDuration)
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        isDrawPhase = false;
        hasWinner = false;
        countdownTimer = countdownDuration;
        countdownSlider.value = 1;
        drawButton.interactable = false;
        SetDifficultyThreshold();
    }

    private void SetDifficultyThreshold()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                aiDrawThreshold = Random.Range(0f, 0.8f);
                break;
            case DifficultyLevel.Medium:
                aiDrawThreshold = Random.Range(0f, 0.5f);
                break;
            case DifficultyLevel.Hard:
                aiDrawThreshold = Random.Range(0f, 0.3f);
                break;
            default:
                aiDrawThreshold = Random.Range(0f, 0.5f);
                break;
        }
    }

    private void PlayerDraw()
    {
        if (isDrawPhase && !hasWinner)
        {
            // Debug.Log($"Player Draw Time: {drawTimer:F3} seconds");
            CheckWinner(drawTimer, "Player");
        }
    }

    private void CheckWinner(float drawTime, string drawer)
    {
        hasWinner = true;

        float playerTime = drawer == "Player" ? drawTime : Mathf.Infinity;
        float aiTime = drawer == "AI" ? drawTime : aiDrawThreshold;

        if (drawer == "Player")
        {
            Debug.Log($"Player Draw Time: {drawTime:F3} seconds");
        }
        else
        {
            Debug.Log($"Player Draw Time: No draw");
        }

        if (drawer == "AI")
        {
            Debug.Log($"AI Draw Time: {aiTime:F3} seconds");
        }

        if (drawer == "Player")
        {
            playerScore++;
            playerScoreText.text = playerScore.ToString("D4");
        }
        else if (drawer == "AI")
        {
            aiScore++;
            aiScoreText.text = aiScore.ToString("D4");
        }
        else
        {
            // Draw or no winner?
        }

        // DrawPhase handles reset at end of timer
    }
}