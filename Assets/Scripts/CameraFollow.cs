
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public Transform target;
    [SerializeField]
    private Vector3 offset = Vector3.back * 10;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        thisTransform.position = target.position + offset;
	}
}
