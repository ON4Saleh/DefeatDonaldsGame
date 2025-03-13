using DialogueEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private NPCConversation npcconv;

    public void StartDialogue()
    {
        if (npcconv != null)
        {
            ConversationManager.Instance.StartConversation(npcconv);
            Debug.Log("Dialogue Started!");
        }
        else
        {
            Debug.LogWarning("No NPCConversation assigned!");
        }
    }
}
