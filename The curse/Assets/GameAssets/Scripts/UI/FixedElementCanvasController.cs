using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedElementCanvasController : MonoBehaviour
{   [SerializeField]
    GameObject textPanel;
    [SerializeField]
    GameObject sentenceOptionsPanel;
    [SerializeField]
    GameObject optionButton;
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
    Coroutine panelTextCoroutine, textTyping;
    float textTime;
    int sentenceIndex;
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
        textPanel.SetActive(false);
        sentenceOptionsPanel.SetActive(false);
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
            if (textTyping != null) StopCoroutine(textTyping);
            ResetPanelText();
        }
        InitTextPanel(text, isNpc, name);
        textTime = text.Length > 30 ? (float)text.Length / 20 : 1f;
        panelTextCoroutine = StartCoroutine(WaitForTextToBeShown(textTime, isNpc));
    }

    public void UpdateSentenceOptionsPanel(Sentence s, int index)
    {
        sentenceIndex = index;
        //Update de visual panel
    }

    public void ChooseAnOption(int index)
    {
        //guarda la opción
        StartCoroutine(WaitForCleanSentencesOptions(1, index));
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
        print("Pone en active el textpanel");
        textPanel.SetActive(true);
        if (isNpc)
        {
            npcNamePanel.SetActive(true);
            npcNamePanel.GetComponentInChildren<Text>().text = name;
            textTyping = StartCoroutine(TypeText(text));

        }
        else textPanel.GetComponentInChildren<Text>().text = text;

    }

    public void ResetPanelText()
    {
        textTime = 0;
        npcNamePanel.SetActive(false);
        npcNamePanel.GetComponentInChildren<Text>().text = "";
        textPanel.SetActive(false);
        textPanel.GetComponentInChildren<Text>().text = "";
    }

    IEnumerator WaitForCleanSentencesOptions(float time, int index)
    {
        yield return new WaitForSeconds(time);
        FindObjectOfType<DialogueManager>().DisplayNextSentence(index + 1);
    }

    IEnumerator WaitForTextToBeShown(float time, bool isNpc)
    {
        yield return new WaitForSeconds(time);
        ResetPanelText();
    }

    IEnumerator TypeText(string text)
    {
        textPanel.GetComponentInChildren<Text>().text = "";
        foreach (char letter in text.ToCharArray())
        {
            textPanel.GetComponentInChildren<Text>().text += letter;
            yield return null;
        }
    }

    public float GetTextTime()
    {
        return textTime;
    }
}
