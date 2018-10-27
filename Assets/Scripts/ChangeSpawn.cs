using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpawn : MonoBehaviour {

    private Rigidbody2D thisRigidbody;
    [SerializeField]
    private Transform spawnPoint;
    private int deltaSpawn = 5;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void MoveSpawn(){

        thisRigidbody.velocity += Vector2.up * deltaSpawn;
    }
}
