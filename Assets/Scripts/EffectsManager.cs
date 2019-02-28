using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{

    public static EffectsManager singleton { get; private set; }
    [SerializeField]
    private GameObject[] bloodSplatters;

    [SerializeField]
    private GameObject bloodParticles;


    private void Awake()
    {
        singleton = this;
    }

    public void BloodSplatter(Vector3 pos, Quaternion rot)
    {
        int spriteIndex = Random.Range(0, bloodSplatters.Length);
        // 2d splatters
        GameObject newObj = (GameObject)Instantiate(bloodSplatters[spriteIndex], pos,rot);
        newObj.transform.parent = transform;

        // particles
        Instantiate(bloodParticles, pos, Quaternion.Euler(-90,0,0));
    }

}
