using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float maxStamina;
    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
    }

    public void LoseStamina(float value)
    {
        currentStamina -= value;
        if (hasNoStamina())
        {
            currentStamina = 0;
        }
        GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateStaminaBar();
    }

    public void GainStamina(float value)
    {
        currentStamina += value;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateStaminaBar();
    }

    public bool hasMaxStamina() { return currentStamina == maxStamina; }

    public bool hasNoStamina() { return currentStamina <= 0; }

    public float GetMaxStamina() { return maxStamina; }

    public float GetCurrentStamina() { return currentStamina; }
}
