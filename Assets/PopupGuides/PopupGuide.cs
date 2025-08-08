using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PopupGuide : MonoBehaviour
{
    //[SerializeField] private Image nextIcon;
    //[SerializeField] private Image closeIcon;

    [SerializeField] private Button cornerButton;
    [SerializeField] private GameObject nextGuide;

    private void OnEnable()
    {
        if (nextGuide == null)
        {
            //cornerButton.image = closeIcon;
            cornerButton.image.color = Color.red;
        }
        else
        {
            //cornerButton.image = nextIcon;
            cornerButton.image.color = Color.orange;
        }
    }


    public void ClosePopup()
    {
        if (nextGuide != null)
        {
            nextGuide.SetActive(true);

            Debug.Log("Next popup enabled");

            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("End of popups");

            this.gameObject.SetActive(false);
        }
    }
}
