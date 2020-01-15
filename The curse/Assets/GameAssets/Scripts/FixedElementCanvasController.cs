using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedElementCanvasController : MonoBehaviour
{
   // GameObject player;
    Health playerHealth;
    HealthBar playerHealthBar;
    Stamina playerStamina;
    StaminaBar playerStaminaBar;
   /* Fuel playerWeaponFuel;
    FuelBar playerFuelBar;*/
    // Start is called before the first frame update
    void Start()
    {
        //Obtains the player
        //player = GameObject.FindGameObjectWithTag("Player");
        //Obtains the health atributes
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.SetMaxHealth(playerHealth.GetMaxHealth());
        playerHealthBar.SetCurrentHealth(playerHealth.GetMaxHealth());
        //Obtains the stamina atributes
        playerStamina = GameObject.FindGameObjectWithTag("Player").GetComponent<Stamina>();
        playerStaminaBar = GetComponentInChildren<StaminaBar>();
        playerStaminaBar.SetMaxStamina(playerStamina.GetMaxStamina());
        playerStaminaBar.SetCurrentStamina(playerStamina.GetMaxStamina());
        //Obtains the fuel atributes
        //playerWeaponFuel = player.GetComponent<Fuel>();
        //playerFuelBar = GetComponentInChildren<FuelBar>();
        //playerFuelBar.SetMaxFuel(playerWeaponFuel.GetMaxFuel());
        //playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetMaxFuel());
        print("Health: " + playerHealth.GetCurrentHealth());
        print("Stamina: " + playerStamina.GetCurrentStamina());
        //print("Fuelbar: " + playerFuelBar);
    }

    public void UpdateHealthBar()
    {
        playerHealthBar.SetCurrentHealth(playerHealth.GetCurrentHealth());
        playerHealthBar.UpdateHealthBar();
    }

    public void UpdateStaminaBar()
    {
        playerStaminaBar.SetCurrentStamina(playerStamina.GetCurrentStamina());
        playerStaminaBar.UpdateStaminaBar();
    }
    /*
    public void UpdateFuelBar()
    {
        playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetCurrentFuel());
        playerFuelBar.UpdateFuelBar();
    }*/
}
