using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPC : NPC
{
    // Update is called once per frame
    public override void Update()
    {
        if (canRotate)
        {
            canRotate = false;
            CallRotateCoroutine();
        }
    }
}
