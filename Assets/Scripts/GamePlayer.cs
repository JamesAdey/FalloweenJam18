using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{

    public PhysicsPlayer pPlayer;
    private bool reqRespawn;
    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
        // buffer inputs
        reqRespawn |= Input.GetKeyUp(KeyCode.R);
    }

    void FixedUpdate()
    {
        if (pPlayer == null)
        {
            pPlayer = PhysRagdollPool.singleton.GetNewPlayer();
            return;
        }
        if (reqRespawn && !pPlayer.IsAlive)
        {
            pPlayer = PhysRagdollPool.singleton.GetNewPlayer();
        }
        thisTransform.position = pPlayer.baseRigidbody.position;
        reqRespawn = false;
    }
}
