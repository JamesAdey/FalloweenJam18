using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField]
    private Vector2 topPos = Vector2.up * 21;

    [SerializeField]
    GameObject[] chunks;

    public static Tower singleton { get; private set; }

    private void Awake()
    {
        singleton = this;
    }

    public void AddChunk()
}
