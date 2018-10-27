using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D thisRigidbody;
    private RagdollPlayer ragdoll;

    private bool isGrounded;
    [SerializeField]
    private Vector2 offset = new Vector2(0, -0.5f);
    [SerializeField]
    private LayerMask layers;
    [SerializeField]
    private float acceleration = 9;
    [SerializeField]
    private float moveSpeed = 3;

    

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        ragdoll = GetComponent<RagdollPlayer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        float upDir = Input.GetAxis("Vertical");
        float walkDir = Input.GetAxis("Horizontal");

        Vector2 pos = thisRigidbody.position;

        RaycastHit2D hit = Physics2D.Linecast(pos, pos + offset,layers);

        isGrounded = hit.distance < 0.55f;
        ragdoll.forcePose = isGrounded;

        Vector2 finalVel = thisRigidbody.velocity;

        finalVel.x = Mathf.MoveTowards(finalVel.x, walkDir * moveSpeed, acceleration * Time.deltaTime);


        thisRigidbody.velocity = finalVel;
        
    }

    public void Respawn(Vector2 pos, float rot, bool finish)
    {
        thisRigidbody.position = pos;
        thisRigidbody.rotation = rot;
        //thisRigidbody.velocity = Vector2.zero;
        if (finish == true)   thisRigidbody.angularVelocity = 0;
        if (finish == false)  thisRigidbody.velocity = Vector2.zero;
        ragdoll.Die();
    }
    public void Spring_Rebound(Vector2 velocity) {
        thisRigidbody.velocity += velocity;
        //thisRigidbody.angularVelocity = 0.1f;
    }
}
