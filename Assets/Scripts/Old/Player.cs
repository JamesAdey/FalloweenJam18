using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private Transform spawnPoint;

    private Rigidbody2D thisRigidbody;
    public RagdollPlayer ragdoll;

    private bool isGrounded;
    [SerializeField]
    private Vector2 offset = new Vector2(0, -0.5f);
    [SerializeField]
    private LayerMask layers;
    [SerializeField]
    private float acceleration = 9;
    [SerializeField]
    private float moveSpeed = 3;
    private bool isAlive;
    private float respawnRot;
    private bool reqRespawn;
    private bool reqJump;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        ragdoll = GetComponent<RagdollPlayer>();
    }

    // Use this for initialization
    void Start()
    {
        Respawn(0f);
    }

    // Update is called once per frame
    void Update()
    {
        // buffer inputs
        reqRespawn |= Input.GetKeyUp(KeyCode.R);
        reqJump |= Input.GetKeyUp(KeyCode.W);
    }

    void FixedUpdate()
    {
       
        if (!isAlive)
        {
            if (reqRespawn)
            {
                Respawn(respawnRot);
            }
            return;
        }
        float upDir = Input.GetAxis("Vertical");
        float walkDir = Input.GetAxis("Horizontal");



        Vector2 pos = thisRigidbody.position;

        RaycastHit2D hit = Physics2D.Linecast(pos, pos + offset, layers);
        if (hit.fraction > 0) {
            isGrounded = hit.distance < 0.55f;
        }
        else
        {
            isGrounded = false;
        }
        if (ragdoll != null)
        {
            ragdoll.forcePose = isGrounded;
        }

        if(isGrounded && reqJump)
        {
            thisRigidbody.velocity += Vector2.up * 7;
        }

        Vector2 finalVel = thisRigidbody.velocity;

        finalVel.x = Mathf.MoveTowards(finalVel.x, walkDir * moveSpeed, acceleration * Time.deltaTime);


        thisRigidbody.velocity = finalVel;

        reqRespawn = false;
        reqJump = false;
    }

    public void Respawn(float rot)
    {
        
        thisRigidbody.isKinematic = false;
        thisRigidbody.position = spawnPoint.position;
        thisRigidbody.rotation = rot;

        GetRagdoll();

        isAlive = true;
    }

    private void GetRagdoll()
    {
        if (ragdoll != null)
        {
            ragdoll.Die();
            ragdoll.baseRigidbody = null;
        }
        ragdoll = RagdollPool.singleton.GetRagdoll(thisRigidbody);
    }

    public void Spring_Rebound(Vector2 velocity)
    {
        thisRigidbody.velocity += velocity;
        //thisRigidbody.angularVelocity = 0.1f;
    }

    public void Dead(Vector2 bloodPos, float rot, bool finish)
    {
        EffectsManager.singleton.BloodSplatter(bloodPos, Quaternion.Euler(0, 0, rot));
        Dead(rot, finish);
    }


    public void Dead(float rot, bool finish)
    {
        isAlive = false;
        respawnRot = rot;

        // first drop the ragdoll
        if (ragdoll != null)
        {

            ragdoll.Die();
            ragdoll.baseRigidbody = null;
            ragdoll = null;
        }

        // now respawn?
        if (finish == true)
        {
            thisRigidbody.angularVelocity = 0;
            Respawn(rot);
        }
        else
        {
            thisRigidbody.angularVelocity = 0;
            thisRigidbody.velocity = Vector2.zero;
            thisRigidbody.isKinematic = true;
        }



        // TODO... this
    }
}
