using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public void MikeyTestPressed()
    {
        StartNPCDialogue(
            npcKey: "Mikey",
            introKnot: "mikeyDialogue.FirstIntroduction",
            returnKnot: "mikeyDialogue.ReturningIntroduction"
        );
    }

    public void SuzyTestPressed()
    {
        StartNPCDialogue(
            npcKey: "Suzy",
            introKnot: "suzyDialogue.FirstIntroduction",
            returnKnot: "suzyDialogue.ReturningIntroduction"
        );
    }

    private void StartNPCDialogue(string npcKey, string introKnot, string returnKnot)
    {
        bool returning = NPCInteractionTracker.Instance != null &&
                         NPCInteractionTracker.Instance.HasVisited(npcKey);

        string knot = returning ? returnKnot : introKnot;
        GameEventsManager.instance.dialogueEvents.EnterDialogue(knot);
    }
}
