﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField]
    private float iniAttackDist;

    public override void DetectPlayerInArea()
    {
        if(distToPlayer < iniAttackDist)
       {
           base.EnemyMovement(attackSpeed, -transform.forward);
       }
       else  if (distToPlayer > endAttackDist)
        {
            base.EnemyMovement(attackSpeed, transform.forward);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }
}
