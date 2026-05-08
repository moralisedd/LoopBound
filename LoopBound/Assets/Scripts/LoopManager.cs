using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Controls the game loop: timer, ghost spawning, win and fail conditions
// The ghost and timer work independently. The ghost is a tool the player controls manually, the timer resets everything when it runs out
public class LoopManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GhostRecorder    ghostRecorder;
    [SerializeField] private GameObject       ghostPrefab;
    [SerializeField] private Transform        ghostSpawnPoint;
    [SerializeField] private GameUI           gameUI;
    [SerializeField] private GoalTrigger      goalTrigger;
    [SerializeField] private float            loopDuration = 120f;

    private float         loopTimer      = 0f;
    private int           loopCount      = 0;
    private bool          loopRunning    = false;
    private bool          waitingToStart = false;
    private GhostPlayback activeGhost;

    void Start()
    {
        gameUI.ShowMessage("SPACE to trace your path");
        BeginLoop();
    }

    void Update()
    {
        // After winning, wait for the player to press Space before restarting
        if (waitingToStart)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                waitingToStart = false;
                BeginLoop();
            }
            return;
        }

        if (!loopRunning) return;

        loopTimer += Time.deltaTime;
        gameUI.UpdateTimer(loopDuration - loopTimer);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            HandleTraceToggle();

        if (loopTimer >= loopDuration)
            TimerExpired();
    }

    // Space toggles recording on and off
    // Stopping the recording commits the path and spawns the ghost straight away
    // Starting a new recording removes the current ghost so it stays accurate
    private void HandleTraceToggle()
    {
        if (!ghostRecorder.IsRecording)
        {
            DespawnGhost();
            ghostRecorder.RestartRecording();
            gameUI.ShowMessage("Tracing...");
        }
        else
        {
            ghostRecorder.StopRecording();
            SpawnGhost(ghostRecorder.GetFrames());
            gameUI.ShowMessage("Path committed!");
            Invoke(nameof(ClearMessage), 1.5f);
        }
    }

    private void ClearMessage() => gameUI.ShowMessage("");

    // Full reset: player, ghost, plates, door, and timer
    // Called at game start and after winning
    public void BeginLoop()
    {
        loopCount++;
        loopTimer   = 0f;
        loopRunning = true;

        DespawnGhost();
        playerController.ResetToStart();
        ghostRecorder.PrepareForLoop();

        goalTrigger?.ResetDoor();
        goalTrigger?.ResetPlates();

        gameUI.UpdateLoopCount(loopCount);
        gameUI.UpdateTimer(loopDuration);
        gameUI.ShowMessage("SPACE to trace your path");
        AudioManager.Instance?.RestartMusic();
    }

    // Timer ran out: shows the fail message and reset everything after a short delay
    private void TimerExpired()
    {
        loopRunning = false;
        CancelInvoke(nameof(ClearMessage));
        ghostRecorder.StopAndGetFrames();

        gameUI.ShowMessage("You failed! :(");
        AudioManager.Instance?.PlayGameOver();
        Invoke(nameof(ResetAfterTimeout), 1.5f);
    }

    private void ResetAfterTimeout()
    {
        loopCount++;
        loopTimer   = 0f;
        loopRunning = true;

        DespawnGhost();
        playerController.ResetToStart();
        ghostRecorder.PrepareForLoop();

        goalTrigger?.ResetDoor();
        goalTrigger?.ResetPlates();

        gameUI.UpdateLoopCount(loopCount);
        gameUI.UpdateTimer(loopDuration);
        gameUI.ShowMessage("SPACE to trace your path");
        AudioManager.Instance?.RestartMusic();
    }

    // Called by GoalTrigger when the player walks through the unlocked door
    public void OnPlayerWin()
    {
        if (!loopRunning) return;
        loopRunning = false;

        CancelInvoke(nameof(ClearMessage));
        CancelInvoke(nameof(ResetAfterTimeout));
        DespawnGhost();

        gameUI.ShowMessage("You escaped the loop!  SPACE to play again", Color.green);
        AudioManager.Instance?.PlayGameWon();
        waitingToStart = true;
    }

    private void SpawnGhost(List<LoopFrame> frames)
    {
        if (ghostPrefab == null || frames.Count == 0) return;

        GameObject obj   = Instantiate(ghostPrefab, ghostSpawnPoint.position, Quaternion.identity);
        GhostPlayback ghost = obj.GetComponent<GhostPlayback>();

        if (ghost == null) return;
        activeGhost = ghost;
        ghost.LoadFrames(frames);
    }

    // Helper to avoid repeating the null check and Destroy call everywhere
    private void DespawnGhost()
    {
        if (activeGhost == null) return;
        activeGhost.Despawn();
        activeGhost = null;
    }
}
