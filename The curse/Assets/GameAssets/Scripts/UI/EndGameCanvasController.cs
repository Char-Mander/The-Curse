using System.Collections;
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
        LoadWinLoseSetUp(/*(GameManager.instance.GetMaxCheckPoints() == GameManager.instance.GetCurrentCheckPoint() - 1) 
            && (GameManager.instance.GetMaxQuests() == GameManager.instance.GetCurrentQuest() - 1) && */GameManager.instance.GetCurrentPlayerHealth() > 0);
        print("Muertes: " + GameManager.instance.GetDeaths());
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
