using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text descriptionText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        hide();
    }

    public void show(string descriotion)
    {
        descriptionText.text = descriotion;
        infoPanel.SetActive(true);
    }
    public void hide()
    { 
        if(infoPanel) infoPanel.SetActive(false);
    }
}
