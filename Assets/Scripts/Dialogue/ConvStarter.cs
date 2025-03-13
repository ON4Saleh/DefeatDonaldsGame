using UnityEngine;
using DialogueEditor;
public class ConvStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation npcconv;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
               ConversationManager.Instance.StartConversation(npcconv);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
