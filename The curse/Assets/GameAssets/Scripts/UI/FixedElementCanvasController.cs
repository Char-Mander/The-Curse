using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedElementCanvasController : MonoBehaviour
{   [SerializeField]
    GameObject textPanel;
    [SerializeField]
    GameObject savePanel;
    [SerializeField]
    Text questText;
    [SerializeField]
    GameObject sentenceOptionsPanel;
    [SerializeField]
    Transform optionsContent;
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
    Sentence sAux;
    // Start is called before the first frame update
    void Awake()
    {
        //Obtains the player
        player = GameObject.FindGameObjectWithTag("Player");
        //Obtains the health atributes
        playerHealth = player.GetComponent<Health>();
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.SetMaxHealth(playerHealth.GetMaxHealth());
        playerHealthBar.SetCurrentHealth(GameManager.instance.data.HasPreviousData() ? GameManager.instance.GetCurrentPlayerHealth() : playerHealth.GetMaxHealth());
        //Obtains the stamina atributes
        playerStamina = player.GetComponent<Stamina>();
        playerStaminaBar = GetComponentInChildren<StaminaBar>();
        playerStaminaBar.SetMaxStamina(playerStamina.GetMaxStamina());
        playerStaminaBar.SetCurrentStamina(playerStamina.GetMaxStamina());
        npcNamePanel.SetActive(false);
        textPanel.SetActive(false);
        savePanel.SetActive(false);
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

    public void ShowSavePanel()
    {
        savePanel.SetActive(true);
        StartCoroutine(HideSavePanel());
    }

    public void UpdateQuestPanel(string text)
    {
        StartCoroutine(QuestChangeWait(text));
    }

    public void UpdateSentenceOptionsPanel(Sentence s, int index)
    {
        sAux = s;
        sentenceIndex = index;
        sentenceOptionsPanel.SetActive(true);
        for(int i = 0; i < s.options.Count; i++)
        {
            GameObject btn = Instantiate(optionButton, optionsContent);
            btn.name = i.ToString();
            btn.GetComponentInChildren<Text>().text = s.options[i].optionTxt;
        }
        FindObjectOfType<PlayerController>().SetIsLocked(true);
    }


    public void ChooseAnOption(int index)
    {
        FindObjectOfType<PlayerController>().SetIsLocked(false);
        FindObjectOfType<DecisionState>().AddOrSubtractToBalance(sAux.options[index].decisionBalance);
        sentenceOptionsPanel.SetActive(false);
        FindObjectOfType<DialogueManager>().DisplayNextSentence(index + 1);
        FindObjectOfType<CursedGirlEnemy>().ApplyDecisionState();
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
        if (playerFuelBar.enabled)
        {
            playerFuelBar.SetCurrentFuel(playerWeaponFuel.GetCurrentFuel());
            playerFuelBar.UpdateFuelBar();
        }
    }

    public void InitTextPanel(string text, bool isNpc, string name)
    {
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

    IEnumerator QuestChangeWait(string text)
    {
        if(questText.text.Length > 0) questText.text = "¡MISIÓN COMPLETADA!";
        yield return new WaitForSeconds(2f);
        questText.text = text;
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

    IEnumerator HideSavePanel()
    {
        yield return new WaitForSeconds(0.5f);
        savePanel.SetActive(false);
    }

    public float GetTextTime()
    {
        return textTime;
    }
}
