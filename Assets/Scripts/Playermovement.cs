using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playermovement : MonoBehaviour {
    [SerializeField] private float acceleration; //how fast layer gets to his max speed
    [SerializeField] private int health; //players healthpoints
    [SerializeField] private float Maxspeed; //max movementspeed
    [SerializeField] private float jumpHeight; //How high player jumps
    [SerializeField] private Rigidbody2D rb;  //players rigidbody
    [SerializeField] private GameObject prefab; //tortilla that player throws
    [SerializeField] private float attackspeed; //how fast player throws tortillas
    private bool throwAllow = true; //allows next tortilla to be thrown
    private CollisionDamage collisionScript; //Players collisionscript
    private Vector2 jump; //Jump Vector
    [SerializeField] private bool direction = true; //direction that player is facing
    private bool invunerability = false; //player takes no damage when true
    private SpriteRenderer playerMR; //players meshrenderer
    private GameObject hp1; //hp1 gameobject
    private GameObject hp2; //hp2 gameobject
    private GameObject hp3; //hp3 gameobject
    private AudioSource playerHitSound; // Audiosource used as the hitsound
    private Animator animator;

    /// <summary>
    /// Finding needed gameobjects and components
    /// </summary>
    private void Start () {
        playerMR = gameObject.GetComponent<SpriteRenderer>();
        collisionScript = GameObject.Find("Player Collider").GetComponent<CollisionDamage>();
        hp1 = GameObject.Find("1hp");
        hp2 = GameObject.Find("2hp");
        hp3 = GameObject.Find("3hp");
        playerHitSound = GameObject.Find("Playerhitsound").GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	/// <summary>
    /// players movement and shooting happens here. Also when player is killed.
    /// </summary>
	void FixedUpdate () {
        jump = new Vector2(rb.velocity.x, jumpHeight/30);
        if (rb.velocity.x <  Maxspeed && rb.velocity.x > -Maxspeed)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x - acceleration, rb.velocity.y);
                playerMR.flipX = true;
                direction = false;
            }
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x + acceleration, rb.velocity.y);
                playerMR.flipX = false;
                direction = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x / 1.08f, rb.velocity.y);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && throwAllow)
        {
            StartCoroutine(Throw());
        }
        if (Input.GetKey(KeyCode.Space) && rb.velocity.y >= 0)
        {
            rb.AddForce(new Vector2(0.0f, 37f));
            if (Input.GetKey(KeyCode.Space) && collisionScript.GetGrounded())
            {
                rb.velocity = jump;
            }
        }
        if (health <= 0)
        {
            GameObject.Find("Timer").GetComponent<Timer>().SetText();
            GameObject.Find("Pausemenu").GetComponent<SceneChanger>().StopGame();
            Destroy(gameObject.GetComponent<SpriteRenderer  >());
            Destroy(gameObject.GetComponent<Playermovement>());
        }
        if (rb.velocity.x >= 0.5 || rb.velocity.x <= -0.5)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        if (Input.GetKey(KeyCode.LeftShift)){
            animator.SetBool("Throw", true);
        }else if (throwAllow)
        {
            animator.SetBool("Throw", false);
        }
    }
    
    /// <summary>
    /// Timer based method that throws tortillas at adjustable rate
    /// </summary>
    /// <returns></returns>
    private IEnumerator Throw()
    {
        throwAllow = false;
        yield return new WaitForSeconds(attackspeed);
        Vector2 position = transform.position;
        GameObject newobject = Instantiate(prefab, position, Quaternion.identity);
        newobject.name = "Tortilla";
        throwAllow = true;
        
    }
    /// <summary>
    /// when player is not invunerable, loses one health and gives invunerability.
    /// </summary>
    public void Hit()
    {
        if (invunerability == false)
        {
            health--;
            UpdateHealth();
            StartCoroutine(invunerableTime());
            StartCoroutine(invunerable());
        }
    }
    /// <summary>
    /// heals player for one hitpoint.
    /// </summary>
    public void Heal()
    {
        health++;
        UpdateHealth();
    }
    /// <summary>
    /// Updates health values on screen.
    /// </summary>
    private void UpdateHealth()
    {
        if (health == 3)
        {
            hp1.SetActive(true);
            hp2.SetActive(true);
            hp3.SetActive(true);
        }
        if (health == 2)
        {
            hp2.SetActive(true);
            hp1.SetActive(true);
            hp3.SetActive(false);
            playerHitSound.Play();
        }
        if (health == 1)
        {
            hp1.SetActive(true);
            hp2.SetActive(false);
            hp3.SetActive(false);
            playerHitSound.Play();
        }
        if (health == 0)
        {
            hp1.SetActive(false);
            hp2.SetActive(false);
            hp3.SetActive(false);
        }
    }
    /// <summary>
    /// Makes Blinking effect to player gameobject when invunerability is true
    /// </summary>
    /// <returns></returns>
    private IEnumerator invunerable()
    {
        do
        {
            playerMR.enabled = false;
            yield return new WaitForSecondsRealtime(0.08f);
            playerMR.enabled = true;
            yield return new WaitForSecondsRealtime(0.08f);
        } while (invunerability);
    }
    /// <summary>
    /// Sets invunerability is true for 1.5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator invunerableTime()
    {
        invunerability = true;
        yield return new WaitForSecondsRealtime(1.5f);
        invunerability = false;
    }
    /// <summary>
    /// Sets players health to zero
    /// </summary>
    public void Kill()
    {
        health = 0;
        UpdateHealth();
    }
    /// <summary>
    /// sets new attackspeed
    /// </summary>
    /// <param name="atkspd"></param>
    public void SetAttkspd(float atkspd)
    {
        this.attackspeed = atkspd;
    }
    /// <summary>
    /// returns players health
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return this.health;
    }
    /// <summary>
    /// Sets players health
    /// </summary>
    /// <param name="newhealth"></param>
    public void SetHealth(int newhealth)
    {
        this.health = newhealth;
    }
    /// <summary>
    /// returns players facing direction
    /// </summary>
    /// <returns></returns>
    public bool GetDirection()
    {
        return this.direction;
    }
}
