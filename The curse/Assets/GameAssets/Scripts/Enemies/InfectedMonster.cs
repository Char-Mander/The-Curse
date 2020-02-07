using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedMonster : Enemy
{
    [SerializeField]
    private float iniAttackDist;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float radius;

    private int phase=1;
    bool explosionActivated = false;
    Coroutine explosion;
    // [SerializeField]
    // private GameObject explosionParticle;

    public override void Update()
    {
        if (GetPhase() == 1)
        {
            base.Update();
        }
        else
        {
            if (!explosionActivated)
            {
                explosionActivated = true;
                moveSpeed = patrolSpeed;
                explosion = StartCoroutine(Explosion());
            }
            direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
            base.EnemyMovement(moveSpeed, direToPlayer);
        }
    }

    public override void Attack()
    {
        DetectPlayerInArea();
    }
    

    public override void DetectPlayerInArea()
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

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
            //Mover hacia el player
        }
        if (hit.collider.CompareTag("Player") && (canAttack || explosionActivated))
        {
            if (explosionActivated)
            {
                StopCoroutine(explosion);
                Explode();
            }
            else{
                canAttack = false;
                hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
                base.ReloadCoroutine();
            }
        }
    }

    private int GetPhase()
    {
        return phase = GetComponent<Health>().GetCurrentHealth() > GetComponent<Health>().GetMaxHealth() / 5 ? 1 : 2;
    }

    private void Explode()
    {
        direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
        distToPlayer = direToPlayer.magnitude;
        //Si está dentro de los límites
        if (distToPlayer <= radius)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direToPlayer.normalized, out hit, detectDist, lm))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage*2);
                }

            }
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(timer);
        Explode();
        GetComponent<Health>().LoseHealth(GetComponent<Health>().GetMaxHealth());
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }

}
