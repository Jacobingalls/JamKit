using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public GameObject QuitButton;
    public GameObject Content, PauseMenuTab, SettingsMenuTab;
    public UnityEvent onReturnToMainMenu;

    public void Resume()
    {
        TogglePause();
    }

    public void ReturnToMenu()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            Time.timeScale = 1.0f;
            onReturnToMainMenu.Invoke();
        }
        else
        {
            TogglePause();
        }
    }

    public void Quit()
    {
#if !UNITY_WEBGL
    Application.Quit();
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        Destroy(QuitButton);
#endif
    }

    float _cachedTimescale;
    void TogglePause()
    {
        ShowPauseMenuTab();

        if (Content.activeInHierarchy)
        {
            Time.timeScale = _cachedTimescale;
            Content.SetActive(false);
        }
        else
        {
            _cachedTimescale = Time.timeScale;
            Time.timeScale = 0.0f;
            Content.SetActive(true);
        }
    }

    public void ShowPauseMenuTab() {
        PauseMenuTab.SetActive(true);
        SettingsMenuTab.SetActive(false);
    }

    public void ShowSettingsMenuTab() {
        PauseMenuTab.SetActive(false);
        SettingsMenuTab.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
