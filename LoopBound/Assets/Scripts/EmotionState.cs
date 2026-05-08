using UnityEngine;

// The three emotion states the player can cycle through using E
// Shared across PlayerController, GhostRecorder, and GhostPlayback
public enum EmotionType { Neutral, Joy, Sad }

// Static helper so any script can get a speed value or HUD label
// without needing a reference to a GameObject
public static class EmotionState
{
    // Joy speeds the player up, Sad slows them down
    public static float GetSpeedMultiplier(EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Joy: return 1.6f;
            case EmotionType.Sad: return 0.5f;
            default:              return 1.0f;
        }
    }

    public static string GetLabel(EmotionType emotion)
    {
        switch (emotion)
        {
            case EmotionType.Joy: return "Joy";
            case EmotionType.Sad: return "Sad";
            default:              return "Neutral";
        }
    }
}
