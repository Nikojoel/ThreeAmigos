    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour {
    private GameObject player; //players Gameobject
    private Vector3 offset; //Difference between players and cameras position
    [SerializeField] private float speed;


	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player"); //Finds player in Hierarchy. 
        offset = transform.position - player.transform.position; //Defines cameras offset to player.
	}
    
    /// <summary>
    /// follows players movement
    /// </summary>
	void FixedUpdate () {
        transform.position = player.transform.position/speed + offset; //updates cameras position.
	}
}
