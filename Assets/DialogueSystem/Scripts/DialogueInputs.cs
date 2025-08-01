using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputs : MonoBehaviour
{
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
    //    {
    //        DialogueInputPressed();
    //    }
    //}

    public void DialogueConfirmPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ConfirmSubmit();
        }
    }

    private IEnumerator ConfirmSubmit()
    {
        //Wait till end of frame so dialogue choice can register correctly before the submit button is pressed from same input
        yield return new WaitForEndOfFrame();

        GameEventsManager.instance.dialogueEvents.SubmitPressed();
    }
}
