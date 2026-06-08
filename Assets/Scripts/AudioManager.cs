using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOverMusic;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        EnsureAudioSources();
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayShoot()
    {
        PlayEffect(shootClip);
    }

    public void PlayEnemyDeath()
    {
        PlayEffect(enemyDeathClip);
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic, true);
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic, false);
    }

    private void EnsureAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (effectsSource == null)
        {
            effectsSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        effectsSource.playOnAwake = false;
    }

    private void PlayEffect(AudioClip clip)
    {
        if (clip != null && effectsSource != null)
        {
            effectsSource.PlayOneShot(clip);
        }
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
}
