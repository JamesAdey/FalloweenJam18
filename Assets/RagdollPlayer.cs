using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoneData
{
    public Rigidbody2D rig;
    public Vector2 offset;
    public Joint2D joint;
    [System.NonSerialized]
    public Vector2 startPos;
}

public class RagdollPlayer : MonoBehaviour {


    private Rigidbody2D thisRigidbody;
    [SerializeField]
    private BoneData[] limbs;
    [SerializeField]
    private float damping = 5;
    public bool forcePose;
    private bool dead;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        dead = false;

        for(int i = 0; i < limbs.Length; i++)
        {
            limbs[i].startPos = limbs[i].rig.position;
        }
    }

    private void FixedUpdate()
    {
        if (!forcePose || dead)
        {
            return;
        }
        Vector2 basePos = thisRigidbody.position;
        Vector2 baseForce = -Physics2D.gravity;
        for (int i = 0; i < limbs.Length; i++)
        {
            BoneData bone = limbs[i];
            // compute position offset
            Vector2 bonePos = bone.rig.position;                        // where the bone currently is
            Vector2 desiredPos = basePos + bone.offset;                 // where we want to be
            Vector2 predictedPos = bone.rig.velocity + bonePos;         // where we will be in a second
            Vector2 bodyDisp = ((desiredPos - predictedPos)/Mathf.Log(damping)) + desiredPos - bonePos;
            
            bone.rig.AddForce(bodyDisp * damping);
        }
    }

    public void Die()
    {
        ToggleJoints(false);
        dead = true;
    }

    public void Resurrect()
    {
        ToggleJoints(true);
        dead = false;
    }

    private void ToggleJoints(bool on)
    {
        for(int i = 0; i < limbs.Length; i++)
        {
            if(limbs[i].joint != null)
            {
                limbs[i].joint.enabled = on;
            }
        }
    }
}
