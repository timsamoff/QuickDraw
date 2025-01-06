using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider countDownIndicator; // Countdown bar
    [SerializeField] private TextMeshProUGUI playerScoreText; // Player score UI
    [SerializeField] private TextMeshProUGUI aiScoreText; // AI score UI
    [SerializeField] private Button drawButton;

    [Header("Game Settings")]
    [SerializeField] private float countdownDuration = 3f; // Countdown time
    [SerializeField] private float easyThreshold = 0.5f; // Easy AI threshold
    [SerializeField] private float mediumThreshold = 0.3f; // Medium AI threshold
    [SerializeField] private float difficultThreshold = 0.1f; // Difficult AI threshold
    [SerializeField] private float drawThreshold = 0.1f; // Threshold to allow player to draw before slider hits 0

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Difficult
    }

    [SerializeField] private DifficultyLevel selectedDifficulty = DifficultyLevel.Medium;

    /* [Header("Other Settings")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator aiAnimator;
    [SerializeField] private AudioSource sfx; */

    private float currentProgress; // Tracks countdown progress
    private float aiThreshold; // Threshold based on difficulty
    private float playerDrawTime = -1f; // Time when player draws (-1 indicates no draw yet)
    private float aiDrawTime = -1f; // Time when AI draws (-1 indicates no draw yet)
    private bool gameActive = false; // Controls game state
    private int playerScore = 0;
    private int aiScore = 0;

    private void Start()
    {
        drawButton.onClick.AddListener(OnPlayerDraw);
        SetDifficulty(selectedDifficulty); // Set difficulty from the dropdown menu
        ResetGame();
    }

    private void Update()
    {
        if (gameActive)
        {
            Countdown();
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                aiThreshold = easyThreshold;
                break;
            case DifficultyLevel.Medium:
                aiThreshold = mediumThreshold;
                break;
            case DifficultyLevel.Difficult:
                aiThreshold = difficultThreshold;
                break;
        }

        Debug.Log("Difficulty set to: " + difficulty);
    }

    private void Countdown()
    {
        if (currentProgress > 0)
        {
            currentProgress -= Time.deltaTime / countdownDuration;
            countDownIndicator.value = currentProgress;
        }

        // Allow the player to draw before the slider reaches 0
        if (currentProgress <= drawThreshold && playerDrawTime == -1f && gameActive)
        {
            OnPlayerDraw();
        }

        // AI will only draw after the slider reaches 0 (not before)
        if (currentProgress <= 0 && aiDrawTime == -1f && gameActive)
        {
            AI_Draw();
        }

        // Only when the slider reaches 0 do we determine the winner
        if (currentProgress <= 0 && !IsWinnerDetermined())
        {
            DetermineWinner();
        }
    }

    private void OnPlayerDraw()
    {
        if (!gameActive || IsWinnerDetermined()) return;

        playerDrawTime = currentProgress; // Record the player's draw time
        // Don't reset the game here; just stop the countdown when the player draws
    }

    private void AI_Draw()
    {
        if (IsWinnerDetermined()) return;

        // Simulate AI drawing as close to 0 as possible within its threshold range
        aiDrawTime = Mathf.Max(0, currentProgress - Random.Range(0, aiThreshold));
    }

    private void DetermineWinner()
    {
        gameActive = false; // Stop the game loop

        // Ensure the slider reaches 0 before determining the winner
        currentProgress = 0;

        float playerDifference = Mathf.Abs(playerDrawTime);
        float aiDifference = Mathf.Abs(aiDrawTime);

        if (playerDifference < aiDifference)
        {
            PlayerWins();
        }
        else
        {
            AIWins();
        }
    }

    private void PlayerWins()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        Debug.Log("Player Wins! New Score: " + playerScore);
        ResetGame(); // Reset the game after the player wins
    }

    private void AIWins()
    {
        aiScore++;
        aiScoreText.text = aiScore.ToString();
        Debug.Log("AI Wins! AI Score: " + aiScore);
        ResetGame(); // Reset the game after the AI wins
    }

    private void ResetGame()
    {
        currentProgress = 1f; // Reset countdown to 1
        countDownIndicator.value = currentProgress;
        playerDrawTime = -1f; // Reset player's draw time
        aiDrawTime = -1f; // Reset AI's draw time
        gameActive = true; // Reactivate game for the next round
    }

    private bool IsWinnerDetermined()
    {
        return playerDrawTime != -1f || aiDrawTime != -1f;
    }
}