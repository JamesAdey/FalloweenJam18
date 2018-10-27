using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_Block : MonoBehaviour {
    [SerializeField]
    private float spring_power = 10;
    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D collider) {
        Debug.Log("Boing");
        Player pl = collider.transform.root.GetComponent<Player>();
        pl.Spring_Rebound(Vector2.up * spring_power);

    }
}
