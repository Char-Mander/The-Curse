using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    /* [SerializeField]
     private GameObject enemyDeadParticle;*/
    private float currentHealth;
    private bool isSufferingConstantDamage = false;
    private bool isHealingConstantly = false;
    private float constantDamage;
    private float constantHeal;
    private bool godMode;

    private void Awake()
    {
        if (this.gameObject.CompareTag("Player") && !GameManager.instance.data.HasPreviousData())
            GameManager.instance.SetPlayerMaxHealth(maxHealth);

        currentHealth = (this.gameObject.CompareTag("Player") && !GameManager.instance.data.HasPreviousData()) || !this.CompareTag("Player") ?
              maxHealth : GameManager.instance.GetCurrentPlayerHealth();
    }

    private void Start()
    {
        
       /* if (this.gameObject.CompareTag("Player"))
        {
            print("Objeto: " + this.gameObject.name);
            print("Max health before: " + maxHealth);
            print("Current health before: " + currentHealth);
            FindObjectOfType<FixedElementCanvasController>().UpdateHealthBar();
            print("Max health after update: " + maxHealth);
            print("Current health after update: " + currentHealth);
        }*/
    }

    public void Update()
    {
        if (isSufferingConstantDamage && !godMode)
        {
            LoseHealth(constantDamage);
        }

        if (isHealingConstantly)
        {
            GainHealth(constantHeal);
        }
    }

    public void LoseHealth(float value)
    {
        if (!godMode)
        {
            currentHealth -= value;
            if (currentHealth <= 0)
            {
                if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "Explosive Sphere")
                {
                    // Destroy(Instantiate(enemyDeadParticle, transform.position, Quaternion.identity), 3);
                    GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
                    Destroy(this.gameObject);
                }
                else if (this.gameObject.tag == "Player")
                {
                    GameManager.instance.SetDeaths(GameManager.instance.GetDeaths() + 1);
                    GameManager.instance.sceneC.LoadGameOver();
                }
            }
            if (GetComponentInChildren<EnemyCanvasController>() != null) GetComponentInChildren<EnemyCanvasController>().UpdateHealthBar();
            else FindObjectOfType<FixedElementCanvasController>().UpdateHealthBar();
        }
        
    }

    public void GainHealth(float value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (GetComponentInChildren<EnemyCanvasController>() != null) GetComponentInChildren<EnemyCanvasController>().UpdateHealthBar();
        else FindObjectOfType<FixedElementCanvasController>().UpdateHealthBar();
    }

    public float GetMaxHealth() { return maxHealth; }

    public float GetCurrentHealth() { return currentHealth; }

    public void ReceiveConstantDamage(float damage)
    {
        if (!godMode)
        {
            isSufferingConstantDamage = true;
            this.constantDamage = damage;
        }
    }

    public void StopReceivingConstantDamage()
    {
        isSufferingConstantDamage = false;
    }

    public void ReceiveConstantHeal(float healAmount)
    {
        isHealingConstantly = true;
        this.constantHeal = healAmount;
    }

    public void StopReceivingConstantHeal()
    {
        isHealingConstantly = false;
    }

    public bool hasMaxHealth() { return maxHealth <= currentHealth; }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public bool GetGodMode() { return godMode; }

    public void SetGodMode(bool active)
    {
        godMode = active;
    }
}
