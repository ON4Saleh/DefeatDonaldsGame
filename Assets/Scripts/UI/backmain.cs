using UnityEngine;
using UnityEngine.SceneManagement;

public class backmain : MonoBehaviour
{
    public void playerController()
    {
        SceneManager.LoadSceneAsync("PlayerControls");
    }
    public void back()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
