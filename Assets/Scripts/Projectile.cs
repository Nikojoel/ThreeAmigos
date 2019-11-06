using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private GameObject player; //players gameobject 
    [SerializeField] private float speed; //this gameobjects speed
    private Rigidbody2D rb; //this gameobjects RigidBody2D
    [SerializeField] private float spread; //Defines How big of a spread bullets will make

    

	/// <summary>
    /// Here this gameobject finds player and shoots itself towards it
    /// </summary>
	void Start () {
        Vector2 moveHorizontal = new Vector2(speed, 0);
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        if (rb.position.x < player.transform.position.x)
        {
            rb.velocity = (moveHorizontal);
            gameObject.GetComponent<SpriteRenderer>().flipX = true; 
        }
        else
        {
            rb.velocity = (-moveHorizontal);
        }
        rb.AddForce(new Vector2(0.0f, Random.Range(-5, 15) * spread));
        StartCoroutine(Time());
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
    /// Time for Bullet to be destroyed if hit nothing.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Time()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
    
}
