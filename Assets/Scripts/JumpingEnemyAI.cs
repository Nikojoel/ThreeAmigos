using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyAI : MonoBehaviour {
    [SerializeField] private float speed; //Movement speed of enemy
    [SerializeField] private float jumpforce; //Force of jump
    [SerializeField] private float health; //health of enemy
    private Vector2 jump;
    private Rigidbody2D rb;  //Enemys Rigidbody2D
    private Rigidbody2D playerRB; //Players rigidBody2D.
    [SerializeField] private GameObject healthPickup; //healthPickups that enemies drop on death
    [SerializeField] private GameObject powerupPickup; //powerupPickups that enemies drop on death
    private bool activate = false; //when true enemy starts moving
    private AudioSource hitSound;
    private Animator animator;
    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>(); //Finds enemys RigidBody2D
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>(); //Finds Players RigidBody2D
        hitSound = GameObject.Find("Hitsound").GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }
        
	
	/// <summary>
    /// Here is declared how this enemy moves
    /// </summary>
	void Update () {
        if (playerRB.position.x + 30 >= rb.position.x) //when Player is 30 units away from enemy, enemy starts moving.
        {
            activate = true;
            rb.gravityScale = 5;
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
    public void Hit()
    {
         health--;
    }
    /// <summary>
    /// Checks Triggers that collide with enemy.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.Play("Turtle_Anim", -1, 0);    
        if (activate && collision.gameObject.CompareTag("Ground")) //when enemy collides with ground, enemy jumps towards player.
        {
            if (playerRB.position.x >= rb.position.x)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (playerRB.position.x < rb.position.x)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            jump = new Vector2(rb.velocity.x, jumpforce);
            rb.velocity = jump;
        }
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
