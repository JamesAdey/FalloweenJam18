using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Transform thisTransform;

    [SerializeField]
    private TowerTop towerTop;
    [SerializeField]
    private Vector2 topPos = Vector2.up * 21;

    private Vector2 size = Vector2.up * 3;

    [SerializeField]
    GameObject[] chunks;

    public static Tower singleton { get; private set; }

    private void Awake()
    {
        singleton = this;
        thisTransform = GetComponent<Transform>();
        towerTop.SetTargetPos(topPos);
    }

    public void Sacrificed()
    {
        int num = Random.Range(0, chunks.Length);
        Instantiate(chunks[num], topPos, Quaternion.identity, thisTransform);

        topPos += size;
        towerTop.SetTargetPos(topPos);
    }
}
