using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    GameObject targetObj;
    [SerializeField]
    Transform targetDestPos;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    private int neededObjects;

    private int activeObj = 0;
    Vector3 dir;
    private bool activated = false;
    private bool reached = false;
    private float distance;


    // Update is called once per frame
    void Update()
    {
        if (!activated && activeObj == neededObjects)
        {
            print("Se activa: " + neededObjects);
            activated = true;
            dir = targetDestPos.position - targetObj.transform.position;
            distance = dir.magnitude;
        }
        else if(activated && !reached)
        {
            Move();
        }
    }

    private void Move()
    {
        targetObj.transform.position += dir * moveSpeed * Time.deltaTime;
        distance -= dir.magnitude * moveSpeed * Time.deltaTime;
        if (distance <= 0) reached = true;
    }

    public int GetActiveObj() { return activeObj; }

    public void SetActiveObj(int num)
    {
        activeObj = num;
    }
}
