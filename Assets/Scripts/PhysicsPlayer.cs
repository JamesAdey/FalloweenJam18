using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayer : MonoBehaviour
{

    public Transform spawnPoint;
    private Vector2 centrePos;
    private bool isGrounded;
    [SerializeField]
    private Vector2 offset = new Vector2(0, -0.5f);
    [SerializeField]
    private LayerMask layers;
    [SerializeField]
    private float acceleration = 30;
    [SerializeField]
    private float moveSpeed = 6;
    [SerializeField]
    private float jumpSpeed = 20;
    public bool IsAlive { get; private set; }
    private float respawnRot;
    private bool reqRespawn;
    private bool reqJump;

    public Rigidbody2D baseRigidbody;
    [SerializeField]
    private BoneData[] limbs;
    [SerializeField]
    private float damping = 30;

    float walkDir = 0;


    void Awake()
    {
        baseRigidbody = GetComponentInChildren<Rigidbody2D>();

        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].startPos = limbs[i].rig.position - baseRigidbody.position;
            limbs[i].startRot = limbs[i].rig.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive)
        {
            return;
        }
        reqJump |= Input.GetKeyUp(KeyCode.W);
        reqJump |= Input.GetKeyUp(KeyCode.UpArrow);
    }

    void FixedUpdate()
    {

        if (!IsAlive)
        {
            return;
        }

        DoMovePhysics();

        DoRagdollPhysics();
    }

    void DoMovePhysics()
    {
        float upDir = Input.GetAxis("Vertical");
        walkDir = Input.GetAxis("Horizontal");

        Vector2 pos = baseRigidbody.position;
        // see if we're standing on a surface?
        RaycastHit2D hit = Physics2D.Linecast(pos, pos + offset, layers);

        if (hit.fraction > 0)
        {
            // recenter if we weren't on the floor
            if (!isGrounded)
            {
                centrePos = hit.point + Vector2.up * 0.5f;
            }
        }

        isGrounded = hit.fraction > 0;



        if (isGrounded && reqJump)
        {
            baseRigidbody.velocity += Vector2.up * jumpSpeed;
            centrePos += Vector2.up;
        }


        if (!isGrounded)
        {
            Vector2 finalVel = baseRigidbody.velocity;
            finalVel.x = Mathf.MoveTowards(finalVel.x, walkDir * moveSpeed, acceleration * Time.deltaTime);
            baseRigidbody.velocity = finalVel;
        }

        centrePos.x += walkDir * moveSpeed * Time.deltaTime;

        reqRespawn = false;
        reqJump = false;
    }

    void DoRagdollPhysics()
    {
        if (!IsAlive || baseRigidbody == null)
        {
            return;
        }
        Vector2 basePos = centrePos;
        for (int i = 0; i < limbs.Length; i++)
        {
            BoneData bone = limbs[i];


            // compute position offset
            Vector2 bonePos = bone.rig.position;                        // where the bone currently is
            Vector2 desiredPos = basePos + bone.offset;                 // where we want to be
            if (!isGrounded)
            {
                float diff = (walkDir * moveSpeed) - bone.rig.velocity.x;
                bone.rig.AddForce(Vector2.right * diff * damping);
                return;
            }
            Vector2 predictedPos = bone.rig.velocity + bonePos;         // where we will be in a second
            float d = damping;
            Vector2 bodyDisp = ((desiredPos - predictedPos) / Mathf.Log(d)) + desiredPos - bonePos;

            bone.rig.AddForce(bodyDisp * damping);
        }
    }

    public void Respawn(float rot)
    {

        baseRigidbody.isKinematic = false;
        baseRigidbody.position = spawnPoint.position;
        baseRigidbody.rotation = rot;

        Resurrect(true);

        IsAlive = true;
    }

    public void Spring_Rebound(Vector2 velocity)
    {
        baseRigidbody.velocity += velocity;
        //thisRigidbody.angularVelocity = 0.1f;
    }

    public void Dead(Vector2 bloodPos, float rot, bool finish)
    {
        Debug.Log("DEAD");
        if (IsAlive)
        {
            EffectsManager.singleton.BloodSplatter(bloodPos, Quaternion.Euler(0, 0, rot));
            Dead(rot, finish);
        }
    }

    public void Dead(float rot, bool finish)
    {
        IsAlive = false;
        respawnRot = rot;

        Die();

        // now respawn?
        if (finish == true)
        {
            Respawn(rot);
        }

        // TODO... this
    }

    private void OnEnable()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].rig.isKinematic = false;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].rig.isKinematic = true;
        }
    }

    public void Die()
    {
        ToggleJoints(false);
        IsAlive = false;
        PhysRagdollPool.singleton.RagdollDead(this);        // add this ragdoll to the death list
    }

    public void Resurrect(bool instant)
    {
        baseRigidbody.position = spawnPoint.position;
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].rig.WakeUp();
            limbs[i].rig.velocity = baseRigidbody.velocity;
            limbs[i].rig.angularVelocity = baseRigidbody.angularVelocity;
            if (instant)
            {
                limbs[i].rig.position = baseRigidbody.position + limbs[i].startPos;
                limbs[i].rig.rotation = limbs[i].startRot;
            }
        }
        ToggleJoints(true);
        IsAlive = true;
    }

    private void ToggleJoints(bool on)
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            if (limbs[i].joint != null)
            {
                limbs[i].joint.enabled = on;
            }
        }
    }
}
