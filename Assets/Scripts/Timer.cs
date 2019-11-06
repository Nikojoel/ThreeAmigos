using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Timer : MonoBehaviour {
    public int duration; // Integer value that defines the duration of the level
    private static int timeRemaining; // Integer value that tells the time that is remaining in the level
    private bool isCounting = false; // Boolean used to check if the time is counting
    private SceneChanger sceneChanger; // Object reference of the SceneChanger class
    private Text timeText; // Text that shows the time in the game
    private static int score; // Integer value that is used as the score
    /// <summary>
    /// Used to initialize the timer in the level
    /// </summary>
    void Start () {
        sceneChanger = GameObject.Find("Pausemenu").GetComponent<SceneChanger>();
        // Finds the player from the hierarchy and gets the component SceneChanger from the children
        timeText = GameObject.Find("Timetext").GetComponent<Text>(); // Finds the time text from the hierarchy
        if (!isCounting) // Checks if the boolean isCounting is true
        {
            isCounting = true; // Changes the boolean isCounting to true
            timeRemaining = duration; // Sets the timeRemaining to be the same as the duration
            timeText.text = timeRemaining.ToString(); // Sets the time text as "Time left:" and the timeRemaining
            Invoke("Tick",1f); // Invokes the Tick() method
        }
    }
    /// <summary>
    /// Used as the second ticker
    /// </summary>
    private void Tick() {

        timeRemaining--; // Minuses 1 from the timeRemaining
        timeText.text = timeRemaining.ToString(); // Sets the time text as "Time left:" and the timeRemaining

         if (timeRemaining > 0) // Checks if the timeRemaining is more than 0
        {
            Invoke("Tick", 1f); // While the timeRemaining is more than 0, Invokes the Tick method every 1 second
        }

         else // When the timeRemaining is 0
        {
            isCounting = false; // Changes the boolean isCounting to false
            sceneChanger.StopGame(); // Calls the StopGame() method from the SceneChanger class, this brings up the death screen UI
            SetText(); // Calls the SetText method that "hides" the time text
            GameObject.Find("YOURHP").SetActive(false);// Sets the activity of all the taco images on the screen to false and stops displaying it
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    /// <summary>
    /// Used when the player is dead
    /// </summary>
    public void SetText()
    {
        GameObject.Find("Clock").SetActive(false);
    }
    /// <summary>
    /// Returns the score
    /// </summary>
    /// <returns></returns>
    public static int GetScore()
    {
        score = timeRemaining * 5; // Multiplies the timeRemaining by 5 and sets it as the score
        return score; // Returns the score
    }
}
