using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static Scene scene;
    private Button resumeButton; // Button that is found in the pause menu UI
    private Button exitButton;  // Button that is found in the pause menu UI
    private Button restartButton; // Button that is found in the pause menu UI
    private Button doOverButton; // Button that is found in the death screen UI
    private Button quitButton; // Button that is found in the death screen UI
    private bool gamePaused; // Boolean used to check if the game is paused
    [SerializeField] private GameObject PauseMenuUI; // Gameobject that displays the pause menu UI
    [SerializeField] private GameObject DeathUI; // Gameobject that displays the death screen UI
    [SerializeField] private GameObject LoadingScreenUI; // Gameobject that displays the loading screen UI
    private Timer timer; // Object reference of the Timer class
    private bool check; // Boolean used to check if the player is dead
    private static int score; // Integer value that is used as the score
    private AudioSource music; // Audiosource that is used as the music of the game
    private AudioSource deathSound; // Audiosource that is used as the death sound of the player
    private static int killScore; // Integer value that is used as the score that is rewarded from killing enemies
    /// <summary>
    /// Used as the initializer 
    /// </summary>
    private void Start()
    {
        killScore = 0;
        scene = SceneManager.GetActiveScene(); // Gets the currently active scene
        check = false; // Sets the boolean check to true
        music = GameObject.Find("Music").GetComponent<AudioSource>(); // Finds the game music object from the hierarchy
        music.Play(); // Calls the Play method and plays the music
        deathSound = GameObject.Find("Deathsound").GetComponent<AudioSource>(); // Finds the death sound object from the hierarchy
        LoadingScreenUI.SetActive(false); // Sets the activity of the loading screen UI to false
    }
    /// <summary>
    /// Pauses the game and displays the pause menu
    /// </summary>
    private void FreezeGame()
    {
        PauseMenuUI.SetActive(true); // Sets the activity of the pause menu UI to true and displays it
        Time.timeScale = 0; // Sets the time scale to 0
        gamePaused = true; // Changes the boolean gamepaused to true
        music.Pause(); // Calls the Pause method and pauses the music
        resumeButton = (Button)GameObject.Find("Resumebutton").GetComponent<Button>(); // Finds the resume button from the hierarchy
        resumeButton.onClick.AddListener(() => ResumeButtonClicked()); // Adds a listener to the resume button
        exitButton = (Button)GameObject.Find("Exitmenubutton").GetComponent<Button>(); // Finds the exit button from the hierarchy
        exitButton.onClick.AddListener(() => ExitMenuButtonClicked()); // Adds a listener to the exit button
        restartButton = (Button)GameObject.Find("Restartbutton").GetComponent<Button>(); // Finds the restart button from the hierarchy
        restartButton.onClick.AddListener(() => RestartButtonClicked()); // Adds a listener to the restart button
    }
    /// <summary>
    /// Continues the game and stops displaying the pause menu
    /// </summary>
    private void ContinueGame()
    {
        PauseMenuUI.SetActive(false); // Sets the activity of the pause menu UI to false and stops displaying it
        DeathUI.SetActive(false); // Sets the activity of the death screen UI to false and stops displaying it
        Time.timeScale = 1; // Sets the time scale back to 1
        gamePaused = false; // Changes the boolean gamepaused to false
        music.UnPause(); // Calls the UnPause method and unpauses the music
    }

    /// <summary>
    /// Used to pause the game
    /// </summary>
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && check == false) // Checks if the user presses ESC and the player isn't dead
        {
            if (gamePaused==true && check == false) // If the boolean gamepaused is true and the player isn't dead executes the ContinueGame method
            {
                ContinueGame();
            } else if (gamePaused==false && check == false) // If the boolean gamepaused is false and the player isn't dead executes the FreezeGame method
            { 
                FreezeGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && check == true) // Checks if the user has pressed Space and the player is dead
        {
            DoOverButtonClicked(); // Calls the DoOverButtonClicked method
        }
    }
    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// Continues the game
    /// </summary>
    private void ResumeButtonClicked()
    {
        ContinueGame(); // Calls the ContinueGame method
        
    }
    /// <summary>
    /// Exits the game and goes to the main mennu
    /// </summary>
    private void ExitMenuButtonClicked()
    {
        ContinueGame(); // Calls the ContinueGame method
        music.Stop();
        GameObject.Find("Timer").GetComponent<Timer>().SetText(); // Sets the time text as empty
        GameObject.Find("YOURHP").SetActive(false); // Deactivates the taco images
        LoadingScreenUI.SetActive(true); // Sets the activity of the loading screen UI to true and displays it
        SceneManager.LoadScene(0, LoadSceneMode.Single); // Loads the main menu scene
    }
    /// <summary>
    /// Restarts the current level the player is on
    /// </summary>
    private void RestartButtonClicked()
    {
        ContinueGame(); // Calls the ContinueGame method
        music.Stop(); // Calls the Stop method and stops the music 
        GameObject.Find("Timer").GetComponent<Timer>().SetText(); // Sets the time text as empty
        GameObject.Find("YOURHP").SetActive(false); // Deactivates the taco images
        LoadingScreenUI.SetActive(true); // Sets the activity of the loading screen UI to true and displays it
        SceneManager.LoadScene(scene.name); // Loads the scene
    }
    /// <summary>
    /// Stops the game completely and displays the death menu UI
    /// </summary>
    public void StopGame()
    {
        check = true; // Changes the boolean check to true
        GameObject.Find("Player").GetComponent<Playermovement>().SetHealth(0); // Sets the players health to 0
        music.Stop(); // Calls the Stop method and stops the music
        DeathUI.SetActive(true); // Sets the activity of the death screen UI to true and displays it
        Time.timeScale = 0; // Sets the time scale to 0
        gamePaused = true; // Changes the boolean gamepaused to true
        deathSound.Play(); // Calls the Play method and plays the music
        doOverButton = (Button)GameObject.Find("Restartbutton").GetComponent<Button>(); // Finds the restart button from the hierarchy
        doOverButton.onClick.AddListener(() => DoOverButtonClicked()); // Adds a listener to the restart button
        quitButton = (Button)GameObject.Find("Quitbutton").GetComponent<Button>(); // Finds the quit button from the hierarchy
        quitButton.onClick.AddListener(() => QuitButtonClicked()); // Adds a listener to the quit button

    }
    /// <summary>
    /// Restarts the current level the player is on
    /// </summary>
    private void DoOverButtonClicked()
    {
        ContinueGame(); // Calls the ContinueGame method
        LoadingScreenUI.SetActive(true); // Sets the activity of the loading screen UI to true and displays it
        SceneManager.LoadScene(scene.name); // Loads the scene
    }
    /// <summary>
    /// Exits the game and goes to the main menu
    /// </summary>
    private void QuitButtonClicked()
    {
        ContinueGame(); // Calls the ContinueGame method
        LoadingScreenUI.SetActive(true); // Sets the activity of the loading screen UI to true and displays it
        SceneManager.LoadScene(0, LoadSceneMode.Single); // Loads the main menu scene
    }
    /// <summary>
    /// Changes the scenes by adding 1 to the build index every time its called, also gets the score of the level
    /// </summary>
    public void ChangeScene()
    {
        score += Timer.GetScore() + killScore; // Adds the previous level score
        LoadingScreenUI.SetActive(true);
        GameObject.Find("Timer").GetComponent<Timer>().SetText();
        GameObject.Find("YOURHP").SetActive(false); // Deactivates the taco images
        SceneManager.LoadScene(scene.buildIndex + 1); // Loads the next scene by plussing the build index by 1
    }
    /// <summary>
    /// Returns the score when called
    /// </summary>
    /// <returns></score>
    public static int GetScore()
    {
        return score; // Returns the score
    }
    /// <summary>
    /// Sets the score to 0
    /// </summary>
    public static void SetScore()
    {
        score = 0; // Sets the score to 0
    }
    /// <summary>
    /// Adds the parameter value to the score when enemy is killed
    /// </summary>
    /// <param name="amount"></param>
    public static void AddScore(int amount)
    {
        killScore += amount;
    }
}

