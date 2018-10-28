using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    [SerializeField]
    private Transform spawnPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("hit");
        //Player pl = collider.transform.root.GetComponent<Player>();
        PhysicsPlayer pl = collider.GetComponent<PhysicsPlayer>();
        if (pl == null)
        {
            return;
        }
        bool finish = true;
        pl.Dead(0 , finish);
    }
}
