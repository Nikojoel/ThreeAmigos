using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatform : MonoBehaviour {
    private Rigidbody2D bossRb; //Bosses Rigidbody2D
    private Rigidbody2D rb; //platforms RigidBody2D
    private bool active = false; //Tells bossplatform to stop only once.

	// Use this for initialization
	void Start () {
        bossRb = GameObject.Find("Boss").GetComponent<Rigidbody2D>(); //Finds Bosses Rigidbody2D.
        rb = gameObject.GetComponent<Rigidbody2D>(); //Defines bossPlatforms RigidBosy2D
	}
	
	/// <summary>
    /// stops platform under boss.
    /// </summary>
	void Update () {
        if (active && rb.position.x <= bossRb.position.x) //This will stop this platform under Boss.
        {
            active = false; //does stopping only once/when Activate method is called.
            rb.velocity = new Vector2(0f, 0f);
        }

	}
    /// <summary>
    /// moves this platform towards bosses x position
    /// </summary>
    public void Activate() //Moves platform to left.
    {
        active = true;
        rb.velocity = new Vector2(bossRb.position.x-rb.position.x, 0.0f);
    } 
    /// <summary>
    /// moves this platform away
    /// </summary>
    public void Deactivate() //Moves platform to right.
    {
        rb.velocity = new Vector2(25f, 0.0f);
    }
}
