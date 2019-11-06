using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour {
    [SerializeField] private float health; //health of enemy
    [SerializeField] private float speed; //Movement speed of enemy
    private Rigidbody2D rb;  //Enemys Rigidbody2D
    private Rigidbody2D playerRB; //Players rigidBody2D.
    [SerializeField] private GameObject healthPickup; //healthPickups that enemies drop on death
    [SerializeField] private GameObject powerupPickup; //powerupPickups that enemies drop on death
    private AudioSource hitSound;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>(); //Finds enemys RigidBody2D
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>(); //Finds Players RigidBody2D
        hitSound = GameObject.Find("Hitsound").GetComponent<AudioSource>();
    }
	
	/// <summary>
    /// Here is declared ow enemy moves and when it dies
    /// </summary>
	void FixedUpdate () {
        if (playerRB.position.x + 30 >= rb.position.x) ////when Player is 30 units away from enemy, enemy starts moving.
        {
            Vector2 angle = rb.position - playerRB.position;
            angle = angle.normalized;
            rb.AddForce(angle * -speed); //flies towards player
        }
        if(rb.velocity.x < -0.1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x > 0.1f) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (health <= 0) //kills enemy and rolls RandomDrop
        {
            SceneChanger.AddScore(75);
            RandomDrop();
            Destroy(this.gameObject);   
        }
    }
    /// <summary>
    /// when this is called enemy loses one helath.
    /// </summary>
    public void Hit() //when this is called loses one helath.
    {
        health--;
    }
    /// <summary>
    /// Checks collisions that collide with enemy.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tortilla"))  //Takes damage when hit by Tortilla.
        {
            hitSound.Play();
            Destroy(collision.gameObject);
            Hit();
        }
    }
    /// <summary>
    /// Drops randomly healthPickup or powerupPickup
    /// </summary>
    private void RandomDrop()
    {
        int random = Random.Range(1, 6);
        if (random == 1) //10% change for enemy to drop healthPickup
        {
            GameObject HealthPU = Instantiate(healthPickup, rb.position, Quaternion.identity);
            HealthPU.name = "Health Pickup";
        }
        if (random == 2) //10% change for enemy to drop powerupPickup
        {
            GameObject HealthPU = Instantiate(powerupPickup, rb.position, Quaternion.identity);
            HealthPU.name = "Powerup Pickup";
        }
    }
}
