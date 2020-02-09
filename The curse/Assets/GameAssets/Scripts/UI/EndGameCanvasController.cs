﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject winTitle;
    [SerializeField]
    private GameObject loseTitle;
    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    TextMeshProUGUI defeatedEnemiesText;
    [SerializeField]
    TextMeshProUGUI deathsText;
    [SerializeField]
    private GameObject decisionPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;


    private void Start()
    {
        LoadWinLoseSetUp(GameManager.instance.GetCurrentPlayerHealth() > 0);
        if (GameManager.instance.GetCurrentPlayerHealth() <= 0)
        {
            GameManager.instance.SetCurrentPlayerHealth(GameManager.instance.GetPlayerMaxHealth());
            GameManager.instance.data.SaveHealth(GameManager.instance.GetCurrentPlayerHealth());
        }
    }

    public void LoadWinLoseSetUp(bool win)
    {
        winTitle.SetActive(win);
        loseTitle.SetActive(!win);
        scorePanel.SetActive(true);
        decisionPanel.SetActive(win);
        winPanel.SetActive(win);
        losePanel.SetActive(!win);
        UpdateScorePanels();
    }

    public void UpdateScorePanels()
    {
        if (decisionPanel.activeInHierarchy)
        {
            string decisionText = GameManager.instance.GetDecision() > 0 ? " salvar " : " matar ";
            decisionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Decidiste" + decisionText + "a la chica maldita";
        }
        
        defeatedEnemiesText.text = "Enemigos derrotados:  " + GameManager.instance.GetDefeatedEnemies().ToString();
        deathsText.text = "Muertes:  " + GameManager.instance.GetDeaths().ToString();
    }
}