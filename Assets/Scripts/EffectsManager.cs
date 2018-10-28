using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public static EffectsManager singleton { get; private set; }
    [SerializeField]
    private GameObject[] bloodSplatters;

    private void Awake()
    {
        singleton = this;
    }

    public void BloodSplatter(Vector3 pos, Quaternion rot)
    {
        int spriteIndex = Random.Range(0, bloodSplatters.Length);
        GameObject newObj = (GameObject)Instantiate(bloodSplatters[spriteIndex], pos, rot);
        newObj.transform.parent = transform;
    }

}
