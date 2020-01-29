using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public const float gravity = -9.8f;

    CharacterController cController;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    Transform head;

    GameObject player;
    Vector3 direToPlayer;
    Quaternion iniRotation;
    bool detected = false;
    [HideInInspector]
    public bool canRotate = true;
    // Start is called before the first frame update
    public virtual void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Movement(moveSpeed, transform.forward);
        if (canRotate)
        {
            canRotate = false;
            CallRotateCoroutine();
        }
    }

    void Movement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime);
    }

    public void CallRotateCoroutine()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        canRotate = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Terrain") && !hit.collider.CompareTag("Player"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!detected)
            {
                detected = true;
            }
            direToPlayer = player.transform.position - this.transform.position;
            Vector3 aux = direToPlayer;
            aux.y = 0;
            head.transform.rotation = Quaternion.LookRotation(direToPlayer);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")/* && detected*/)
        {
            detected = false;
            head.transform.rotation = this.transform.rotation;
        }
    }
}
