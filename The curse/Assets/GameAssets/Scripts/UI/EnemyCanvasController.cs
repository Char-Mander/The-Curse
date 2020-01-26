using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasController : WorldSpaceCanvasController
{
    GameObject enemy;
    Health enemyHealth;
    HealthBar enemyHealthBar;
    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = GetComponentInParent<Health>();
        enemyHealthBar = GetComponentInChildren<HealthBar>();
        enemyHealthBar.SetMaxHealth(enemyHealth.GetMaxHealth());
        enemyHealthBar.SetCurrentHealth(enemyHealth.GetMaxHealth());
    }

    public void UpdateHealthBar()
    {
        enemyHealthBar.SetCurrentHealth(enemyHealth.GetCurrentHealth());
        enemyHealthBar.UpdateHealthBar();
    }
}
