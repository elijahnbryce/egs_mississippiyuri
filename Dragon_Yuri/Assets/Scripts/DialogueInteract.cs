using UnityEngine;
using Yarn.Unity;


public class DialogueInteract : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public void Interact()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue("tutorial");
        }
    }
}
