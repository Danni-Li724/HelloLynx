using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class VideoPlaylistUI : MonoBehaviour
{
    [Header("References")]
    public VideoPlaylistController controller;
    public Button previousButton;
    public Button nextButton;
    public Text titleLabel; 

    private void Awake()
    {
        if (previousButton != null) previousButton.onClick.AddListener(OnPrevClicked);
        if (nextButton != null)     nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnEnable()
    {
        if (controller != null)
            controller.onTitleChanged.AddListener(UpdateTitle);
    }

    private void OnDisable()
    {
        if (controller != null)
            controller.onTitleChanged.RemoveListener(UpdateTitle);
    }

    private void OnPrevClicked()
    {
        if (controller != null) controller.Previous();
    }

    private void OnNextClicked()
    {
        if (controller != null) controller.Next();
    }

    private void UpdateTitle(string title)
    {
        if (titleLabel != null) titleLabel.text = title;
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("Motherboard");
    }
}