using UnityEngine;
using UnityEngine.InputSystem;

// Handles player movement and emotion cycling
// Rigidbody2D.MovePosition is used so the physics colliders block movement correctly
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed         = 4f;
    [SerializeField] private float cameraPadding     = 0.5f;
    [SerializeField] private float walkSoundInterval = 0.3f;

    private Rigidbody2D rb;
    private Camera      cam;
    private EmotionType currentEmotion = EmotionType.Neutral;
    private float       walkSoundTimer = 0f;

    // Saved in Awake so LoopManager can teleport the player back here each loop
    [HideInInspector] public Vector3 startPosition;

    // Awake runs before any Start, so Rigidbody is ready when LoopManager.Start calls ResetToStart
    void Awake()
    {
        rb            = GetComponent<Rigidbody2D>();
        cam           = Camera.main;
        startPosition = transform.position;
    }

    void Update()
    {
        CycleEmotion();

        if (walkSoundTimer > 0f)
            walkSoundTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 dir = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)    dir += Vector2.up;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)  dir += Vector2.down;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  dir += Vector2.left;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) dir += Vector2.right;

        if (dir.magnitude > 0) dir.Normalize();

        // Footstep sound
        if (dir.sqrMagnitude > 0.01f && walkSoundTimer <= 0f)
        {
            AudioManager.Instance?.PlayWalk();
            walkSoundTimer = walkSoundInterval;
        }

        float   speed  = baseSpeed * EmotionState.GetSpeedMultiplier(currentEmotion);
        Vector2 newPos = rb.position + dir * speed * Time.fixedDeltaTime;

        rb.MovePosition(ClampToCamera(newPos));
    }

    // Keeps the player inside the camera's visible area
    private Vector2 ClampToCamera(Vector2 pos)
    {
        if (cam == null) return pos;

        float   halfH  = cam.orthographicSize - cameraPadding;
        float   halfW  = cam.orthographicSize * cam.aspect - cameraPadding;
        Vector2 centre = cam.transform.position;

        pos.x = Mathf.Clamp(pos.x, centre.x - halfW, centre.x + halfW);
        pos.y = Mathf.Clamp(pos.y, centre.y - halfH, centre.y + halfH);
        return pos;
    }

    // Cycle through emotions with E
    private void CycleEmotion()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            currentEmotion = (EmotionType)(((int)currentEmotion + 1) % 3);
    }

    public EmotionType GetEmotion() => currentEmotion;

    public void ResetToStart()
    {
        rb.position       = (Vector2)startPosition;
        rb.linearVelocity = Vector2.zero;
        currentEmotion    = EmotionType.Neutral;
    }
}
