using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortilla : MonoBehaviour
{
    [SerializeField] private float speed; //speed that the tortillas will fly
    [SerializeField] private Rigidbody2D rb; //this gameobjects rigidbody
    [SerializeField] private float range; //time after tortillas start to fall
    [SerializeField] private float spread; //Defines How big of a spread tortillas will make
    private SpriteRenderer spriterndr;
    /// <summary>
    /// Finds the direction that player is facing and shoots tortilla to that direction
    /// </summary>
    void Start()
    {
        spriterndr = gameObject.GetComponent<SpriteRenderer>(); 
        Vector2 horizontal = new Vector2(speed, 0);
        Vector2 direction = new Vector2(0, Random.Range(-5, 15));
        Playermovement playerscript = GameObject.Find("Player").GetComponent<Playermovement>();
        if (playerscript.GetDirection())
        {
            rb.velocity = (horizontal);
        }
        if (playerscript.GetDirection() == false)
        {
            rb.velocity = (-horizontal);
            spriterndr.flipX = true;
        }
        rb.AddForce(spread*direction);
        StartCoroutine(time());
        
    }

    /// <summary>
    /// this gameobject is destroyed when collides with Ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// this gameobjects time when it starts falling and if hit nothing destroyed later.
    /// </summary>
    /// <returns></returns>
    private IEnumerator time()
    {
        yield return new WaitForSeconds(range);
        rb.gravityScale = 2;
        yield return new WaitForSeconds(5-range);
        Destroy(this.gameObject);
    }
}
    
