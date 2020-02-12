using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMecanism : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector3 iniPos;
    private bool activated = false;
    private int direction = 0;

    private void Start()
    {
        iniPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(activated && (this.gameObject.transform.position - iniPos).magnitude >= 0)
        {
            if(direction == 0)
            {
                if (this.gameObject.transform.position.magnitude > iniPos.magnitude) direction = 1;
                else direction = -1;
            }
            Move();
        }
    }

    private void Move()
    {
        if(direction == 1) this.transform.position -= (this.gameObject.transform.position - iniPos) * moveSpeed * Time.deltaTime;
        else this.transform.position += (this.gameObject.transform.position - iniPos) * moveSpeed * Time.deltaTime;
    }

    public bool GetActivated() { return activated; }

    public void SetActivated(bool value)
    {
        activated = value;
    }
}
