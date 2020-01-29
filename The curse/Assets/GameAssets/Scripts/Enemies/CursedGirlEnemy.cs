﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlEnemy : Enemy
{
    private int currentPhase = 1;
    private bool hasSpoken = false;
    [SerializeField]
    private float iniAttackDist;
    [SerializeField]
    GameObject canvas;

    public override void Start()
    {
        base.Start();
        canvas.SetActive(false);
    }

    public override void Update()
    {
        if (hasSpoken)
        {
            if(!canvas.active)
                canvas.SetActive(true);
            if(!isAttacking) isAttacking = true;
            AimPlayer();
            Attack();

            if (GetPhase() == 1)
            {
            }
            else if (GetPhase() == 2)
            {
                //Lanza objetos, los cuales se instancian y se aplican una fuerza
            }
            else
            {
                //Ataca cuerpo a cuerpo
            }
        }
    }

    public override void Attack()
    {
        DetectPlayerInArea();
        if(GetPhase() != 3 && canAttack)
        {
            Action();
        }
    }


    public override void DetectPlayerInArea()
    {
        if (distToPlayer < iniAttackDist)
        {
            if (GetPhase() == 3  && !canAttack)
            base.EnemyMovement(moveSpeed, -transform.forward);
        }
        else if (distToPlayer > endAttackDist)
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
        else if (hit.collider.CompareTag("Player") && canAttack)
        {
            canAttack = false;
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
            base.ReloadCoroutine();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!hasSpoken)
            hasSpoken = true;
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

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }
}
