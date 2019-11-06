using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {
    [SerializeField] private float health; //This is health of the Boss.
    [SerializeField] private float speed; //This is the speed in x axis when boss jumps.
    [SerializeField] private GameObject Bullet; //This gameobject is Boss Bullet and is taken from prefabs when Boss shoots.
    [SerializeField] private GameObject Missile; //This gameobject is Boss Missiletrigger and is taken from prefabs when Bosses second phase starts.
    [SerializeField] private GameObject Solidier; //This gameobject is Boss Solidiertrigger and is taken from prefabs when Bosses second phase starts.
    [SerializeField] private float jump; //This is the speed in y axis when boss jumps.
    private GameObject player; //Players Gameobject. 
    private bool nextMove = true; //When nextMove is true, Boss starts its next move.
    private Rigidbody2D rb; //This is bThe Bosses RigidBody2D.
    private bool activate = false; //when active is false, Boss does nothing and takes no damage.
    private bool secondPhase = false; //when active is true, second phase of Boss starts.
    private BossPlatform bossPlatform; //This is Bossplatforms script and its called in second phase.
    private Image bossHealthBar; //this is Boss healthbar. 
    private AudioSource hitSound; // Audiosource used as the boss hitsound
    private GameObject nextLevel; // Gameobject used as the collider that ends the game
    private GameObject speakBubble1; // Gameobject that displays a speakbubble
    private GameObject speakBubble2;
    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
        speakBubble1 = GameObject.Find("Image"); // Finds the speakbubble gameobject from the hierarchy
        speakBubble2 = GameObject.Find("Imagetwo");
        speakBubble1.SetActive(true); // Sets the activity of the  speakbubble to true
        speakBubble2.SetActive(false); // Sets the activity of the another speakbubble to false
        nextLevel = GameObject.Find("Nextlevel"); // Finds the nextlevel gameobject from the hierarchy
        nextLevel.SetActive(false); // Sets the activity of the gameobject to false
        rb = gameObject.GetComponent<Rigidbody2D>(); //Defines Bosses Rigidbody2D.
        player = GameObject.Find("Player"); //Finds Players Gameobject in Hierarchy.
        StartCoroutine(StartTime());//Starts startTime() named IEnumerator.
        bossPlatform = GameObject.Find("Boss Platform").GetComponent<BossPlatform>(); //Finds BossPlatforms Script in Hierarchy.
        bossHealthBar = GameObject.Find("Boss Health").GetComponent<Image>(); //Finds Bosses Healthbar in Hierarchy.
        hitSound = GameObject.Find("Hitsound").GetComponent<AudioSource>(); // Finds the hitsound gameobject from the hierarchy
    }
	
	/// <summary>
    /// Core of bossfight
    /// </summary>
	void FixedUpdate () {
        if (health <= 50 && secondPhase == false) //When Health is 50 or less, second hase of boss starts.
        {
            activate = false; //Boss stops attacking and becomes invunerable.
            if (nextMove)
            {
                secondPhase = true; //This makes that there cannot be another second phase.
                StartCoroutine(SecondPhase()); //Starts the scripted second phase.
            }
        }
        if (nextMove && activate) //When nextmove is true, Boss does its next move.
        {
            nextMove = false; //This restricts multiple next moves happening.
            StartCoroutine(next(Random.Range(1, 3))); //Starts randomly one of two Bosses moves.
        }
        if (health <= 0) //When Bosses health reach zero boss is dead and game is won.
        {
            Destroy(this.gameObject); 
            GameObject.Find("MissileTrigger").SetActive(false);
            GameObject.Find("Timer").GetComponent<Timer>().CancelInvoke(); // Cancels invoking the timer and freezez the timer
            GameObject.Find("Music").GetComponent<AudioSource>().Stop(); // Stops the audiosource 
            nextLevel.SetActive(true);
        }
        if (player.transform.position.x < rb.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    /// <summary>
    /// action that the boss does next
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator next(int action) //here is defined what bBoss does on his next move.
    {
        if (action == 1) //Boss jumps towards player.
        {
            if (player.transform.position.x < rb.position.x)
            {
                rb.AddForce(new Vector2(-speed, jump));
            }
            else
            {
                rb.AddForce(new Vector2(speed, jump));
            }
            yield return new WaitForSeconds(1.5f);
        }
        if (action == 2) //Boss shoots 20 bullets towards player
        {
            animator.SetBool("Shoot", true);
            int count = 0;
            do
            {
                GameObject Bullets;
                if (player.transform.position.x < rb.position.x)
                {
                    Bullets = Instantiate(Bullet, rb.position + new Vector2(-0.8f,-0.7f), Quaternion.identity); //Spawns new Boss Bullet.
                }
                else
                {
                    Bullets = Instantiate(Bullet, rb.position + new Vector2(0.8f, -0.7f), Quaternion.identity); //Spawns new Boss Bullet.
                }
                count++;
                Bullets.name = "Bullet";
                yield return new WaitForSeconds(0.3f);
            } while (count <= 20 && activate); //Stops shooting after 20 bullets or when second phase is starting.
            animator.SetBool("Shoot", false);
        }
        yield return new WaitForSeconds(1.5f);
        nextMove = true; //Allows next move to start.
    }
    /// <summary>
    /// boss is invunerable during this time and start moving after this.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTime() // this is only five second countdown at the beginning of bossfight.
    {
        animator.SetBool("Shout", true);
        yield return new WaitForSeconds(5);
        animator.SetBool("Shout", false);
        speakBubble1.SetActive(false);
        activate = true;
    }
    /// <summary>
    /// reduces one of bosses health when he is not invunerable
    /// </summary>
    public void Hit() //when tortilla hits boss this is called and Boss loses one health.
    {
        if (activate){ //Boss wont take damage if he is not active.
            health--;
            hitSound.Play();
            bossHealthBar.fillAmount -= 0.01f; //Updates Bosses healthbar.
        } 
    }
    /// <summary>
    /// when tortilla hits boss, reduces bosses health by one. also for animator
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tortilla")) //if collisions tag is Tortilla Hit Method is triggered.
        {
            Destroy(collision.gameObject); //Destroys collided tortilla.
            Hit();
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
        }
    }
    /// <summary>
    /// For animator
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", true);
        }
    }
    /// <summary>
    /// Bosses second plahse starts and he jumps up to a platform.
    /// Boss is also invunerable this time
    /// </summary>
    /// <returns></returns>
    private IEnumerator SecondPhase() //This is scripted second phase of bossfight.
    {
        speakBubble2.SetActive(true);
        animator.SetBool("Shout", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Shout", false);
        speakBubble2.SetActive(false);
        rb.velocity = new Vector2(0.0f, 50f); //Boss jumps straight up.
        bossPlatform.Activate(); //Bossplatform comes and catches Boss in the air.
        GameObject Missiletrg = Instantiate(Missile, rb.position, Quaternion.identity); //Spawns Missiletrigger which starts dropping missiles in the bossarena.
        Missiletrg.name = "MissileTrigger";
        GameObject Solidiertrg = Instantiate(Solidier, rb.position, Quaternion.identity); //Spawns Solidiertrigger which starts dropping solidiers in the bossarena.
        Solidiertrg.name = "SolidierTrigger";
        yield return new WaitForSeconds(20); // Boss waits 20 seconds on the platform and then jumps down.
        Destroy(Solidiertrg); //Stops the Spawning of solidiers. Missiles still keep on falling down.
        bossPlatform.Deactivate(); //Bossplatform leaves the arena.
        yield return new WaitForSeconds(2);
        activate = true; //Boss starts fighting again.
    }
}
    