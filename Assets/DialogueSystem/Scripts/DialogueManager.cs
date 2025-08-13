using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem.XR.Haptics;
using System.Runtime.CompilerServices;

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager Instance { get; private set; }
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;
    private int currentChoiceIndex = -1;

    private bool canContinueToNextLine = false;
    private bool dialoguePlaying = false;

    private InkExternalFunctions inkExternalFunctions;
    private string currentNpcKey;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        story = new Story(inkJson.text);
        inkExternalFunctions = new InkExternalFunctions();
        inkExternalFunctions.Bind(story);

    }

    private void OnDestroy()
    {
        if (Instance == this && inkExternalFunctions != null && story != null)
            inkExternalFunctions.Unbind(story);
    }

    private void Start()
    {
        GameEventsManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        GameEventsManager.instance.dialogueEvents.onSubmitPressed += SubmitPressed;
        GameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
        GameEventsManager.instance.dialogueEvents.onCanContinueToNextLine += CanContinueToNextLine;
        GameEventsManager.instance.dialogueEvents.onDialogueStarted += HandleDialogueStarted;
        GameEventsManager.instance.dialogueEvents.onDialogueFinished += HandleDialogueFinished;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null && GameEventsManager.instance.dialogueEvents != null)
        {
            GameEventsManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
            GameEventsManager.instance.dialogueEvents.onSubmitPressed -= SubmitPressed;
            GameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
            GameEventsManager.instance.dialogueEvents.onCanContinueToNextLine -= CanContinueToNextLine;
            GameEventsManager.instance.dialogueEvents.onDialogueStarted -= HandleDialogueStarted;
            GameEventsManager.instance.dialogueEvents.onDialogueFinished -= HandleDialogueFinished;
        }
    }
    
    
    public void SetCurrentNPC(string npcKey) 
    {
        currentNpcKey = npcKey;
        // Debug.Log($"DialogueManager: current NPC set to {currentNpcKey}");
    }
    private void HandleDialogueStarted()
    {
        if (PlayerInputHandler.Instance)
            PlayerInputHandler.Instance.SetMovementEnabled(false);
    }

    private void HandleDialogueFinished() 
    {
        if (PlayerInputHandler.Instance)
            PlayerInputHandler.Instance.SetMovementEnabled(true);
        if (!string.IsNullOrWhiteSpace(currentNpcKey))
        {
            NPCInteractionTracker.Instance?.MarkVisited(currentNpcKey);
            // Debug.Log($"Marked visited on dialogue finish: {currentNpcKey}");
            currentNpcKey = null; // reset
        }
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    private void SubmitPressed()
    {
        if (!dialoguePlaying)
        {
            return;
        }

        if (canContinueToNextLine)
        {
            ContinueOrExitStory();
        }  
    }

    private void EnterDialogue(string knotName)
    {
        // if (dialoguePlaying)
        // {
        //     return;
        // }
        //
        // dialoguePlaying = true;
        //
        // GameEventsManager.instance.dialogueEvents.DialogueStarted();
        //
        // //freeze player movement (TO BE IMPLEMENTED)
        //
        // if (!knotName.Equals(""))
        // {
        //     story.ChoosePathString(knotName);
        // }
        // else
        // {
        //     Debug.LogWarning("Knot name was the empty string when entering dialogue");
        // }
        //
        // ContinueOrExitStory();
        dialoguePlaying = true;
        GameEventsManager.instance.dialogueEvents.DialogueStarted();

        if (!string.IsNullOrEmpty(knotName))
            story.ChoosePathString(knotName);
        else
            Debug.LogWarning("Knot name was empty when entering dialogue");

        ContinueOrExitStory();
    }

    private void CanContinueToNextLine(bool choice)
    {
        canContinueToNextLine = choice;
    }
    private void ContinueOrExitStory()
    {
        //make a choice (if applicable)
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);

            //reset choice index for next time
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            //if theres an empty line of dialogue that inky doesnt skip (error failsafe) continue until there is a line with content
            while (IsLineBlank(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            }
            //check if it is the last line that is blank
            if (IsLineBlank(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                GameEventsManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            }
        }

        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }
    private void ExitDialogue()
    {
        dialoguePlaying = false;

        GameEventsManager.instance.dialogueEvents.DialogueFinished();

        //unfreeze player movement (TO BE IMPLEMENTED)

        story.ResetState();
    }
    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }
}
