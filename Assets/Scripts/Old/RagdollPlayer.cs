using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoneData
{
    public Rigidbody2D rig;
    public Collider2D col;
    public Vector2 offset;
    public Joint2D joint;
    [System.NonSerialized]
    public Vector2 startPos;
    public bool alwaysSim;
    [System.NonSerialized]
    public float startRot;
}

public class RagdollPlayer : MonoBehaviour
{


    public Rigidbody2D baseRigidbody;
    [SerializeField]
    private BoneData[] limbs;
    [SerializeField]
    private float damping = 5;
    public bool forcePose;
    private bool dead;

    private void Awake()
    {
        dead = false;

        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].startPos = limbs[i].rig.position;
        }
    }

    private void FixedUpdate()
    {
        if (dead || baseRigidbody == null)
        {
            return;
        }
        Vector2 basePos = baseRigidbody.position;
        for (int i = 0; i < limbs.Length; i++)
        {
            BoneData bone = limbs[i];
            // compute position offset
            Vector2 bonePos = bone.rig.position;                        // where the bone currently is
            Vector2 desiredPos = basePos + bone.offset;                 // where we want to be
            Vector2 predictedPos = bone.rig.velocity + bonePos;         // where we will be in a second
            Vector2 bodyDisp = ((desiredPos - predictedPos) / damping) + desiredPos - bonePos;

            bone.rig.AddForce(bodyDisp * damping);
        }
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
        dead = true;
        RagdollPool.singleton.RagdollDead(this);        // add this ragdoll to the death area
    }

    public void Resurrect(bool instant, Rigidbody2D connected)
    {
        baseRigidbody = connected;
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].rig.WakeUp();
            limbs[i].rig.velocity = connected.velocity;
            limbs[i].rig.angularVelocity = connected.angularVelocity;
            if (instant)
            {
                limbs[i].rig.position = connected.position + limbs[i].startPos;
            }
        }
        ToggleJoints(true);
        dead = false;
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
