using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float maxObjectHealth;
    private float currentObjectHealth;
    private float currentFillHealth;
    [SerializeField]
    Image fillImage;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBar();
    }
    
    // Update is called once per frame
    public void UpdateHealthBar()
    {
        currentFillHealth = currentObjectHealth/ maxObjectHealth;
        fillImage.fillAmount = currentFillHealth;
        BarColorByHealth();
        if (GetComponentInChildren<Text>() != null) GetComponentInChildren<Text>().text = ((int)currentObjectHealth).ToString() + "/" + maxObjectHealth.ToString();
    }

    void BarColorByHealth()
    {
        float percentHealthValue = currentObjectHealth * 100/ maxObjectHealth;
        if(percentHealthValue < 33)
        {
            fillImage.color = Color.red;
        }
        else if(percentHealthValue < 66)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    public void SetMaxHealth(float max)
    {
        maxObjectHealth = max;
    }

    public void SetCurrentHealth(float current)
    {
        currentObjectHealth = current;
    }
}
