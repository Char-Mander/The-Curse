using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
     private float maxObjectFuel;
     private float currentObjectFuel;
     [SerializeField]
     private Color red, orange, yellow, blue;
     private float currentFillFuel;
     [SerializeField]
     Image fillImage;

    void Start()
     {
        UpdateFuelBar();
     }

     // Update is called once per frame
     public void UpdateFuelBar()
     {
        currentFillFuel = currentObjectFuel / maxObjectFuel;
        fillImage.fillAmount = currentFillFuel;
        BarColorByAmount();
        if (GetComponentInChildren<Text>() != null) GetComponentInChildren<Text>().text = ((int)currentObjectFuel).ToString() + "/" + maxObjectFuel.ToString();
     }

     void BarColorByAmount()
     {
         float percentFuelValue = currentObjectFuel * 100 / maxObjectFuel;
         if (percentFuelValue < 10)
         {
            fillImage.color = blue;
         }
         else if(percentFuelValue < 33)
         {
            fillImage.color = yellow;
         }
         else if (percentFuelValue < 66)
         {
            fillImage.color = orange;
         }
         else
         {
            fillImage.color = red;
         }
     }

     public void SetMaxFuel(float max)
     {
         maxObjectFuel = max;
     }

     public void SetCurrentFuel(float current)
     {
         currentObjectFuel = current;
     }
}
