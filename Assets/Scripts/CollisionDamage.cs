using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour {
    [SerializeField] private bool grounded = true; //if player is on ground this is true. Otherwise false.
    private bool knockback = true; //if true player can take knockback from enemies.
    private GameObject player; //Players GameObject.
    private Playermovement playerScript; //Players Script.
    private Rigidbody2D playerrb; //Players RigidBody2D.

    private void Start()
    {
        player = GameObject.Find("Player"); //Finds player gameobject.
        playerScript = player.GetComponentInParent<Playermovement>(); //Finds Players script.
        playerrb = player.GetComponentInParent<Rigidbody2D>(); //Finds Players RigidBody2D.
    }
    /// <summary>
    /// Kills or damages player when hit by enemies or killtriggers
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Killbox")) //if collisions tag is Killbox player is killed.
        {
            playerScript.Kill();
        }
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Projectile")) //if collisions tag is Enemy or Projectile player takes damage and knockback.
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                Destroy(collision.gameObject);
            }
            if (knockback)
            {
                StartCoroutine(Knockback());
                if (playerrb.position.x < collision.transform.position.x)
                {
                    playerrb.velocity = new Vector2(-10, 10);
                }
                else
                {
                    playerrb.velocity = new Vector2(10, 10);
                }
            }
            playerScript.Hit();
        }  
    }
    /// <summary>
    /// jump allowed when triggered with ground and changes scenes when triggered with next level
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) //checks if player enters ground
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponentInParent<Animator>().SetBool("Grounded", true);
            grounded = true;
        }
        if (collision.gameObject.name == "Nextlevel")   //Changes to the next scene
        {
            GameObject.Find("Pausemenu").GetComponent<SceneChanger>().ChangeScene();
        }
    }
    /// <summary>
    /// jump is not allowed when exiting trigger. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)//checks if player exits ground
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponentInParent<Animator>().SetBool("Grounded", false   );
            grounded = false;
        }
    }
    /// <summary>
    /// time when knckback is allowed again
    /// </summary>
    /// <returns></returns>
    private IEnumerator Knockback() //Knockback cooldown
    {
        knockback = false;
        yield return new WaitForSeconds(1.5f);
        knockback = true;
    }
    /// <summary>
    /// returns grounded status
    /// </summary>
    /// <returns></returns>
    public bool GetGrounded() //returns grounded.
    {
        return this.grounded;
    }
}
