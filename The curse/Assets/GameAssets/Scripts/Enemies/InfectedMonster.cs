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
    bool hasExploded = false;
    Coroutine explosion;
    // [SerializeField]
    // private GameObject explosionParticle;

    public override void Update()
    {
        if (!locked)
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
                    attackSpeed = patrolSpeed * 2;
                    explosion = StartCoroutine(Explosion());
                }
                direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
                base.EnemyMovement(attackSpeed, direToPlayer.normalized);
                AimPlayer();
            }
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
            base.EnemyMovement(attackSpeed, -transform.forward);
        }
        else
        {
            base.EnemyMovement(attackSpeed, transform.forward);
        }
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.GetComponentInParent<PlayerController>() == null && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
            //Mover hacia el player
        }
        if (hit.collider.GetComponentInParent<PlayerController>() != null && (canAttack || explosionActivated))
        {
            if (explosionActivated && !hasExploded)
            {
                hasExploded = true;
                StopCoroutine(explosion);
                Explode();
            }
            else{
                hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
                base.ReloadCoroutine();
            }
            canAttack = false;
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
                if (hit.collider.GetComponentInParent<PlayerController>() != null)
                {
                    hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage*2);
                }

            }
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(timer);
        hasExploded = true;
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
