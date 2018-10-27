using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D thisRigidbody;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Respawn(Vector2 pos, float rot)
    {
        thisRigidbody.position = pos;
        thisRigidbody.rotation = rot;
        //thisRigidbody.velocity = Vector2.zero;
        thisRigidbody.angularVelocity = 0;
    }
}
