using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlsBack : MonoBehaviour
{
    public GameObject PlayerControlsMenu;
    public void back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void start()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
