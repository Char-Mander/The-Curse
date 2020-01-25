using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Update()
    {
        if (isSufferingConstantDamage)
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
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "Explosive Sphere")
            {
               // Destroy(Instantiate(enemyDeadParticle, transform.position, Quaternion.identity), 3);
               if (this.gameObject.name == "Final Boss")
                {
                    //Instantiate(this.gameObject.GetComponent<FinalBoss>().GetGoal(), this.gameObject.transform.position, this.gameObject.transform.rotation);
                }
                Destroy(this.gameObject);
            }
            else if (this.gameObject.tag == "Player")
            {
                print("Has perdido");
              //  SceneManager.LoadScene("GameOver");
            }
        }
       if(GetComponentInChildren<EnemyCanvasController>() != null) GetComponentInChildren<EnemyCanvasController>().UpdateHealthBar();
       else GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateHealthBar();
    }

    public void GainHealth(float value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (GetComponentInChildren<EnemyCanvasController>() != null) GetComponentInChildren<EnemyCanvasController>().UpdateHealthBar();
        else GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateHealthBar();
    }

    public float GetMaxHealth() { return maxHealth; }

    public float GetCurrentHealth() { return currentHealth; }

    public void ReceiveConstantDamage(float damage)
    {
        isSufferingConstantDamage = true;
        this.constantDamage = damage;
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
}
