using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // Assign a source with Loop = true
    [SerializeField] private AudioSource sfxSource;   // Assign a source with Loop = false

    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // --- SFX METHODS ---

    public void PlayWin()
    {
        // Plays the sound once on top of the music
        if (winSound != null) 
            sfxSource.PlayOneShot(winSound);
    }

    public void PlayDeath()
    {
        // Plays the sound once on top of the music
        if (deathSound != null) 
            sfxSource.PlayOneShot(deathSound);
    }
}