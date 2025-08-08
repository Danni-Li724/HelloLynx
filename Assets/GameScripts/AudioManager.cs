using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource footstepSource;
    public AudioSource collisionSource;
    public AudioSource cpuSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip footstepSound;
    public AudioClip collisionMusic;
    public AudioClip cpuMusic;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume;
    [Range(0f, 1f)]
    public float footStepsVolume = 1f;
    
    public static AudioManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    private void InitializeAudio()
    {
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            backgroundMusicSource.loop = true;
            backgroundMusicSource.playOnAwake = false;
        }

        if (footstepSource == null)
        {
            footstepSource = gameObject.AddComponent<AudioSource>();
            footstepSource.loop = true;
            footstepSource.playOnAwake = false;
        }

        if (collisionSource == null)
        {
            collisionSource = gameObject.AddComponent<AudioSource>();
            collisionSource.loop = true;
            collisionSource.playOnAwake = false;
        }

        if (cpuSource == null)
        {
            cpuSource = gameObject.AddComponent<AudioSource>();
            cpuSource.loop = true;
            cpuSource.playOnAwake = false;
        }

        backgroundMusicSource.volume = musicVolume;
            footstepSource.volume = footStepsVolume;
            footstepSource.clip = footstepSound;
    }
    
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.Play();
        }
    }
    
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }
    public void PlayCollisionMusic()
    {
        if (collisionMusic != null && collisionSource != null)
        {
            collisionSource.clip = collisionMusic;
            collisionSource.Play();
        }
    }

    public void PlayCPUMusic()
    {
        if (cpuMusic != null && cpuSource != null)
        {
            cpuSource.clip = cpuMusic;
            cpuSource.Play();
        }

    }

    public void PlayFootsteps()
    {
        if (footstepSound != null && footstepSource != null && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }
    
    public void PlaySound(AudioClip clip)
    {
        if (clip != null )//&& xSource != null)
        {
            footstepSource.PlayOneShot(clip);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = musicVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        footStepsVolume = Mathf.Clamp01(volume);
        if (footstepSource != null)
        {
            footstepSource.volume = footStepsVolume;
        }
    }
    
    // Fade in/out methods for smooth transitions
    public void FadeInMusic(float duration = 1f)
    {
        StartCoroutine(FadeAudio(backgroundMusicSource, 0f, musicVolume, duration));
    }
    
    public void FadeOutMusic(float duration = 1f)
    {
        StartCoroutine(FadeAudio(backgroundMusicSource, musicVolume, 0f, duration));
    }
    
    private System.Collections.IEnumerator FadeAudio(AudioSource source, float startVolume, float targetVolume, float duration)
    {
        float elapsedTime = 0f;
        source.volume = startVolume;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }
        
        source.volume = targetVolume;
    }
}