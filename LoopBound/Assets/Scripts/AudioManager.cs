using UnityEngine;

// Handles all game audio from one place
// Uses two AudioSources- one for looping background music, one for sound effects
// Other scripts call AudioManager.Instance.Play___() without needing AudioSource references
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip plateOnSound;
    [SerializeField] private AudioClip plateOffSound;
    [SerializeField] private AudioClip gameWonSound;
    [SerializeField] private AudioClip gameOverSound;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() => PlayMusic();

    // PlayOneShot lets sounds overlap without cutting each other off
    public void PlayWalk()     { if (walkSound    != null) sfxSource.PlayOneShot(walkSound);    }
    public void PlayPlateOn()  { if (plateOnSound  != null) sfxSource.PlayOneShot(plateOnSound); }
    public void PlayPlateOff() { if (plateOffSound != null) sfxSource.PlayOneShot(plateOffSound); }

    public void PlayGameWon()
    {
        musicSource.Stop();
        if (gameWonSound != null) sfxSource.PlayOneShot(gameWonSound);
    }

    public void PlayGameOver()
    {
        musicSource.Stop();
        if (gameOverSound != null) sfxSource.PlayOneShot(gameOverSound);
    }

    public void RestartMusic() => PlayMusic();

    private void PlayMusic()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}
