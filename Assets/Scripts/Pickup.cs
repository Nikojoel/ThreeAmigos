using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    private Rigidbody2D rb; //pickups rigidbody
    private Vector2 position; //fixed position spot which is used to make pickups go up and down.
    [SerializeField] private string pickupType; //type ot the pickup.
    private GameObject player; //Players Gameobject
    private Playermovement PMscript; //Players Scritp
    private int seconds; //Time how long the powerup will last.
    private AudioSource pickUpSound; // Audisource used as the sound when the player pick ups a object

	/// <summary>
    /// Defines correct stats for current gameobject
    /// </summary>
	void Start () {
        seconds = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        pickUpSound = GameObject.Find("Pickupsound").GetComponent<AudioSource>();
        PMscript = player.GetComponentInChildren<Playermovement>();
        position = rb.position;
        rb.AddForce(new Vector2(0, 75));
    }
	
	/// <summary>
    /// Here is done the up down movement of this gameobject
    /// </summary>
	void FixedUpdate () {
        if (rb.position.y < position.y)
        {
           rb.AddForce(new Vector2(0, 5));
        }
	}
    /// <summary>
    /// When player triggers with this gameobject Effect method is called.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player Collider")
        {
            Effect();
        }
    }
    /// <summary>
    /// Here is defined what different types of pickups does
    /// </summary>
    private void Effect()
    {
      
        if (pickupType.Equals("Health") && PMscript.GetHealth() < 3) //player is healed if he does not hae 3 health
        {
            Destroy(this.gameObject);
            PMscript.Heal();
            pickUpSound.Play();
        }
        if (pickupType.Equals("Power")) //player is given attackspeed powerup for 10 seconds. 
        {
            StartCoroutine(PowerUP());
            seconds = 10;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            pickUpSound.Play();
        }
    }
    /// <summary>
    /// This is the duration of this powerup
    /// </summary>
    /// <returns></returns>
    private IEnumerator PowerUP()
    {
        do
        {
            PMscript.SetAttkspd(0.2f);
            yield return new WaitForSeconds(1);
            seconds--;
        } while (seconds > 0);
        PMscript.SetAttkspd(0.4f);
        Destroy(this.gameObject);
    }
}
