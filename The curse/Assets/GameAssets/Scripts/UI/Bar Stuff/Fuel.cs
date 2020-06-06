using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField]
    private float maxFuel;
    private float currentFuel;

    void Start()
    {
        currentFuel = maxFuel;
    }


    void Update()
    {
    }

    public void LoseFuel(float value)
    {
        currentFuel -= value;
        if (hasNoFuel())
        {
            currentFuel = 0;
        }
        GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateFuelBar();
    }

    public void GainFuel(float value)
    {
        currentFuel += value;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
        GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateFuelBar();
    }

    public bool hasMaxFuel() { return currentFuel == maxFuel; }

    public bool hasNoFuel() { return currentFuel <= 0; }

    public float GetMaxFuel() { return maxFuel; }

    public float GetCurrentFuel() { return currentFuel; }
}
