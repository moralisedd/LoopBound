using UnityEngine;

// Manages the exit door. Once both plates are active at the same time,
// the door unlocks permanently for that loop.
// LoopManager resets it on fail or win so the next loop starts fresh.
public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private LoopManager    loopManager;
    [SerializeField] private PressurePlate  plateA;
    [SerializeField] private PressurePlate  plateB;
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Color          unlockedColour = Color.yellow;
    [SerializeField] private Color          lockedColour   = Color.white;

    public bool DoorUnlocked { get; private set; } = false;

    void Start() => UpdateVisual();

    void Update()
    {
        if (DoorUnlocked) return;

        if (plateA != null && plateA.IsActive && plateB != null && plateB.IsActive)
        {
            DoorUnlocked = true;
            UpdateVisual();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        if (DoorUnlocked) loopManager.OnPlayerWin();
    }

    public void ResetDoor()
    {
        DoorUnlocked = false;
        UpdateVisual();
    }

    public void ResetPlates()
    {
        plateA?.ResetPlate();
        plateB?.ResetPlate();
    }

    private void UpdateVisual()
    {
        if (doorRenderer != null)
            doorRenderer.color = DoorUnlocked ? unlockedColour : lockedColour;
    }
}
