using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlEnemy : Enemy
{
    [SerializeField]
    private float teleportDistance;
    [SerializeField]
    private float teleportCoolDown;
    [SerializeField]
    private float iniAttackDist;
    [SerializeField]
    private float thirdPhaseSpeed;
    [SerializeField]
    GameObject canvas;
    
    private int currentPhase = 1;
    private bool hasSpoken = false;
    private bool canTeleport = true;
    private bool playerCanKillHer = true;

    public override void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        thirdPhaseSpeed = moveSpeed * 3;
        canvas.SetActive(false);
    }

    public override void Update()
    {
        direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
        distToPlayer = direToPlayer.magnitude;

        if (distToPlayer < detectDist && !hasSpoken)
        {
            //Suelta el diálogo 

            hasSpoken = true;
            canvas.SetActive(true);
            isAttacking = true;
        }

        if (hasSpoken)
        {
            AimPlayer();
            if (GetPhase() == 1)
            {
                Attack();
            }
            
            if (GetPhase() == 2)
            {
                Attack();
                Teleport();
            }
            else if(GetPhase() == 3 )
            {
                if (playerCanKillHer)
                {
                    moveSpeed = thirdPhaseSpeed;
                    DetectPlayerInArea();
                }
                else
                {
                    //Diálogo
                }
            }
        }
    }

    public override void Attack()
    {
        DetectPlayerInArea();
        if(canAttack)
        {
            Action();
        }
    }
    

    public void Teleport()
    {
        if (canTeleport)
        {
            canTeleport = false;
            Vector3 nextPos = new Vector3(player.transform.position.x + teleportDistance, this.transform.position.y, player.transform.position.z - teleportDistance);
            this.transform.position = nextPos;
            StartCoroutine(WaitForTeleport());
        }
    }


    public override void DetectPlayerInArea()
    {
        if (GetPhase() != 3)
        {
            if (distToPlayer < iniAttackDist)
            {
                base.EnemyMovement(moveSpeed, -transform.forward);
            }
            else if (distToPlayer > endAttackDist)
            {
                base.EnemyMovement(moveSpeed, transform.forward);
            }

        }
        else
        {
            if (distToPlayer < iniAttackDist && !canAttack)
            {
                base.EnemyMovement(moveSpeed, -transform.forward);
            }
            else
            {
                base.EnemyMovement(moveSpeed, transform.forward);
            }
        }
        
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
            //Mover hacia el player
        }
        else if (hit.collider.CompareTag("Player") && (GetPhase() == 3 && canAttack))
        {
            Teleport();
            canAttack = false;
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
            base.ReloadCoroutine();
        }
    }
    
    private int GetPhase()
    {
        int phase = -1;
        if (GetComponent<Health>().GetCurrentHealth() > 2 * GetComponent<Health>().GetMaxHealth() / 3) phase = 1;
        else if (GetComponent<Health>().GetCurrentHealth() > GetComponent<Health>().GetMaxHealth() / 3) phase = 2;
        else phase = 3;
        return phase;
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(teleportCoolDown);
        canTeleport = true;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }
}
