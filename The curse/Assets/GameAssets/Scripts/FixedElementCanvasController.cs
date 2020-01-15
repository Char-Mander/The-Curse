using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedElementCanvasController : MonoBehaviour
{   [SerializeField]
    GameObject interactionPanel;
    // GameObject player;
    Health playerHealth;
    HealthBar playerHealthBar;
    Stamina playerStamina;
    StaminaBar playerStaminaBar;
   /* Fuel playerWeaponFuel;
    FuelBar playerFuelBar;*/
    // Start is called before the first frame update
    void Awake()
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
        interactionPanel.SetActive(false);
        //Obtains the fuel atributes
        //playerWeaponFuel = player.GetComponent<Fuel>();
        //playerFuelBar = GetComponentInChildren<FuelBar>();
        //playerFuelBar.SetMaxFuel(playerWeaponFuel.GetMaxFuel());
        //playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetMaxFuel());
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

    public void UpdateTextPanel(string text)
    {
        interactionPanel.SetActive(true);
        interactionPanel.GetComponentInChildren<Text>().text = text;
        StartCoroutine(WaitForTextToBeShown((float)text.Length/10));
    }

    /*
    public void UpdateFuelBar()
    {
        playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetCurrentFuel());
        playerFuelBar.UpdateFuelBar();
    }*/

    IEnumerator WaitForTextToBeShown(float time)
    {
        yield return new WaitForSeconds(time);
        interactionPanel.GetComponentInChildren<Text>().text = "";
        interactionPanel.SetActive(false);
    }
}
