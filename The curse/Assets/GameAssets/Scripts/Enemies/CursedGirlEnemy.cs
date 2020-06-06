using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursedGirlStates { ATTACKING, TALKING, DECISION}

public class CursedGirlEnemy : Enemy
{
    public GameObject peacefulModel;
    public GameObject enemyCanvas;
    public float iniAttackDist;
    [SerializeField]
    GameObject mechanismObj;
    [HideInInspector]
    public CursedGirlStates cursedGirlState;
    [HideInInspector]
    public bool activation = false;

    [HideInInspector]
    public float speed;

    public override void Start()
    {
        peacefulModel.SetActive(false);
        cController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyStates.PATROL;
        cursedGirlState = CursedGirlStates.TALKING;
        enemyCanvas.SetActive(false);
        GetComponent<Health>().SetGodMode(true);
    }

    public override void Update()
    {
        if (!locked)
        {
            Vector3 playerPos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1.4f, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            Vector3 currentPos = new Vector3(this.transform.position.x, this.transform.position.y + viewOffset, this.transform.position.z);
            direToPlayer = playerPos - currentPos;
            distToPlayer = direToPlayer.magnitude;
        }

        if (distToPlayer < detectDist && !activation)
        {
            activation = true;
            //Si está sobre una montura, se baja, se deshabilita, y desaparece del área
            if (FindObjectOfType<PlayerController>().IsOnAMount())
            {
                FindObjectOfType<Mount>().PlayerGetsOff();
                FindObjectOfType<Mount>().SetIsLocked(true);
                FindObjectOfType<Mount>().transform.position = Vector3.zero;
            }

            //Si está cogiendo un objeto, lo suelta
            if (FindObjectOfType<PickUpObjects>().IsPickingAnObject())
                FindObjectOfType<PickUpObjects>().PickOrDropObject();

            FindObjectOfType<GeneralSoundManager>().ManageActionSound();
            state = EnemyStates.ATTACK;
        }
    }
    
    public override void DetectPlayerInArea()
    {
        if (distToPlayer < iniAttackDist)
        {
            base.EnemyMovement(attackSpeed, -transform.forward);
            speed = cController.velocity.magnitude;
        }
        else if (distToPlayer > endAttackDist)
        {
            base.EnemyMovement(attackSpeed, transform.forward);
            speed = cController.velocity.magnitude;
        }
        else
        {
            speed = 0;
        }
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }

    public override void ReloadCoroutine()
    {
        base.ReloadCoroutine();
    }
}
