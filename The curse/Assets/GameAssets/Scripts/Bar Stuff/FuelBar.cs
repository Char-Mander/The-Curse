using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBar : MonoBehaviour
{
    /* private float maxObjectFuel;
     private float currentObjectFuel;
     [SerializeField]
     private Color red, orange, yellow, blue;
     private SimpleHealthBar simpleFuelBar;

     void Start()
     {
         simpleFuelBar = GetComponent<SimpleHealthBar>();
         simpleFuelBar.UpdateBar(currentObjectFuel, maxObjectFuel);
     }

     // Update is called once per frame
     public void UpdateFuelBar()
     {
         simpleFuelBar.UpdateBar(currentObjectFuel, maxObjectFuel);
         BarColorByAmount();
     }

     void BarColorByAmount()
     {
         float percentFuelValue = currentObjectFuel * 100 / maxObjectFuel;
         if (percentFuelValue < 10)
         {
             simpleFuelBar.UpdateColor(blue);
         }
         else if(percentFuelValue < 33)
         {
             simpleFuelBar.UpdateColor(yellow);
         }
         else if (percentFuelValue < 66)
         {
             simpleFuelBar.UpdateColor(orange);
         }
         else
         {
             simpleFuelBar.UpdateColor(red);
         }
     }

     public void SetMaxFuel(float max)
     {
         maxObjectFuel = max;
     }

     public void SetCurrentFuel(float current)
     {
         currentObjectFuel = current;
     }*/
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
