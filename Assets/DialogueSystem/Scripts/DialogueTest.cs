using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public void MikeyTestPressed()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("mikeyIntroduction");
    }

    public void SuzyTestPressed()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("suzyDialogue.FirstIntroduction");
    }
}
