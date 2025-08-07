using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    
    private SuzyMinigameController suzyMinigameController;
    private MikeyMinigameController mikeyMinigameController;
    public void Bind (Story story)
    {
        // Dialogue & minigame control
        story.BindExternalFunction("ExitDialogue", ExitDialogue);
        story.BindExternalFunction("StartCollisionControl", StartCollisionControl);
        story.BindExternalFunction("StartByteRunner", StartByteRunner);
        
        // Badge awards
        story.BindExternalFunction("AwardCollisionControlBadge", AwardCollisionControlBadge);
        story.BindExternalFunction("AwardByteRunnerBadge", AwardByteRunnerBadge);
        story.BindExternalFunction("AwardScreenBadge", AwardScreenBadge);
        story.BindExternalFunction("AwardRamBadge", AwardRamBadge);
        story.BindExternalFunction("AwardAudioBadge", AwardAudioBadge);
        story.BindExternalFunction("AwardPowerBadge", AwardPowerBadge);
    }

    public void Unbind (Story story)
    {
        // Dialogue & minigame control
        story.UnbindExternalFunction("ExitDialogue");
        story.UnbindExternalFunction("StartCollisionControl");
        story.UnbindExternalFunction("StartByteRunner");
        
        // Badge awards
        story.UnbindExternalFunction("AwardCollisionControlBadge");
        story.UnbindExternalFunction("AwardByteRunnerBadge");
        story.UnbindExternalFunction("AwardScreenBadge");
        story.UnbindExternalFunction("AwardRamBadge");
        story.UnbindExternalFunction("AwardAudioBadge");
        story.UnbindExternalFunction("AwardPowerBadge");
    }

    private void ExitDialogue()
    {
        //EXIT DIALOGUE SCENE AND RETURN TO OVERWORLD
        GameEventsManager.instance.dialogueEvents.DialogueFinished();
        Debug.Log("Exit Dialogue Called from Inky JSON");
    }

    private void StartCollisionControl()
    {
        //EXIT DIALOGUE SCENE AND GO TO 'COLLISION CONTROL' SCENE
        Debug.Log("StartCollisionControl Called from Inky JSON");
            suzyMinigameController = GameObject.FindObjectOfType<SuzyMinigameController>();
            if (!suzyMinigameController) return;
        if (suzyMinigameController)
            suzyMinigameController.BeginCollisionGame();
    }

    private void StartByteRunner()
    {
        //EXIT DIALOGUE SCENE AND GO TO 'BYTE RUNNER' SCENE
        Debug.Log("StartByteRunner Called from Inky JSON");
            mikeyMinigameController = GameObject.FindObjectOfType<MikeyMinigameController>();
            if (!mikeyMinigameController) return;
        if (mikeyMinigameController)
            mikeyMinigameController.BeginCPUGame();
    }

    private void AwardCollisionControlBadge()
    {
        //AWARD PLAYER BADGE FOR SUCCESSFULLY COMPLETING COLLISION CONTROL IN BADGE SYSTEM

        Debug.Log("AwardCollisionControlBadge Called from Inky JSON");
    }

    private void AwardByteRunnerBadge()
    {
        //AWARD PLAYER BADGE FOR SUCCESSFULLY COMPLETING BYTE RUNNER IN BADGE SYSTEM

        Debug.Log("AwardByteRunnnerBadge Called from Inky JSON");
    }
    
    private void AwardScreenBadge()
    {
        Debug.Log("AwardScreenBadge called from Ink.");
        BadgeInventory.Instance.EarnBadge(BadgeType.Screen);
    }

    private void AwardRamBadge()
    {
        Debug.Log("AwardRamBadge called from Ink.");
        BadgeInventory.Instance.EarnBadge(BadgeType.Ram);
    }

    private void AwardAudioBadge()
    {
        Debug.Log("AwardAudioBadge called from Ink.");
        BadgeInventory.Instance.EarnBadge(BadgeType.Audio);
    }

    private void AwardPowerBadge()
    {
        Debug.Log("AwardPowerBadge called from Ink.");
        BadgeInventory.Instance.EarnBadge(BadgeType.Power);
    }
}
