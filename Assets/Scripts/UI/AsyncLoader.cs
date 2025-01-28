using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private Image loadingImage;

    public void loadLevelBtn(string levelToLoad)
    {
        MainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(loadLevelAsync(levelToLoad));
    }

    IEnumerator loadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            float progressValue = loadOperation.progress;
            loadingImage.fillAmount = progressValue;

            if (loadOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    void Start()
    {
        loadingImage.fillAmount = 0;
    }

    void Update()
    {

    }
}