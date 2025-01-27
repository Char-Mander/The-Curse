﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    private bool isSufferingConstantDamage = false;
    private bool isHealingConstantly = false;
    private float constantDamage;
    private float constantHeal;
    private bool godMode;
    private bool damageSoundPlayed = false;
    Coroutine dieCoroutine;

    private void Awake()
    {
        if (this.gameObject.CompareTag("Player") && !GameManager.instance.data.HasPlayerData())
            GameManager.instance.SetPlayerMaxHealth(maxHealth);

        currentHealth = (this.gameObject.CompareTag("Player") && !GameManager.instance.data.HasPlayerData()) || !this.CompareTag("Player") ?
              maxHealth : GameManager.instance.GetCurrentPlayerHealth();
    }

    private void Start()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            if (currentHealth <= 0)
            {
                GameManager.instance.SetCurrentPlayerHealth(GameManager.instance.GetPlayerMaxHealth());
                GameManager.instance.data.SaveHealth(GameManager.instance.GetCurrentPlayerHealth());
                currentHealth = maxHealth;
            }
        }
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
                currentHealth = 0;
                if (this.gameObject.CompareTag("Enemy") && dieCoroutine == null)
                {
                    this.gameObject.GetComponent<Enemy>().locked = true;
                    GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
                    dieCoroutine = StartCoroutine(WaitForDie(3.25f));
                }
                else if (this.gameObject.CompareTag("Player"))
                {
                    FindObjectOfType<PlayerController>().Die();
                }
            }
            if (this.gameObject.CompareTag("Player") && !damageSoundPlayed)
            {
                damageSoundPlayed = true;
                StartCoroutine(PlayDamageSound());
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

    IEnumerator WaitForDie(float time)
    {
        GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(time);
        if(GetComponent<CursedGirlEnemy>()!=null) GameManager.instance.sceneC.LoadGameOver();
        Destroy(this.gameObject);
    }

    IEnumerator PlayDamageSound()
    {
        FindObjectOfType<PlayerController>().soundsManager.ManageDamageSound();
        yield return new WaitForSeconds(1.5f);
        damageSoundPlayed = false;
    }

    public bool GetGodMode() { return godMode; }

    public void SetGodMode(bool active)
    {
        godMode = active;
    }

    public bool IsReceivingConstantDamage() { return isSufferingConstantDamage; }
}
