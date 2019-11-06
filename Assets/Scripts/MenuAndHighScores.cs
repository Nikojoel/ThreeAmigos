using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuAndHighScores : MonoBehaviour {
    private Button startButton;
    private Button exitButton; 
    private Button scoreBoardButton; 
    private Button levelButton; 
    private Button okButton;
    // Buttons that are found in the main menu scene
    private Button level1;
    private Button level2; 
    private Button level3; 
    // Buttons that are found in the level selection UI
    private Button boss; // Button that is found in the level selection UI
    [SerializeField] private GameObject highScoreUI; // Gameobject that displays the high score UI
    [SerializeField] private GameObject levelUI; // Gameobject that displays the level selection UI
    [SerializeField] private GameObject loadingUI; // Gameobject that displays the loading screen UI
    [SerializeField] private GameObject howToPlayUI; // Gameobject that displays the how to play UI
    private bool levelUIOn = false; // Boolean used to check if the level selection UI is on
    private bool highScoreUIOn = false; // Boolean used to check if the high score UI is on
    private bool howToPlayUIOn = false; // Boolean used to check if the how to play UI is on
    /// <summary>
    /// Used as the initializer
    /// </summary>
    void Start () {
        exitButton = GameObject.Find("Exitbutton").GetComponent<Button>(); // Finds the exit button from the hierarchy
        exitButton.onClick.AddListener(() => ExitButtonClicked()); // Adds a listener to the exit button
        startButton = GameObject.Find("Startbutton").GetComponent<Button>(); // Finds the start button from the hierarchy
        startButton.onClick.AddListener(() => StartButtonClicked()); // Adds a listener to the start button 
        scoreBoardButton = GameObject.Find("Scoreboardbutton").GetComponent<Button>(); // Finds the scoreboard button from the hierarchy
        scoreBoardButton.onClick.AddListener(() => ScoreBoardButtonClicked()); // Adds a listener to the scoreboard button
        levelButton = GameObject.Find("Levelbutton").GetComponent<Button>(); // Finds the level selection button from the hierarchy
        levelButton.onClick.AddListener(() => LevelButtonClicked()); // Adds a listener to the level selection button
        SceneChanger.SetScore(); // Sets the score back to 0, so that the next player will not have the same score as the previous one
    }

    /// <summary>
    /// Checks if the level UI or the high score UI is on 
    /// </summary>
    void FixedUpdate()
    {
        if(levelUIOn==true && Input.GetKeyDown(KeyCode.Escape)) // Checks if the levelUIOn boolean is true and the user has pressed ESC
        {
            levelUI.SetActive(false); // Sets the activity of the level selection UI to false and stops displaying it
            levelUIOn = false; // Sets the boolean value to false
        }
        if (highScoreUIOn==true && Input.GetKeyDown(KeyCode.Escape)) // Checks if the highScoreUIOn boolean is true and the user has pressed ESC
        {
            highScoreUI.SetActive(false); // Sets the activity of the high score UI to false and stops displaying it
            highScoreUIOn = false; // Sets the boolean value to false
        }
        if (howToPlayUIOn == true && Input.GetKeyDown(KeyCode.Escape))
        {
            howToPlayUI.SetActive(false);
            howToPlayUIOn = false;
        }
    }
    /// <summary>
    /// Dispays the how to play UI
    /// </summary>
    private void StartButtonClicked()
    {
        howToPlayUI.SetActive(true);
        howToPlayUIOn = true;
        okButton = GameObject.Find("Okbutton").GetComponent<Button>();
        okButton.onClick.AddListener(() => OkButtonClicked());
    }
    /// <summary>
    /// Starts the level 1 of the game
    /// </summary>
    private void OkButtonClicked()
    {
        howToPlayUI.SetActive(false);
        loadingUI.SetActive(true);
        SceneManager.LoadScene(1, LoadSceneMode.Single); // Loads the first level of the game
    }
    /// <summary>
    /// Closes the application
    /// </summary>
    private void ExitButtonClicked()
    {
        ///UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit(); // Quits the application when the game is built
    }
    /// <summary>
    /// Displays the high score UI
    /// </summary>
    private void ScoreBoardButtonClicked()
    {
        highScoreUI.SetActive(true); // Sets the activity of the high score UI to true and displays it
        highScoreUIOn = true; // Sets the boolean value to true
    }
    /// <summary>
    /// Allows the player to start a specific level
    /// </summary>
    private void LevelButtonClicked()
    {
        levelUI.SetActive(true); // Sets the activity of the level selection UI to true and displays it
        levelUIOn = true; // Sets the boolean value to true
        level1 = GameObject.Find("Level1button").GetComponent<Button>(); // Finds the level 1 button from the hierarchy
        level2 = GameObject.Find("Level2button").GetComponent<Button>(); // Finds the level 2 button from the hierarchy
        level3 = GameObject.Find("Level3button").GetComponent<Button>(); // Finds the level 3 button from the hierarchy
        boss = GameObject.Find("Bossbutton").GetComponent<Button>(); // Finds the boss button from the hierarchy
        level1.onClick.AddListener(() => Level1ButtonClicked()); // Adds a listener to the level 1 button 
        level2.onClick.AddListener(() => Level2ButtonClicked()); // Adds a listener to the level 2 button
        level3.onClick.AddListener(() => Level3ButtonClicked()); // Adds a listener to the level 3 button
        boss.onClick.AddListener(() => BossButtonClicked()); // Adds a listener to the boss button
    }
    /// <summary>
    /// Stars level 1
    /// </summary>
    private void Level1ButtonClicked()
    {
        levelUI.SetActive(false);
        levelUIOn = false;
        loadingUI.SetActive(true);
        SceneManager.LoadScene(1, LoadSceneMode.Single); // Loads the first level  
    }
    /// <summary>
    /// Starts level 2
    /// </summary>
    private void Level2ButtonClicked()
    {
        levelUI.SetActive(false);
        levelUIOn = false;
        loadingUI.SetActive(true);
        SceneManager.LoadScene(2, LoadSceneMode.Single); // Loads the second level
    }
    /// <summary>
    /// Starts level 3
    /// </summary>
    private void Level3ButtonClicked()
    {
        levelUI.SetActive(false);
        levelUIOn = false;
        loadingUI.SetActive(true);
        SceneManager.LoadScene(3, LoadSceneMode.Single); // Loads the third level
    }
    /// <summary>
    /// Starts level 4, which is the boss fight
    /// </summary>
    private void BossButtonClicked()
    {
        levelUI.SetActive(false);
        levelUIOn = false;
        loadingUI.SetActive(true);
        SceneManager.LoadScene(4, LoadSceneMode.Single); // Loads the boss level
    }
}
