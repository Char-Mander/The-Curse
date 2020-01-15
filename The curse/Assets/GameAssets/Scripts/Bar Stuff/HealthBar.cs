using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float maxObjectHealth;
    private float currentObjectHealth;
    private SimpleHealthBar simpleHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        simpleHealthBar = GetComponent<SimpleHealthBar>();
        simpleHealthBar.UpdateBar(currentObjectHealth, maxObjectHealth);
        UpdateHealthBar();
    }
    
    // Update is called once per frame
    public void UpdateHealthBar()
    {
        simpleHealthBar.UpdateBar(currentObjectHealth, maxObjectHealth);
        BarColorByHealth();
    }

    void BarColorByHealth()
    {
        float percentHealthValue = currentObjectHealth * 100/ maxObjectHealth;
        if(percentHealthValue < 33)
        {
            simpleHealthBar.UpdateColor(Color.red);
        }
        else if(percentHealthValue < 66)
        {
            simpleHealthBar.UpdateColor(Color.yellow);
        }
        else
        {
            simpleHealthBar.UpdateColor(Color.green);
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
