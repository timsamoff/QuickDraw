using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] private float countdownDuration = 3f;
    [SerializeField] private float drawDuration = 1f; // This is the draw duration
    [SerializeField] private Slider countdownIndicator;

    private float countdownTimer;
    private float drawTimer;
    private bool isDrawPhase;

    public float DrawTimer => drawTimer;  // Accessor for drawTimer
    public float DrawDuration => drawDuration;  // Accessor for drawDuration
    public bool IsDrawPhase => isDrawPhase;

    private void Start()
    {
        ResetCountdown();
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

    public void ResetCountdown()
    {
        countdownTimer = countdownDuration;
        countdownIndicator.value = 1;
        isDrawPhase = false;
    }

    private void CountdownPhase()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            countdownIndicator.value = countdownTimer / countdownDuration;
        }
        else
        {
            isDrawPhase = true;
            drawTimer = 0;
        }
    }

    private void DrawPhase()
    {
        if (drawTimer < drawDuration)
        {
            drawTimer += Time.deltaTime;
        }
    }

    public void ResetDrawTimer()
    {
        drawTimer = 0;
    }
}
