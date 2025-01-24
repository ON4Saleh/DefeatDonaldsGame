using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    public GameObject loadingScreen;
    private void Awake()
    {
        instance = this;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        loadingScreen.SetActive(true); 
        scenesLoading.Add(SceneManager.UnloadSceneAsync(0));
        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }
    public IEnumerator GetSceneLoadProgress()
    {
       for(int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }  
        }
       loadingScreen.SetActive(false);
    }
}
