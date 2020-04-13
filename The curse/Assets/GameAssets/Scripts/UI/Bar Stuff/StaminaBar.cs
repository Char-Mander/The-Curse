using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private float maxObjectStamina;
    private float currentObjectStamina;
    private float currentFillStamina;
    [SerializeField]
    Image fillImage;

    void Start()
    {
        UpdateStaminaBar();
    }

    // Update is called once per frame
    public void UpdateStaminaBar()
    {
        currentFillStamina = currentObjectStamina / maxObjectStamina;
        fillImage.fillAmount = currentFillStamina;
        if (GetComponentInChildren<Text>() != null) GetComponentInChildren<Text>().text = ((int)currentObjectStamina).ToString() + "/" + maxObjectStamina.ToString();
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
