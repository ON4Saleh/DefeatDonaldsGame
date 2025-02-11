using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", false);
    }
}
