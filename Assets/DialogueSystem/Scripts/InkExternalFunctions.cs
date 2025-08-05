using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    public void Bind (Story story)
    {
        story.BindExternalFunction("ExitDialogue", ExitDialogue);
        story.BindExternalFunction("StartCollisionControl", StartCollisionControl);
        story.BindExternalFunction("StartByteRunner", StartByteRunner);
        story.BindExternalFunction("AwardCollisionControlBadge", AwardCollisionControlBadge);
        story.BindExternalFunction("AwardByteRunnerBadge", AwardByteRunnerBadge);
    }

    public void Unbind (Story story)
    {
        story.UnbindExternalFunction("ExitDialogue");
        story.UnbindExternalFunction("StartCollisionControl");
        story.UnbindExternalFunction("StartByteRunner");
        story.UnbindExternalFunction("AwardCollisionControlBadge");
        story.UnbindExternalFunction("AwardByteRunnerBadge");
    }

    private void ExitDialogue()
    {
        //EXIT DIALOGUE SCENE AND RETURN TO OVERWORLD

        Debug.Log("Exit Dialogue Called from Inky JSON");
    }

    private void StartCollisionControl()
    {
        //EXIT DIALOGUE SCENE AND GO TO 'COLLISION CONTROL' SCENE

        Debug.Log("StartCollisionControl Called from Inky JSON");
    }

    private void StartByteRunner()
    {
        //EXIT DIALOGUE SCENE AND GO TO 'BYTE RUNNER' SCENE

        Debug.Log("StartByteRunner Called from Inky JSON");
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
}
