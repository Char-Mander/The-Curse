using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    private float maxObjectStamina;
    private float currentObjectStamina;
    private SimpleHealthBar simpleStaminaBar;

    void Start()
    {
        simpleStaminaBar = GetComponent<SimpleHealthBar>();
        simpleStaminaBar.UpdateBar(currentObjectStamina, maxObjectStamina);
    }

    // Update is called once per frame
    public void UpdateStaminaBar()
    {
        simpleStaminaBar.UpdateBar(currentObjectStamina, maxObjectStamina);
    }

    public void SetMaxStamina(float max)
    {
        maxObjectStamina = max;
    }

    public void SetCurrentStamina(float current)
    {
        currentObjectStamina = current;
    }
}
