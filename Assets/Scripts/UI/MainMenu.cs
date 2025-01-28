using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadSceneAsync("MergeMap");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
  
}
