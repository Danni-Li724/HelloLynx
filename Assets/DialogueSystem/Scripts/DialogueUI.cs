using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Parsed;
using Ink.Runtime;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;
using JetBrains.Annotations;

public class DialogueUI : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;
    [SerializeField] private GameObject continueIcon;

    //[SerializeField] private Button submitButton;
    //[SerializeField] private GameObject dialoguePanel;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private List<Ink.Runtime.Choice> currentDialogueChoices; 

    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void Start()
    {
        GameEventsManager.instance.dialogueEvents.onDialogueStarted += DialogueStarted;
        GameEventsManager.instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onDialogueStarted -= DialogueStarted;
        GameEventsManager.instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStarted()
    {
        contentParent.SetActive(true);
    }

    private void DialogueFinished()
    {
        contentParent.SetActive(false);

        //reset dialogue text
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Ink.Runtime.Choice> dialogueChoices)
    {
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }
        displayLineCoroutine = StartCoroutine(DisplayLine((dialogueLine)));

        //Check if there are too many choices coming in and set error if so (if there is more choices in ink file than buttons in UI)
        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices (" + dialogueChoices.Count + ") came through than are supported (" + choiceButtons.Length + ").");
        }

        //Start by setting all choice buttons off
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        currentDialogueChoices = dialogueChoices;
    }

    private void DisplayChoices(List<Ink.Runtime.Choice> dialogueChoices)
    {
        //Enable and set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Ink.Runtime.Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                GameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(0);
            }

            choiceButtonIndex--;
        }
    }

    private void HideChoices()
    {
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

    }

    private void CanContinueToNextLine(bool choice)
    {
        canContinueToNextLine = choice;
        GameEventsManager.instance.dialogueEvents.CanContinueToNextLine(choice);
    }

    private IEnumerator DisplayLine(string line)
    {
        //clear text so previous line is no longer showing
        dialogueText.text = "";

        //hide interactable UI items while text is typing
        continueIcon.SetActive(false);
        HideChoices();

        CanContinueToNextLine(false);

        //display each letter of new line one at a time
        foreach (char letter in line.ToCharArray())
        {
            //if (InputManager.GetInstance().GetSubmitPressed))
            //{
            //    dialogueText.text = line;
            //    break;
            //}

            //THE CODE ABOVE WILL SKIP THE TYPING PROCESS AND INSTANTLY COMPLETE THE CURRENT LINE, IT RELIES ON THEIR BEING AN INSTANCED INPUT MANAGER SCRIPT IN THE PROJECT THAT CAN RETURN A BOOL WHEN THE SUBMIT INPUTEVENT IS PRESSED.

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        //Everything below this will happen after the entire dialogue line has finished typing

        DisplayChoices(currentDialogueChoices);

        if (choiceButtons[0].gameObject.activeSelf == false)
        {
            continueIcon.SetActive(true);
        }
        
        CanContinueToNextLine(true);
    }

    private void ResetPanel()
    {
        dialogueText.text = "";
    }

    //INPUT FOR PROGRESSING THROUGH DIALOGUE, THIS WILL BE REPLACED BY USE OF UNITY INPUTSYSTEM (LEFT MOUSE CLICK, ENTER/RETURN, SPACEBAR)
    public void SubmitPressed()
    {
        GameEventsManager.instance.dialogueEvents.SubmitPressed();
    }


}
