using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    [SerializeField] private float health; //health of enemy
    [SerializeField] private float speed; //Movement speed of enemy
    [SerializeField] private GameObject prefab; //Bullet that eemies shoot
    [SerializeField] private GameObject healthPickup; //healthPickups that enemies drop on death
    [SerializeField] private GameObject powerupPickup; //powerupPickups that enemies drop on death
    private Rigidbody2D playerRB; //Players rigidBody2D.
    [SerializeField] private string enemyType; //Type ot enemy(Public agent/Solidier)
    private bool nextMove = true; //when true, enemy does its next move
    private Rigidbody2D rb; //Enemys Rigidbody2D
    private bool moveleft; //when true enemy moves left
    private bool moveright; //when true enemy moves right
    private bool activate = false; //when true enemy starts moving
    private bool grounded = false; //Checks if enemy collides with ground.
    private AudioSource hitSound; // Audiosource used as the hit sound
    private Animator animator;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>(); //Finds enemys RigidBody2D
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>(); //Finds Players RigidBody2D
        hitSound = GameObject.Find("Hitsound").GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Here is declared ow enemy moves and when it dies
    /// </summary>
	void Update () {
        if (playerRB.position.x + 30 >= rb.position.x && grounded) //when Player is 30 units away from enemy and enemy is touching ground, enemy starts moving.
        {
            activate = true;
        }
        if (nextMove && activate) //enemy does its next move.
        {
            StartCoroutine(next(Random.Range(1,5)));
            nextMove = false;
        }
        if (moveleft) //moves enemy to left
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (moveright) //moves enemy to right
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            if (playerRB.position.x < rb.position.x)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (health <= 0) //kills enemy and rolls RandomDrop
        {
            SceneChanger.AddScore(50);
            RandomDrop();
            Destroy(this.gameObject);
        }
        
	}
    /// <summary>
    /// here is declared what is enemys next move (chosen radomly)
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator next(int action)
    {
        if (action == 1)  //set moveright to true for 1.5 seconds
        {
            moveright = true;
            animator.SetBool("Walking", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("Walking", false);
            moveright = false;
        }
        if (action == 2) //set moveleft to true for 1.5 seconds
        {
            moveleft = true;
            animator.SetBool("Walking", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false ;
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("Walking", false);
            moveleft = false;
        }
        if (action >= 3) //shoots
        {
            animator.SetBool("Shooting", true);
            if (enemyType == "PublicAgent") //if enemy is Public agent, shoots once.
            {
                StartCoroutine(ShootBullet());
                yield return new WaitForSeconds(1.5f);
            }
            if (enemyType == "Solidier") //if enemy is Solidier shoots three times.
            {
                StartCoroutine(ShootBullet());
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(ShootBullet());
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(ShootBullet());
                yield return new WaitForSeconds(1.1f);
            }
            animator.SetBool("Shooting", false);
        }
        yield return new WaitForSeconds(0.5f);
        nextMove = true; // alows next move to happen

    }
    /// <summary>
    /// spawns bulletprefab to enemys position
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootBullet() //when Method is called shoots bullet.
    {
        yield return new WaitForSeconds(0.3f);
        GameObject newobject = Instantiate(prefab, rb.position + new Vector2(0f, 0.6f), Quaternion.identity);
        newobject.name = "Bullet";
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
        if (collision.gameObject.CompareTag("Ground")) //sets grounded to true when collided with ground.
        {
            grounded = true;
        }
        if (collision.gameObject.CompareTag("Tortilla")) //Takes damage when hit by Tortilla.
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
