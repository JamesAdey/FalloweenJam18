﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    [SerializeField]
    private Transform spawnPoint;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D collider)
    {
        //Debug.Log("hit");
        Player pl = collider.transform.root.GetComponent<Player>();
        if (pl == null)
        {
            return;
        }
        bool finish = false;
        pl.Respawn(spawnPoint.position, 0,finish);
    }
}