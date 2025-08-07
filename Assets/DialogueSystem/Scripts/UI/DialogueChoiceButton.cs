using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private Text choiceText;

    [Header("Text Colours")]
    [SerializeField] private Color defaultTextColour = Color.white;
    [SerializeField] private Color highlightedTextColour = Color.yellow;


    public int choiceIndex = -1;

    public void Update()
    {
    }

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectButton();

        //When the player starts hovering over this button, change the text colour to default (white)
        HighlightedTextColour();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //When the player stops hovering over this button, change the choice text colour to the default highlight colour (yellow)
        DefaultTextColour();
    }

    public void DefaultTextColour()
    {
        choiceText.color = defaultTextColour;
    }

    public void HighlightedTextColour()
    {
        choiceText.color = highlightedTextColour;
    }
}

