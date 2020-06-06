using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasController : MonoBehaviour
{

    public virtual void Update()
    {
        this.gameObject.transform.LookAt(FindObjectOfType<PlayerController>().GetPlayerHeadTransform());
    }
    
}
