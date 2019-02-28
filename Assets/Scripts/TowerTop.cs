using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTop : MonoBehaviour {

    public float speed = 7;
    Vector2 desiredPos;

    public void SetTargetPos(Vector2 p)
    {
        desiredPos = p;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, desiredPos, Time.deltaTime * speed);
    }
}
