using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSequenceController : MonoBehaviour
{
    [Header("Sequence")]
    [Tooltip("Order matters. Each element is one screen/panel of credits.")]
    public List<CreditPanel> panels = new List<CreditPanel>();

    [Header("Default Timings (used unless a panel overrides)")]
    [Tooltip("Seconds the whole panel fades in.")]
    public float defaultFadeIn = 0.6f;
    [Tooltip("Seconds each image takes to slide into position.")]
    public float defaultSlideDuration = 0.8f;
    [Tooltip("Seconds the panel stays fully visible after slides finish.")]
    public float defaultHold = 2.5f;
    [Tooltip("Seconds the whole panel fades out.")]
    public float defaultFadeOut = 0.5f;

    [Header("Motion")]
    [Tooltip("Easing for slide-in (0..1).")]
    public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Flow")]
    [Tooltip("Name of the scene to load when all panels are done.")]
    public string nextSceneName = "";
    [Tooltip("If true, will load nextSceneName asynchronously in the background while credits play.")]
    public bool preloadNextScene = true;

    private AsyncOperation preloadOp;

    private void Start()
    {
        // Optional: Preload next scene to make the final transition snappy.
        if (preloadNextScene && !string.IsNullOrEmpty(nextSceneName))
        {
            preloadOp = SceneManager.LoadSceneAsync(nextSceneName);
            if (preloadOp != null) preloadOp.allowSceneActivation = false;
        }

        // Prepare all panels (cache positions, ensure correct initial states).
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null)
                panels[i].Prepare();
        }

        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            var panel = panels[i];
            if (panel == null)
                continue;

            float fadeIn = panel.overrideTimings ? panel.fadeIn : defaultFadeIn;
            float slide   = panel.overrideTimings ? panel.slideDuration : defaultSlideDuration;
            float hold    = panel.overrideTimings ? panel.holdDuration : defaultHold;
            float fadeOut = panel.overrideTimings ? panel.fadeOut : defaultFadeOut;

            yield return panel.Play(fadeIn, slide, hold, fadeOut, slideCurve);
        }

        // Transition to the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (preloadOp != null)
            {
                preloadOp.allowSceneActivation = true;
                // Let Unity switch scenes immediately now that activation is allowed.
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
