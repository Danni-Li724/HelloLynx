using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CreditPanel : MonoBehaviour
{
    [Header("Panel Root")]
    [Tooltip("CanvasGroup on the root panel. This controls the overall fade.")]
    public CanvasGroup panelGroup;

    [Header("Slide-In Elements")]
    [Tooltip("Image/Rect that should slide DOWN from above into place (optional).")]
    public RectTransform imageFromTop;
    [Tooltip("Image/Rect that should slide UP from below into place (optional).")]
    public RectTransform imageFromBottom;

    [Header("Optional: Text (assign your own Text/TMP objects in the panel)")]
    [Tooltip("Optional – keep empty if you handle text yourself in the hierarchy.")]
    public Graphic[] textGraphicsToFade; // e.g., Text, TMP_Text (via TMP's Graphic component), logos, etc.

    [Header("Per-Panel Timings (optional)")]
    public bool overrideTimings = false;
    [Tooltip("Seconds the whole panel fades in.")]
    public float fadeIn = 0.6f;
    [Tooltip("Seconds each image takes to slide into position.")]
    public float slideDuration = 0.8f;
    [Tooltip("Seconds the panel stays fully visible after slides finish.")]
    public float holdDuration = 2.5f;
    [Tooltip("Seconds the whole panel fades out.")]
    public float fadeOut = 0.5f;

    // Cached layout info
    private RectTransform _canvasRect;
    private Vector2 _topFinal;
    private Vector2 _bottomFinal;
    private bool _prepared;

    public void Prepare()
    {
        if (panelGroup == null)
        {
            var found = GetComponent<CanvasGroup>();
            if (found == null) found = gameObject.AddComponent<CanvasGroup>();
            panelGroup = found;
        }

        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null) _canvasRect = canvas.GetComponent<RectTransform>();

        // Cache "final" anchored positions (as placed by you in the editor)
        if (imageFromTop != null)    _topFinal    = imageFromTop.anchoredPosition;
        if (imageFromBottom != null) _bottomFinal = imageFromBottom.anchoredPosition;

        // Set initial invisible state
        panelGroup.alpha = 0f;
        SetGraphicsAlpha(textGraphicsToFade, 0f);

        // Move images off-screen from their respective directions
        MoveImagesToOffscreen();

        // Make sure panel is enabled
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        _prepared = true;
    }
    public IEnumerator Play(float fadeInDur, float slideDur, float holdDur, float fadeOutDur, AnimationCurve slideCurve)
    {
        if (!_prepared) Prepare();

        // Fade in the panel while sliding images
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, fadeInDur);
            float a = Mathf.Clamp01(t);
            panelGroup.alpha = a;
            SetGraphicsAlpha(textGraphicsToFade, a);
            yield return null;
        }

        // Slide-in images concurrently
        float timer = 0f;
        while (timer < slideDur)
        {
            timer += Time.deltaTime;
            float u = Mathf.Clamp01(timer / Mathf.Max(0.0001f, slideDur));
            float eased = slideCurve != null ? slideCurve.Evaluate(u) : u;

            if (imageFromTop != null)
            {
                Vector2 start = GetTopOffscreen(_topFinal);
                imageFromTop.anchoredPosition = Vector2.Lerp(start, _topFinal, eased);
            }

            if (imageFromBottom != null)
            {
                Vector2 start = GetBottomOffscreen(_bottomFinal);
                imageFromBottom.anchoredPosition = Vector2.Lerp(start, _bottomFinal, eased);
            }

            yield return null;
        }

        // Hold on screen
        if (holdDur > 0f)
            yield return new WaitForSeconds(holdDur);

        // Fade out the whole panel
        float t2 = 0f;
        while (t2 < 1f)
        {
            t2 += Time.deltaTime / Mathf.Max(0.0001f, fadeOutDur);
            float a = 1f - Mathf.Clamp01(t2);
            panelGroup.alpha = a;
            SetGraphicsAlpha(textGraphicsToFade, a);
            yield return null;
        }

        // Reset state so we can replay if needed (optional)
        panelGroup.alpha = 0f;
        SetGraphicsAlpha(textGraphicsToFade, 0f);
        MoveImagesToOffscreen();
    }
    private void MoveImagesToOffscreen()
    {
        if (imageFromTop != null)
            imageFromTop.anchoredPosition = GetTopOffscreen(_topFinal);

        if (imageFromBottom != null)
            imageFromBottom.anchoredPosition = GetBottomOffscreen(_bottomFinal);
    }
    private Vector2 GetTopOffscreen(Vector2 finalPos)
    {
        float canvasH = _canvasRect != null ? _canvasRect.rect.height : Screen.height;
        // Push far enough above to be clearly off screen (1x canvas height is safe)
        return finalPos + new Vector2(0f, canvasH + 200f);
    }
    private Vector2 GetBottomOffscreen(Vector2 finalPos)
    {
        float canvasH = _canvasRect != null ? _canvasRect.rect.height : Screen.height;
        return finalPos - new Vector2(0f, canvasH + 200f);
    }
    private void SetGraphicsAlpha(Graphic[] graphics, float a)
    {
        if (graphics == null) return;
        for (int i = 0; i < graphics.Length; i++)
        {
            if (graphics[i] == null) continue;
            var c = graphics[i].color;
            c.a = a;
            graphics[i].color = c;
        }
    }
}

