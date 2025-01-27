﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedElementCanvasController : MonoBehaviour
{
    //Panels and text 
    [SerializeField]
    GameObject textPanel;
    [SerializeField]
    GameObject introTextPanel;
    [SerializeField]
    GameObject savePanel;
    [SerializeField]
    GameObject unlockWeaponPanel;
    [SerializeField]
    GameObject deathPanel;
    [SerializeField]
    GameObject outOfAmmoPanel;
    [SerializeField]
    GameObject bulletPanel;
    [SerializeField]
    Text bulletText;
    [SerializeField]
    Text questText;
    [SerializeField]
    GameObject sentenceOptionsPanel;
    [SerializeField]
    Transform optionsContent;
    [SerializeField]
    GameObject optionButton;
    [SerializeField]
    GameObject teleportButton;
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

    void Awake()
    {
        //Obtains the player
        player = GameObject.FindGameObjectWithTag("Player");
        //Obtains the health atributes
        playerHealth = player.GetComponent<Health>();
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.SetMaxHealth(playerHealth.GetMaxHealth());
        playerHealthBar.SetCurrentHealth(GameManager.instance.data.HasPlayerData() ? GameManager.instance.GetCurrentPlayerHealth() : playerHealth.GetMaxHealth());
        //Obtains the stamina atributes
        playerStamina = player.GetComponent<Stamina>();
        playerStaminaBar = GetComponentInChildren<StaminaBar>();
        playerStaminaBar.SetMaxStamina(playerStamina.GetMaxStamina());
        playerStaminaBar.SetCurrentStamina(playerStamina.GetMaxStamina());
        //Disable the panels
        npcNamePanel.SetActive(false);
        textPanel.SetActive(false);
        introTextPanel.SetActive(false);
        savePanel.SetActive(false);
        unlockWeaponPanel.SetActive(false);
        deathPanel.SetActive(false);
        outOfAmmoPanel.SetActive(false);
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

    public void EnableBulletPanel(bool enable)
    {
        bulletPanel.SetActive(enable);
    }

    public void UpdateBulletPanel(int ammo)
    {
        bulletText.text = "x " + ammo.ToString();
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
        StartCoroutine(HideTemporaryPanel(savePanel));
    }

    public void ShowUnlockWeaponPanel(int weapon)
    {
        unlockWeaponPanel.SetActive(true);
        unlockWeaponPanel.GetComponentInChildren<Text>().text = "¡Has desbloqueado el arma " + weapon + "!";
        StartCoroutine(HideUnlockWeaponPanel());
    }

    public void EnableOrDisableDeathPanel(bool enable)
    {
        deathPanel.SetActive(enable);
    }

    public void ShowOutOfAmmoPanel()
    {
        outOfAmmoPanel.SetActive(true);
        StartCoroutine(HideTemporaryPanel(outOfAmmoPanel));
    }

    public void UpdateQuestPanel(string text)
    {
        StartCoroutine(QuestChangeWait(text));
    }

    public void TeleportPointsDeployment(TeleportPoint[] list)
    {
        Cursor.visible = true;
        EnableOrDisableOptionsPanel(true);
        FindObjectOfType<TeleportController>().SetTeleportOptionsAvailable(true);
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].HasBeenDiscovered())
            {
                GameObject btn = Instantiate(teleportButton, optionsContent);
                btn.name = i.ToString();
                btn.GetComponentInChildren<Text>().text = list[i].GetName();
            }
        }
        GameObject backBtn = Instantiate(teleportButton, optionsContent);
        backBtn.name = list.Length.ToString();
        backBtn.GetComponentInChildren<Text>().text = "Atrás";
        Cursor.lockState = CursorLockMode.None;
        FindObjectOfType<PlayerController>().SetIsLocked(true);
        FindObjectOfType<PlayerController>().GetComponent<CharacterController>().enabled = false;
    }

    public void TeleportOption(int index)
    {
        Cursor.visible = false;
        EnableOrDisableOptionsPanel(false);
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<TeleportController>().HideTP();
        if (FindObjectOfType<TeleportController>().GetTeleportPointListLength() > index) FindObjectOfType<TeleportController>().Teleport(index);
        FindObjectOfType<TeleportController>().SetTeleportOptionsAvailable(false);
        StartCoroutine(WaitForUnlockPlayer());
    }

    public void UpdateSentenceOptionsPanel(Sentence s, int index)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        sAux = s;
        sentenceIndex = index;
        EnableOrDisableOptionsPanel(true);
        for (int i = 0; i < s.options.Count; i++)
        {
            GameObject btn = Instantiate(optionButton, optionsContent);
            btn.name = i.ToString();
            btn.GetComponentInChildren<Text>().text = s.options[i].optionTxt;
        }
        FindObjectOfType<PlayerController>().SetIsLocked(true);
    }


    public void ChooseAnOption(int index)
    {
        Cursor.visible = false;
        FindObjectOfType<PlayerController>().SetIsLocked(false);
        FindObjectOfType<DecisionState>().AddOrSubtractToBalance(sAux.options[index].decisionBalance);
        EnableOrDisableOptionsPanel(false);
        FindObjectOfType<DialogueManager>().DisplayNextSentence(index + 1);
    }

    public void EnableOrDisableOptionsPanel(bool value)
    {
        if (!value)
        {
            int childs = optionsContent.childCount;
            for (int i = 0; i < childs; i++)
            {
                Destroy(optionsContent.GetChild(i).gameObject);
            }
        }
        sentenceOptionsPanel.SetActive(value);
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

    IEnumerator HideUnlockWeaponPanel()
    {
        yield return new WaitForSeconds(2f);
        unlockWeaponPanel.GetComponentInChildren<Text>().text = "";
        unlockWeaponPanel.SetActive(false);
    }

    IEnumerator QuestChangeWait(string text)
    {
        if(questText.text.Length > 0) questText.text = "¡MISIÓN COMPLETADA!";
        yield return new WaitForSeconds(1.25f);
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

    IEnumerator HideTemporaryPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(false);
    }

    public float GetTextTime()
    {
        return textTime;
    }

    IEnumerator WaitForUnlockPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<PlayerController>().SetIsLocked(false);
        FindObjectOfType<PlayerController>().GetComponent<CharacterController>().enabled = true;
    }
}
