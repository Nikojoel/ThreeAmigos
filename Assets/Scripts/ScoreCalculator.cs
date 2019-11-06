using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ScoreCalculator : Database {
    private Button submitButton; // Button that is found on the end credits scene
    private InputField inputText; // Object reference of the InputField
    private static String playerName; // String used as the players name
    private static int score; // Integer that is used as the players score
    private Text scoreNumber; // Text that represents the players score
    
	/// <summary>
    /// Used as the initializer
    /// </summary>
	void Start () {
        inputText = GameObject.Find("InputField").GetComponent<InputField>(); // Finds the input field from the hierarchy
        submitButton = GameObject.Find("Submitbutton").GetComponent<Button>(); // Finds the submit button from the hierarchy
        submitButton.onClick.AddListener(() => SubmitName()); // Adds a listener to the submit button
        scoreNumber = GameObject.Find("Scoretext").GetComponent<Text>();
        SetText(); // Calls the SetText method and displays the player score
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Sets the score as the scoreText that is visible on the screen
    /// </summary>
    private void SetText()
    {
        CalculateScore();  // Calls the CalculateScore method
        scoreNumber.text = score.ToString(); // Sets the score as the text
    }
    /// <summary>
    /// Adds the player in to the database and loads the main menu scene
    /// </summary>
    public void SubmitName()
    {
        base.CreateDBTable(); // Calls the CreateDBTable() method in the parent class
        playerName = inputText.text.ToString(); // Sets the player name as the text in the input field
        base.AddToDB(); // Calls the AddToDB method in the parent class
        GoToMenu(); // Calls the GoToMenu method
    }
    /// <summary>
    /// Returns the player name and if it has been left empty, the players name will be "Unknown"
    /// </summary>
    /// <returns></returns>
    public static String GetName()
    {
        if (playerName == "") // Checks if the input field is empty
        {
            return playerName = "Unknown"; // If its empty, sets the player name to "Unknown"
        } else
        {
            return playerName; // Returns the player name
        }
        
    }
    /// <summary>
    /// Returns the players score
    /// </summary>
    /// <returns></returns>
    public static int GetScore()
    {
        return score; // Returns the score
    }
    /// <summary>
    /// Gets the score from SceneChanger class
    /// </summary>
    public void CalculateScore()
    {
        score = SceneChanger.GetScore(); // Sets the score by calling the GetScore method from the SceneChanger class
    }
    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    private void GoToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single); // Loads the main menu scene  
    }
}

