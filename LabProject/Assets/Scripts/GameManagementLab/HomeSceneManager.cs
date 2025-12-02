using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HomeSceneManager: MonoBehaviour
{
    [Header("===========Main Menu===========")]
    public GameObject MainMenuCanvas;
    public Button NewGameButton;
    public Button LevelSelectButton;
    public Button AboutButton;
    public Button EndGameButton;

    [Header("===========Sub Menu===========")]
    public GameObject SubMenuCanvas;
    public Button MainMenuButton;
    //Any shared UI elements on the sub menu

    [Header("===========Level Select Menu===========")]
    public GameObject LevelSelectCanvas;
    public string[] LevelNames;
    public Button LevelButtonPrefab;

    [Header("===========About Menu===========")]
    public GameObject AboutCanvas;

    [Header("===========Scene Names===========")]
    public string NewGameScene;
    public string EndGameScene;

    private string levelName;
    // Start is called before the first frame update
    void Start()
    {
        //Set up the button listener
        NewGameButton.onClick.AddListener(StartNewGame);
        LevelSelectButton.onClick.AddListener(SelectLevel);
        AboutButton.onClick.AddListener(DisplayAboutInfo);
        MainMenuButton.onClick.AddListener(ShowMainMenu);
        EndGameButton.onClick.AddListener(EndGame);
        //Set up the levels
        SetLevels();

        //Display proper menu
        ShowMainMenu();
    }
    void EndGame()
    {
        SceneManager.LoadScene(EndGameScene);
    }
    void StartNewGame()
    {
        SceneManager.LoadScene(NewGameScene);
    }
    void LoadLevel()
    {
        //This method is invoked by a button click, so the currentSelectedGameObject is the button
        //Get the button name from the event system
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        //get level name from the buttonName. Previously the button name was generated as levelName + "Button"
        string levelName = buttonName.Substring(0, buttonName.Length - 6);
        Debug.Log("Level to load: " + levelName);
        SceneManager.LoadScene(levelName);
    }
    void DisplayAboutInfo()
    {
        MainMenuCanvas.SetActive(false);
        LevelSelectCanvas.SetActive(false);

        AboutCanvas.SetActive(true);
        SubMenuCanvas.SetActive(true);
    }
    void ShowMainMenu()
    {
        SubMenuCanvas.SetActive(false);
        //AboutCanvas.SetActive(false);
        //LevelSelectCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        Debug.Log("Main Menu button pressed");
    }
    void SelectLevel()
    {
        //Switch canvas
        MainMenuCanvas.SetActive(false);
        AboutCanvas.SetActive(false);
        LevelSelectCanvas.SetActive(true);
        SubMenuCanvas.SetActive(true);
    }
    void SetLevels()
    {
        //Set up the level buttons based on the level names
        for (int i = 0; i < LevelNames.Length; i++)
        {
            levelName = LevelNames[i];
            //Create a button from the button template LevelButtonPrefab
            Button levelButton = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);
            //Give the button a unique name
            levelButton.name = levelName + "Button";
            //Set the label of the button
            Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
            levelButtonLabel.text = levelName;

            //Set up the button listener
            Button levelButtonScript = levelButton.GetComponent<Button>();
            levelButtonScript.onClick.RemoveAllListeners();
            //levelButtonScript.onClick.AddListener(() => LoadLevel(levelName));
            levelButtonScript.onClick.AddListener(LoadLevel);
            //Debug.Log("Level name-" + levelName + "-added to button -" + levelButton.name);

            // set the parent of the button as the LevelSelectCanvas so it will be dynamically arranged based on the defined layout
            levelButton.transform.SetParent(LevelSelectCanvas.transform, false);



            // You can even set the button interactivity based on whether the level has been played thru or not
        }
    }
}