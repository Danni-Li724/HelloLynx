using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingUI : MonoBehaviour
{
    [Header("UI Refs")]
    public GameObject uiRoot; // Parent UI GameObject to enable/disable
    public Text nameText; // Text component showing the name
    public string locationName;

    [Header("UI Float Settings")]
    public float floatStrength = 0.5f;
    public float duration = 1f;

    private Vector3 initialPos;

    private void Awake()
    {
        if (uiRoot != null)
            uiRoot.SetActive(false);

        if (nameText != null)
            nameText.text = "";

        initialPos = transform.localPosition;
    }

    private void Start()
    {
        transform.DOLocalMoveY(initialPos.y + floatStrength, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        PlayerDetector detector = GetComponent<PlayerDetector>();
        if (detector != null)
        {
            detector.OnPlayerEnter += ShowUI;
            detector.OnPlayerExit += HideUI;
        }
    }

    private void ShowUI()
    {
        if (uiRoot != null)
            uiRoot.SetActive(true);

        if (nameText != null)
            nameText.text = locationName;
    }

    private void HideUI()
    {
        if (uiRoot != null)
            uiRoot.SetActive(false);

        if (nameText != null)
            nameText.text = "";
    }
}