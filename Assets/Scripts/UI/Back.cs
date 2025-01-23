using UnityEngine;

public class Back : MonoBehaviour
{
    public GameObject MainMenu; 
    public GameObject Options;  

    public void back()
    {
        MainMenu.SetActive(true);
        Options.SetActive(false);
    }
}
