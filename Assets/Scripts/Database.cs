using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

public class Database : MonoBehaviour {
    private Text rankText; // Text that displays the rank
    private Text nameText; // Text that displays the player name
    private Text scoreText; // Text that displays the players score
    private Button emptyDbButton; // Button that is found in the high score UI
    private IDbConnection dbconn; // Connection that accesses relational databases
    private IDbCommand dbcmd; // Command that is executed while a database connection exists
    private String userName; // String that is used as the players name
    private int playerScore; // Integer that is used as the players score
    /// <summary>
    /// Creates a connection to the database and selects information from the table
    /// </summary>
    private void Start() {
        rankText = GameObject.Find("Ranktext").GetComponent<Text>(); // Finds the rank text from the hierarchy
        nameText = GameObject.Find("Nametext").GetComponent<Text>(); // Finds the name text from the hierarchy
        scoreText = GameObject.Find("Scoretext").GetComponent<Text>(); // Finds the score text from the hierarchy
        emptyDbButton = GameObject.Find("Deletebutton").GetComponent<Button>(); // Finds the delete button from the hierarchy
        emptyDbButton.onClick.AddListener(() => DeleteDB()); // Adds a listener to the delete button
        CreateDBTable(); // Establishes a connection to the database by calling ConnectToDB
        UpdateDB(); // Updates and closes the connection to the database by calling UpdateDB
        

    }
    /// <summary>
    /// Tries to create a new table if one doesn't already exist
    /// </summary>
    public void CreateDBTable()
    {
        ConnectToDB(); // Establishes a connection to the database by calling ConnectToDB
        String createTable = "CREATE TABLE IF NOT EXISTS " + "Player_info" + " (" + "Id" + " INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " + "Name" + " Text NOT NULL, " + "Score" + " INTEGER NOT NULL)";
        // Script that tires to create a table if one doesn't already exist
        dbcmd = dbconn.CreateCommand(); // Creates a SQL command
        dbcmd.CommandText = createTable; // Sets the commandText as the string defined above
        dbcmd.ExecuteReader(); // Executes the command
    }
    /// <summary>
    /// Selects information from the table then closes the connection and disposes the command executer
    /// </summary>
    private void UpdateDB()
    {
        String selectFromTable = "SELECT Name,Score FROM Player_info ORDER BY Score DESC"; // Query used to select information from the database
        dbcmd = dbconn.CreateCommand(); // Creates a SQL command
        dbcmd.CommandText = selectFromTable; // Sets the commandText as the string defined above
        IDataReader reader = dbcmd.ExecuteReader(); // Executes the command
        rankText.text = ""; // Sets the rank text as empty
        nameText.text = ""; // Sets the name text as empty
        scoreText.text = ""; // Sets the score text as empty
        int i = 0; // Integer value used in the while loop
        int rank = 1; // Integer value used as the rank
        while (reader.Read() && i<10) // While loop that loops until the reader has stopped reading the records and the integer i is 10
        {
            i++; // Adds 1 to the i value
            String name = reader.GetString(0); // 
            int score = reader.GetInt32(1); // 

            rankText.text += "#" + rank.ToString() + "\n"; // Sets the rank text as #, the value of the rank and changes line
            nameText.text += name + "\n"; // Sets the name text as the string name
            scoreText.text += score.ToString() + "\n"; // Sets the score text as the integer score
            rank++; // Adds 1 to the rank value

        }
        reader.Close(); // Closes the reader connection
        reader = null; // Sets the reader value to null
        dbcmd.Dispose(); // Disposes the execution of the command
        dbcmd = null; // Sets the dbcmd value to null
        dbconn.Close(); // Closes the database connection
        dbconn = null; // Sets the dbconn value to null
    }
    // Update is called once per frame
    void Update() {
        
    }
    /// <summary>
    /// Deletes the minimum score from the table
    /// </summary>
    private void DeleteDB()
    {
        ConnectToDB(); // Establishes a connection to the database by calling ConnectToDB
        String deleteTable = String.Format("DELETE FROM Player_info WHERE Score = (SELECT MIN(Score) FROM Player_info)"); // Query used to delete information from the database
        dbcmd = dbconn.CreateCommand(); // Creates a command
        dbcmd.CommandText = deleteTable; // Sets the commandText as the string defined above
        dbcmd.ExecuteReader(); // Executes the command
        UpdateDB(); // Updates and closes the connection to the database by calling UpdateDB

    }
    /// <summary>
    /// Inserts the player name and score in to the database
    /// </summary>
    public void AddToDB()
    {
        ConnectToDB(); // Establishes a connection to the database by calling ConnectToDB
        userName = ScoreCalculator.GetName(); // Calls the GetName method that is defined in the ScoreCalculator class and sets the playerName
        playerScore = ScoreCalculator.GetScore(); // Calls the GetScore method that is defined in the ScoreCalculator class and sets the playerScore
        String insertToTable = String.Format("INSERT INTO Player_info(Name,Score) VALUES(\"{0}\",\"{1}\")", userName, playerScore);
        // Query used to insert information to the database
        dbcmd = dbconn.CreateCommand(); // Creates a command
        dbcmd.CommandText = insertToTable; // Sets the commandText as the string defined above
        dbcmd.ExecuteReader(); // Executes the command
    }
    /// <summary>
    /// Opens a new connection to the database
    /// </summary>
    private void ConnectToDB()
    {
        String conn = "Uri=file:" + Application.dataPath + "/database_new.db"; // String used to open a database
        dbconn = (IDbConnection)new SqliteConnection(conn); // Creates a new SqLiteConnection that accesses the database
        dbconn.Open(); // Opens a database connection with the settings specified by the connection string 
        
    }
 
    
}


