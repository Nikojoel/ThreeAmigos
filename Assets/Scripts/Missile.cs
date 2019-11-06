using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour 
{
    private Rigidbody2D rb; //Missiles RigidBody2D
    private MeshRenderer MR; //Missiles MeshRenderer
    [SerializeField] private float speed; //Missiles Fall speed
    [SerializeField] private GameObject laser; //Laser gameobject which indicates where the missile will drop
    [SerializeField] private GameObject prefab; //Explosion gameobject
    [SerializeField] private float blinktime; //laser blink delay
    private Vector2 missileSpeed;

    // Use this for initialization
    void Start()
    {
        MR = gameObject.GetComponent<MeshRenderer>(); //Find Missiles MeshRenderer
        rb = this.gameObject.GetComponent<Rigidbody2D>();  //Find Missiles RigidBody2D
        laser.SetActive(false);
        missileSpeed = new Vector2(0.0f, -speed); //defines missileSpeed
        StartCoroutine(Launch()); //Starts launch sequence
    }
    private IEnumerator Launch()
    {

        for (int i = 0; i < 3; i++) //Blinks three times
        {
            yield return new WaitForSeconds(blinktime);
            laser.SetActive(true);
            yield return new WaitForSeconds(blinktime);
            laser.SetActive(false);
        }
        yield return new WaitForSeconds(blinktime * 5);
        rb.velocity = missileSpeed; //Sets new velocity to missile
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")||collision.gameObject.name == "Player Collider") //when missile hits ground or player it explodes.
        {
            StartCoroutine(Explode());
        }
    }
    /// <summary>
    /// Spawns an explosion that kills player if hit
    /// </summary>
    /// <returns></returns>
    private IEnumerator Explode()
    {
        
        GameObject explosion = Instantiate(prefab, gameObject.transform.position + new Vector3(0.0f, 1), Quaternion.identity);
        explosion.name = "Explosion";   
        Destroy(rb);
        Destroy(MR);
        yield return new WaitForSeconds(0.2f);
        Destroy(explosion);
        Destroy(gameObject);
    }
}
