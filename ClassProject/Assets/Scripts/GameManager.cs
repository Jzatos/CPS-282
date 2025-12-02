using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;

	// public variables
	[Header("=======Basic Attributes=======")]
	public int score = 0;
	public bool canBeatLevel = true; //Automatical go to next level if can beat level
	public AudioSource musicAudioSource;
	public bool gameIsOver = false;

	[Header("=======Main Canvas=======")]
	public GameObject mainCanvas;
	public Text mainScoreDisplay;
	public Text mainTimerDisplay;

	[Header("=======Game Over Canvas=======")]
	public GameObject gameOverCanvas;
	public Text gameOverText;
	public Button restartButton;
	public string restartLevelToLoad;

	public Button nextLevelButton;
	public string nextLevelSceneName;

	public Button homeButton;
	public string homeSceneName;

	public Button endGameButton;
	public string endGameSceneName;

	private float currentTime;

	// setup the game
	void Start() {

		// get a reference to the GameManager component for use by other scripts
		if (gm == null)
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		mainScoreDisplay.text = "0";

		//inactivate the game over canvas
		if (gameOverCanvas)
			gameOverCanvas.SetActive(false);

		// add button click listener to the buttons
		//If you need to check whether a button is assigned, use if (ButtonName) { ...}
		restartButton.onClick.AddListener(RestartGame);
		nextLevelButton.onClick.AddListener(NextLevel);
		homeButton.onClick.AddListener(ReturnToHomeScene);
		endGameButton.onClick.AddListener(GoToEndGameScene);
	}
	// this is the main game event loop
	void Update() {
		if (!gameIsOver) { //Game is not over (i.e., not lose / win / beat level)
			
			
		}
	}
	void LoseGame(){
		GameOver();
		gameOverText.text = "YOU LOSE!";
	}
	void WinGame()
	{
		GameOver();
		gameOverText.text = "CONGRATULATIONS!";
	}
	void GameOver() {
		// game is over
		gameIsOver = true;		
		// inactivate the main canvas 
		if (mainCanvas)
			mainCanvas.SetActive(false);
		// activate the game over canvas 
		if (gameOverCanvas)
			gameOverCanvas.SetActive(true);
		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	
	void BeatLevel() {
		// game is over
		GameOver();
		gameOverText.text =  "LEVEL COMPLETE";
		// LoadNextLevelAfter three seconds

	}
	// public function that can be called to update the score or time
	public void targetHit (int scoreAmount)
	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
	}

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the game from a basic level
		Debug.Log("Restarting!");
        SceneManager.LoadScene(restartLevelToLoad);
	}
	public void NextLevel() {
		SceneManager.LoadScene(nextLevelSceneName);
    }
	void ReturnToHomeScene() {
		SceneManager.LoadScene(homeSceneName);
	}
	void GoToEndGameScene()
	{
		SceneManager.LoadScene(endGameSceneName);
	}
}
