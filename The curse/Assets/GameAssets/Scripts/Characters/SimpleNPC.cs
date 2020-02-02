using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPC : NPC
{
    // Update is called once per frame
    [SerializeField]
    bool characterCanRotate;
    public override void Update()
    {
        if (canRotate && characterCanRotate)
        {
            canRotate = false;
            CallRotateCoroutine();
        }
    }
}
