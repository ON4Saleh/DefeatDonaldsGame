using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlsBack : MonoBehaviour
{
    public GameObject PlayerControlsMenu;
    public void back()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void start()
    {
        SceneManager.LoadSceneAsync("MergeMap");
    }
}
