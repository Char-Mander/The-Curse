using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasController : MonoBehaviour
{
    [SerializeField]
    float offset = 1.5f;
    Transform playerTransform;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerTransform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + offset, GameObject.FindGameObjectWithTag("Player").transform.position.z);
    }


    public virtual void Update()
    {
        this.gameObject.transform.LookAt(playerTransform);
    }
}
