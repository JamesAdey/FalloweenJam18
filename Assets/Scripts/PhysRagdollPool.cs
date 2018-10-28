using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysRagdollPool : MonoBehaviour {

    internal static PhysRagdollPool singleton;

    public Transform worldSpawn;

    private Queue<PhysicsPlayer> dead = new Queue<PhysicsPlayer>();


    [SerializeField]
    private GameObject ragdollPrefab;
    [SerializeField]
    private int ragdollCount = 20;

    void Awake()
    {
        singleton = this;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        for (int i = 0; i < ragdollCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(ragdollPrefab, pos, rot);
            obj.name = "PhysDoll " + i;
          
            PhysicsPlayer rg = obj.GetComponent<PhysicsPlayer>();
            rg.spawnPoint = worldSpawn;
            rg.Die();
            rg.enabled = false;
        }
    }

    public void RagdollDead(PhysicsPlayer rg)
    {
        dead.Enqueue(rg);
    }

    internal PhysicsPlayer GetNewPlayer()
    {
        PhysicsPlayer pl = dead.Dequeue();
        pl.enabled = true;
        pl.Resurrect(true);
        return pl;
    }
}
