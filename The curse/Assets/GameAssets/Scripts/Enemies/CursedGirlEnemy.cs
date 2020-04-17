using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursedGirlStates { ATTACKING, TALKING, DECISION}

public class CursedGirlEnemy : Enemy
{
    public GameObject peacefulModel;
    public GameObject enemyCanvas;
    [SerializeField]
    private float iniAttackDist;
    [SerializeField]
    GameObject mechanismObj;
    [HideInInspector]
    public CursedGirlStates cursedGirlState;
    [HideInInspector]
    public bool activation = false;

    public override void Start()
    {
        peacefulModel.SetActive(false);
        cController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        cursedGirlState = CursedGirlStates.TALKING;
        enemyCanvas.SetActive(false);
    }

    public override void Update()
    {
        if (!locked)
        {
            direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
            distToPlayer = direToPlayer.magnitude;
        }

        if(distToPlayer < detectDist && !activation)
        {
            activation = true;
            mechanismObj.GetComponent<BlockMecanism>().SetActivated(false);
        }
    }
    
    public override void DetectPlayerInArea()
    {
        if (distToPlayer < iniAttackDist)
        {
            base.EnemyMovement(attackSpeed, -transform.forward);
        }
        else if (distToPlayer > endAttackDist)
        {
            base.EnemyMovement(attackSpeed, transform.forward);
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
