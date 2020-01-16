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
    Coroutine panelTextCoroutine;
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
        if (panelTextCoroutine != null)
        {
            StopCoroutine(panelTextCoroutine);
            ResetPanelText();
        }
        InitTextPanel(text, isNpc, name);
        float textTime = text.Length > 30 ? (float)text.Length / 20 : 1f;
        panelTextCoroutine = StartCoroutine(WaitForTextToBeShown(textTime, isNpc));
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

    public void InitTextPanel(string text, bool isNpc, string name)
    {
        if (isNpc)
        {
            npcNamePanel.SetActive(true);
            npcNamePanel.GetComponentInChildren<Text>().text = name;
        }
        interactionPanel.SetActive(true);
        //interactionPanel.GetComponentInChildren<Text>().text = text;
        StartCoroutine(TypeText(text));
        
    }

    public void ResetPanelText()
    {
        npcNamePanel.SetActive(false);
        npcNamePanel.GetComponentInChildren<Text>().text = "";
        interactionPanel.SetActive(false);
        interactionPanel.GetComponentInChildren<Text>().text = "";
    }

    IEnumerator WaitForTextToBeShown(float time, bool isNpc)
    {
        yield return new WaitForSeconds(time);
        ResetPanelText();
    }

    IEnumerator TypeText(string text)
    {
        interactionPanel.GetComponentInChildren<Text>().text = "";
        foreach (char letter in text.ToCharArray())
        {
            interactionPanel.GetComponentInChildren<Text>().text += letter;
            yield return null;
        }
    }
}
