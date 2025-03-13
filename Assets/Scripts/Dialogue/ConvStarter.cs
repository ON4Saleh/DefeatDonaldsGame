using UnityEngine;
using DialogueEditor;

public class ConvStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation npcconv;
    private bool isDialogueActive = false;

    private void Start()
    {
        ConversationManager.OnConversationEnded += EndDialogue;
    }

    public void StartDialogue()
    {
        if (isDialogueActive) return;

        if (npcconv != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ConversationManager.Instance.StartConversation(npcconv);
            isDialogueActive = true;
            Debug.Log("Dialogue Started!");
        }
        else
        {
            Debug.LogWarning("No NPCConversation assigned!");
        }
    }

    public void StopDialogue()
    {
        if (isDialogueActive)
        {
            ConversationManager.Instance.EndConversation();
            isDialogueActive = false;
            Debug.Log("Dialogue Stopped!");
        }
    }

    private void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isDialogueActive = false;
        Debug.Log("Dialogue Ended!");
    }

    private void OnDestroy()
    {
        ConversationManager.OnConversationEnded -= EndDialogue;
    }
}
