using System.Collections.Generic;
using UnityEngine;

// A single frame of player data captured during a recording
public struct LoopFrame
{
    public Vector3     position;
    public EmotionType emotion;
}

// Captures the player's movement into a list of frames when recording is active
// Recording is triggered manually via LoopManager when the player presses Space
public class GhostRecorder : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private List<LoopFrame> frames    = new List<LoopFrame>();
    private bool            recording = false;

    public bool IsRecording => recording;

    // FixedUpdate keeps recording in sync with GhostPlayback
    void FixedUpdate()
    {
        if (!recording) return;

        frames.Add(new LoopFrame
        {
            position = transform.position,
            emotion  = playerController.GetEmotion()
        });
    }

    // Called at the start of each loop to clear old data without starting a new recording
    public void PrepareForLoop()
    {
        frames.Clear();
        recording = false;
    }

    // Clears any previous recording and starts a fresh one
    public void RestartRecording()
    {
        frames.Clear();
        recording = true;
    }

    public void StopRecording() => recording = false;

    // Returns a copy of the current frames without stopping the recording
    public List<LoopFrame> GetFrames() => new List<LoopFrame>(frames);

    // Stops recording and returns whatever was captured
    public List<LoopFrame> StopAndGetFrames()
    {
        recording = false;
        return new List<LoopFrame>(frames);
    }
}
