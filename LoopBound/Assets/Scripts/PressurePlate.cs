using UnityEngine;

// Tracks whether a tagged object (Player or Ghost) is standing on this plate
// GoalTrigger reads IsActive to decide when to unlock the door
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private string         requiredTag    = "Player";
    [SerializeField] private SpriteRenderer plateRenderer;
    [SerializeField] private Color          activeColour   = Color.green;
    [SerializeField] private Color          inactiveColour = Color.grey;

    public bool IsActive { get; private set; } = false;

    void Start() => UpdateVisual();

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag(requiredTag)) return;
        IsActive = true;
        UpdateVisual();
        AudioManager.Instance?.PlayPlateOn();
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag(requiredTag)) return;
        IsActive = false;
        UpdateVisual();
        AudioManager.Instance?.PlayPlateOff();
    }

    public void ResetPlate()
    {
        IsActive = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (plateRenderer != null)
            plateRenderer.color = IsActive ? activeColour : inactiveColour;
    }
}
