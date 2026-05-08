using UnityEngine;
using UnityEngine.UI;

// Handles all HUD updates. Gameplay scripts call methods here rather than touching UI components directly
public class GameUI : MonoBehaviour
{
    [SerializeField] private Text emotionText;
    [SerializeField] private Text loopCountText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text messageText;
    [SerializeField] private Text controlsHintText;
    [SerializeField] private PlayerController playerController;

    void Start()
    {
        if (controlsHintText != null)
            controlsHintText.text = "Press E to cycle emotions";
    }

    void Update()
    {
        if (emotionText != null && playerController != null)
            emotionText.text = "Emotion: " + EmotionState.GetLabel(playerController.GetEmotion());
    }

    public void UpdateLoopCount(int count)
    {
        if (loopCountText != null)
            loopCountText.text = "Loop: " + count;
    }

    public void UpdateTimer(float secondsLeft)
    {
        if (timerText == null) return;

        secondsLeft  = Mathf.Max(0f, secondsLeft);
        int minutes  = Mathf.FloorToInt(secondsLeft / 60f);
        int seconds  = Mathf.FloorToInt(secondsLeft % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void ShowMessage(string msg)
    {
        if (messageText == null) return;
        messageText.color = Color.white;
        messageText.text  = msg;
    }

    // Overload to show messages in a specific colour, used for the win message
    public void ShowMessage(string msg, Color colour)
    {
        if (messageText == null) return;
        messageText.color = colour;
        messageText.text  = msg;
    }
}
