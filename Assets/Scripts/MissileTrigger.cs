using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTrigger : MonoBehaviour {
    [SerializeField] private GameObject prefab; //Gameobject that will spawn.
    [SerializeField] private float spawntime; //time between Spawned items.
    [SerializeField] private bool findPlayer; //if true, Gameobjects will spawn near player. Otherwise a defined spot. 
    private GameObject newobject;
    private GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        StartCoroutine(Delay());
	}
    /// <summary>
    /// Here is defined the time between Gameobject spawns.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(spawntime);
        if (findPlayer) //when this is true this will pawn gameobject on top of the player 20 units high
        {
            newobject = Instantiate(prefab, player.transform.position + new Vector3(Random.Range(-10, 11) / 2, 20f, 0.0f), Quaternion.identity);
        }
        else //this will spawn Gameobjects between -25 and 14 on x axis and 20 units high.(Boss arena)
        {
            newobject = Instantiate(prefab, new Vector2(Random.Range(-25, 15), 20f), Quaternion.identity);
        }
        newobject.name = prefab.name;
        StartCoroutine(Delay());
    }
}
    