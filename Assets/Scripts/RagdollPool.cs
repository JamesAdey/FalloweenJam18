using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPool : MonoBehaviour {
    internal static RagdollPool singleton;
    private List<RagdollPlayer> ragdolls = new List<RagdollPlayer>();

    private Queue<RagdollPlayer> dead = new Queue<RagdollPlayer>();


    [SerializeField]
    private GameObject ragdollPrefab;
    [SerializeField]
    private int ragdollCount = 20;

    void Awake()
    {
        singleton = this;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        for(int i = 0; i < ragdollCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(ragdollPrefab, pos, rot);
            obj.name = "Ragdoll " + i;
            obj.transform.parent = transform;
            RagdollPlayer rg = obj.GetComponent<RagdollPlayer>();
            rg.Die();
            rg.enabled = false;
        }
    }

    public void RagdollDead(RagdollPlayer rg)
    {
        dead.Enqueue(rg);
    }

    internal RagdollPlayer GetRagdoll(Vector3 pos)
    {
        RagdollPlayer pl  = dead.Dequeue();
        pl.transform.position = pos;
        pl.enabled = true;
        pl.Resurrect(true,pos);
        return pl;
    }
}
