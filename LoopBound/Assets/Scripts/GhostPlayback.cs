using System.Collections.Generic;
using UnityEngine;

// Replays a recorded list of frames on the ghost prefab.
// Steps through frames in FixedUpdate to match the rate GhostRecorder captured them.
public class GhostPlayback : MonoBehaviour
{
    private List<LoopFrame> frames;
    private int             frameIndex = 0;
    private bool            playing    = false;
    private Rigidbody2D     rb;

    public bool IsPresent { get; private set; } = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LoadFrames(List<LoopFrame> recordedFrames)
    {
        frames     = recordedFrames;
        frameIndex = 0;
        playing    = frames != null && frames.Count > 0;
        IsPresent  = playing;
    }

    void FixedUpdate()
    {
        if (!playing) return;

        // MovePosition triggers OnTriggerEnter2D on pressure plates correctly.
        if (rb != null)
            rb.MovePosition(frames[frameIndex].position);
        else
            transform.position = frames[frameIndex].position;

        frameIndex++;

        // Stop on the final frame rather than looping so the ghost holds its position.
        // This is what allows it to keep a pressure plate active indefinitely.
        if (frameIndex >= frames.Count)
        {
            frameIndex = frames.Count - 1;
            playing    = false;
        }
    }

    public void Despawn()
    {
        IsPresent = false;
        Destroy(gameObject);
    }
}
