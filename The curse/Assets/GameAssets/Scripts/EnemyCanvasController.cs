using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasController : MonoBehaviour
{
    GameObject enemy;
    Health enemyHealth;
    HealthBar enemyHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponentInParent<Health>();
        enemyHealthBar = GetComponentInChildren<HealthBar>();
        enemyHealthBar.SetMaxHealth(enemyHealth.GetMaxHealth());
        enemyHealthBar.SetCurrentHealth(enemyHealth.GetMaxHealth());
    }

    private void Update()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void UpdateHealthBar()
    {
        enemyHealthBar.SetCurrentHealth(enemyHealth.GetCurrentHealth());
        enemyHealthBar.UpdateHealthBar();
    }
}
