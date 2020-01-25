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
    // Start is called before the first frame update
    void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Movement(moveSpeed, transform.forward);
        StartCoroutine(Rotate());
    }

    void Movement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime);
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        StartCoroutine(Rotate());
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            direToPlayer = player.transform.position - this.transform.position;
            Vector3 aux = direToPlayer;
            aux.y = 0;
            transform.rotation = Quaternion.LookRotation(aux, Vector3.up);
            Vector3 playerHead = new Vector3(player.gameObject.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);
            head.LookAt(playerHead);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            head.transform.rotation = this.transform.rotation;
        }
    }
}
