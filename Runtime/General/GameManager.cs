using info.jacobingalls.jamkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject transitionalLoadingScreen;

    public string TitleSceneName, TutorialSceneName, GameSceneName, WinSceneName, LoseSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync(TitleSceneName, LoadSceneMode.Additive);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private GameObject _activeLoadingScreen;

    public void UnloadAll()
    {
        if (SceneManager.GetSceneByName(TitleSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(TitleSceneName));
        }

        if (SceneManager.GetSceneByName(GameSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(GameSceneName));
        }

        if (SceneManager.GetSceneByName(TutorialSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(TutorialSceneName));
        }

        if (SceneManager.GetSceneByName(LoseSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(LoseSceneName));
        }

        if (SceneManager.GetSceneByName(WinSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(WinSceneName));
        }
    }

    public void ShowMainWindow()
    {
        _activeLoadingScreen = transitionalLoadingScreen;
        if (_activeLoadingScreen.activeSelf == true) { return; }
        _activeLoadingScreen.SetActive(true);

        UnloadAll();

        scenesLoading.Add(SceneManager.LoadSceneAsync(TitleSceneName, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowTutorial()
    {
        _activeLoadingScreen = transitionalLoadingScreen;
        if (_activeLoadingScreen.activeSelf == true) { return; }
        _activeLoadingScreen.SetActive(true);

        UnloadAll();

        scenesLoading.Add(SceneManager.LoadSceneAsync(TutorialSceneName, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowGame()
    {
        _activeLoadingScreen = transitionalLoadingScreen;
        if (_activeLoadingScreen.activeSelf == true) { return; }
        _activeLoadingScreen.SetActive(true);

        UnloadAll();

        scenesLoading.Add(SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowLoseScreen()
    {
        _activeLoadingScreen = transitionalLoadingScreen;
        if (_activeLoadingScreen.activeSelf == true) { return; }
        _activeLoadingScreen.SetActive(true);

        UnloadAll();

        scenesLoading.Add(SceneManager.LoadSceneAsync(LoseSceneName, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowWinScreen()
    {
        _activeLoadingScreen = transitionalLoadingScreen;
        if (_activeLoadingScreen.activeSelf == true) { return; }
        _activeLoadingScreen.SetActive(true);

        UnloadAll();

        scenesLoading.Add(SceneManager.LoadSceneAsync(WinSceneName, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    bool _waitingForSpaceToContinue;
    public IEnumerator GetSceneLoadProgress()
    {
        foreach(var sceneLoad in scenesLoading)
        {
            while (!sceneLoad.isDone)
            {
                float totalProgress = 0;
                foreach(var operation in scenesLoading)
                {
                    totalProgress += operation.progress;
                }
                _activeLoadingScreen.GetComponentInChildren<Slider>().value = (totalProgress / scenesLoading.Count);
                yield return null;
            }
        }

        if(_activeLoadingScreen == transitionalLoadingScreen)
        {
            _activeLoadingScreen.gameObject.SetActive(false);
            scenesLoading.Clear();
        }
        else
        {
            // O M E G A L U L
            _activeLoadingScreen.GetComponentInChildren<Slider>().value = 1.0f;
            var loading = _activeLoadingScreen.transform.FindDeepChild("Loading");
            loading.GetComponent<TextMeshProUGUI>().text= "press space to continue";
            _waitingForSpaceToContinue = true;
            Time.timeScale = 0.0f;
        }
    }

    private void Update()
    {
        if (_waitingForSpaceToContinue && Input.GetKeyDown(KeyCode.Space))
        {
            _activeLoadingScreen.gameObject.SetActive(false);
            scenesLoading.Clear();
            Time.timeScale = 1.0f;
        }
    }
}

public static class TransformDeepChildExtension
{
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}