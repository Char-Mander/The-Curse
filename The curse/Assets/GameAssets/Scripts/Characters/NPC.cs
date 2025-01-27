﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public const float gravity = -9.8f;
    
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
    Animator anim;
    bool isMoving = false;
    bool isTalking = false;
    Coroutine wpStop;
    int wpIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (wayPoints.Count > 0) Init();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(wayPoints.Count > 0) Patrol();
    }

    public void Init()
    {
        isMoving = false;
        wpStop = StartCoroutine(WaitOnWP());
        agent.speed = moveSpeed;
    }

    void Patrol()
    {
        anim.SetFloat("Speed", agent.speed);
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
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(wayPoints[wpIndex].position);
        wpIndex++;
        isMoving = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
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
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            detected = false;
            head.transform.rotation = this.transform.rotation;
        }
    }
    
    
    public void StartTalking()
    {
        if (!isTalking)
        {
            print("Empieza a hablar");
            isTalking = true;
            anim.SetLayerWeight(0, 0.5f);
            anim.SetLayerWeight(1, 0.5f);
            StartCoroutine(StopTalking());
        }
    }

    IEnumerator StopTalking()
    {
        yield return new WaitForSeconds(2);
        print("Deja de hablar");
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(0, 1);
        isTalking = false;
    }
}
