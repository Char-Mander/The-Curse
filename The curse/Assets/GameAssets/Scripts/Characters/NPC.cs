using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    bool detected = false;

    [SerializeField]
    public float waitingTime;
    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    bool isMoving = false;
    Coroutine wpStop;
    float mosquedWaitingTime;
    int wpIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        agent = GetComponent<NavMeshAgent>();
        Init();
    }
    
    // Update is called once per frame
    void Update()
    {
        Patroll();
    }

    public void Init()
    {
        isMoving = false;
        wpStop = StartCoroutine(WaitOnWP());
        agent.speed = moveSpeed;
    }

    void Patroll()
    {
        if (wpIndex < wayPoints.Count)
        {
            if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
            {
                wpStop = StartCoroutine(WaitOnWP());
                isMoving = false;
            }
        }
        else
        {
            wpIndex = 0;
        }
    }

    IEnumerator WaitOnWP()
    {
        yield return new WaitForSeconds(mosquedWaitingTime);
        agent.SetDestination(wayPoints[wpIndex].position);
        wpIndex++;
        isMoving = true;
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
        if (other.CompareTag("Player"))
        {
            detected = false;
            head.transform.rotation = this.transform.rotation;
        }
    }


}
