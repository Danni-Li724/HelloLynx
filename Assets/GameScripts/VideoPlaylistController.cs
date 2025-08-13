using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[DisallowMultipleComponent]
public class VideoPlaylistController : MonoBehaviour
{
    public enum SourceType { VideoClips, Urls }

    [Header("Components")]
    public VideoPlayer videoPlayer;
    [Tooltip("Optional, only if your videos have audio tracks you want to hear.")]
    public AudioSource audioSource;

    [Header("Output")]
    [Tooltip("RenderTexture the VideoPlayer should write into.")]
    public RenderTexture outputTexture;

    [Header("Playlist")]
    public SourceType sourceType = SourceType.VideoClips;
    [Tooltip("Assign your 10 VideoClips here if using local assets.")]
    public List<VideoClip> clips = new List<VideoClip>();
    [Tooltip("Or use absolute/relative URLs (StreamingAssets etc.) if SourceType is Urls.")]
    public List<string> urls = new List<string>();
    [Tooltip("Optional titles. If empty/missing, uses clip.name or file name from URL.")]
    public List<string> titles = new List<string>();

    [Header("Behaviour")]
    public bool playOnStart = true;
    [Tooltip("If true, automatically advance to the next video when one finishes.")]
    public bool autoAdvanceOnEnd = false;
    [Tooltip("If true, previous/next wrap around the ends of the list.")]
    public bool loopPlaylist = true;

    [Header("Events")]
    public UnityEvent<string> onTitleChanged;

    private int index;
    private bool isPrepared;

    private void Reset()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        // videoPlayer.isLooping = false; // we handle playlist looping ourselves
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = outputTexture;

        videoPlayer.audioOutputMode = (audioSource != null)
            ? VideoAudioOutputMode.AudioSource
            : VideoAudioOutputMode.None;

        videoPlayer.loopPointReached += OnLoopPointReached;
        videoPlayer.prepareCompleted += OnPrepared;
    }

    private void Start()
    {
        if (playOnStart)
            PlayIndex(0);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnLoopPointReached;
            videoPlayer.prepareCompleted  -= OnPrepared;
        }
    }

    // --- Public API ---

    public void Next()
    {
        PlayIndex(index + 1);
    }

    public void Previous()
    {
        PlayIndex(index - 1);
    }

    public void PlayIndex(int desired)
    {
        int count = GetCount();
        if (count == 0)
        {
            Debug.LogWarning("[VideoPlaylistController] Playlist is empty.");
            return;
        }

        if (loopPlaylist)
        {
            if (desired < 0)
                desired = (desired % count + count) % count; // wrap negatives
            else
                desired = desired % count;
        }
        else
        {
            desired = Mathf.Clamp(desired, 0, count - 1);
        }

        index = desired;

        // Stop current and prepare the next
        videoPlayer.Stop();
        isPrepared = false;

        if (sourceType == SourceType.VideoClips)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip   = clips[index];
        }
        else
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url    = urls[index];
        }
        
        if (audioSource != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

            // Ensure track 0 is enabled and routed to our AudioSource.
            // (audioTrackCount may still be 0 until prepared on some platforms, so we also do this again in OnPrepared)
            try
            {
                videoPlayer.EnableAudioTrack(0, true);
                videoPlayer.SetTargetAudioSource(0, audioSource);
            }
            catch { /* safe to ignore pre-prepare errors; OnPrepared will retry */ }

            // Make sure your AudioSource is 2D and unmuted
            audioSource.spatialBlend = 0f;
            audioSource.mute = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }
        else
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        }

        UpdateTitle();
        videoPlayer.Prepare(); // async; we'll start playback in OnPrepared
    }

    public string GetCurrentTitle()
    {
        return MakeTitleForIndex(index);
    }

    // --- Internals ---

    private int GetCount()
    {
        return (sourceType == SourceType.VideoClips) ? clips.Count : urls.Count;
    }

    private void OnPrepared(VideoPlayer vp)
    {
        isPrepared = true;
        vp.Play();

        if (audioSource != null)
            audioSource.Play();
    }

    private void OnLoopPointReached(VideoPlayer vp)
    {
        if (autoAdvanceOnEnd)
            Next();
    }

    private void UpdateTitle()
    {
        string t = MakeTitleForIndex(index);
        if (onTitleChanged != null)
            onTitleChanged.Invoke(t);
    }

    private string MakeTitleForIndex(int i)
    {
        if (titles != null && i < titles.Count && !string.IsNullOrEmpty(titles[i]))
            return titles[i];

        if (sourceType == SourceType.VideoClips)
        {
            var c = (i < clips.Count) ? clips[i] : null;
            if (c != null) return c.name;
        }
        else
        {
            var u = (i < urls.Count) ? urls[i] : null;
            if (!string.IsNullOrEmpty(u)) return Path.GetFileNameWithoutExtension(u);
        }

        return "Video " + (i + 1);
    }
}

