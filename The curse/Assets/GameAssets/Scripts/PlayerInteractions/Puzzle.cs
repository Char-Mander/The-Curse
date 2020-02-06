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
    private int neededObjects;

    private int activeObj = 0;
    private bool activated = false;

    // Update is called once per frame
    void Update()
    {
        if (!activated && activeObj == neededObjects)
        {
            activated = true;
            StartCoroutine(Move());
        }
    }

    public int GetActiveObj() { return activeObj; }

    public void SetActiveObj(int num)
    {
        activeObj = num;
    }

    IEnumerator Move()
    {
        while(targetObj.transform.position != targetDestPos.position)
        {
            yield return new WaitForSeconds(0.3f);
            //Se mueve
        }
    }
}
