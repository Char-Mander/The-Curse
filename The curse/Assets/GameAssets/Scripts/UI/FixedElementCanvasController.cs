using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedElementCanvasController : MonoBehaviour
{   [SerializeField]
    GameObject interactionPanel;
    [SerializeField]
    GameObject npcNamePanel;
    [SerializeField]
    GameObject fuelBarUI;

    GameObject player;
    Health playerHealth;
    HealthBar playerHealthBar;
    Stamina playerStamina;
    StaminaBar playerStaminaBar;
    Fuel playerWeaponFuel;
    FuelBar playerFuelBar;
    // Start is called before the first frame update
    void Awake()
    {
        //Obtains the player
        player = GameObject.FindGameObjectWithTag("Player");
        //Obtains the health atributes
        playerHealth = player.GetComponent<Health>();
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.SetMaxHealth(playerHealth.GetMaxHealth());
        playerHealthBar.SetCurrentHealth(playerHealth.GetMaxHealth());
        //Obtains the stamina atributes
        playerStamina = player.GetComponent<Stamina>();
        playerStaminaBar = GetComponentInChildren<StaminaBar>();
        playerStaminaBar.SetMaxStamina(playerStamina.GetMaxStamina());
        playerStaminaBar.SetCurrentStamina(playerStamina.GetMaxStamina());
        npcNamePanel.SetActive(false);
        interactionPanel.SetActive(false);
        fuelBarUI.SetActive(false);
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

    public void UpdateTextPanel(string text, bool isNpc, string name)
    {
        interactionPanel.SetActive(true);
        interactionPanel.GetComponentInChildren<Text>().text = text;
        if (isNpc)
        {
            npcNamePanel.SetActive(true);
            npcNamePanel.GetComponentInChildren<Text>().text = name;
        }
        StartCoroutine(WaitForTextToBeShown((float)text.Length / 20, isNpc));
    }

    public void EnableOrDisableFuelBar(bool enable)
    {
        fuelBarUI.SetActive(enable);
        if (enable)
        {
            //Obtains the fuel atributes
            playerWeaponFuel = player.GetComponentInChildren<Fuel>();
            playerFuelBar = GetComponentInChildren<FuelBar>();
            playerFuelBar.SetMaxFuel(playerWeaponFuel.GetMaxFuel());
            playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetMaxFuel());
        }
    }

    public void UpdateFuelBar()
    {
        playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetCurrentFuel());
        playerFuelBar.UpdateFuelBar();
    }

    IEnumerator WaitForTextToBeShown(float time, bool isNpc)
    {
        yield return new WaitForSeconds(time);
        interactionPanel.GetComponentInChildren<Text>().text = "";
        if (isNpc)
        {
            npcNamePanel.SetActive(false);
            npcNamePanel.GetComponentInChildren<Text>().text = "";
        }
        interactionPanel.SetActive(false);
    }
}
