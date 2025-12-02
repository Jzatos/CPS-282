using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public Button mainMenuButton;
    public string mainMenuSceneName;
    public Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        quitButton.onClick.AddListener(ExitGame);
    }
    void LoadMainMenu() {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    void ExitGame() {
        //This will be ignored in Unity Editor Play Mode. It only works in Build version
        Application.Quit();
    }
}
