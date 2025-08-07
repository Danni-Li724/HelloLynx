using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputs : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(ConfirmSubmit());
        }
    }

    private IEnumerator ConfirmSubmit()
    {
        //Wait till end of frame so dialogue choice can register correctly before the submit button is pressed from same input
        yield return new WaitForEndOfFrame();
        GameEventsManager.instance.dialogueEvents.SubmitPressed();
    }
}
