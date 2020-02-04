using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject winTitle;
    [SerializeField]
    private GameObject loseTitle;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;

    private void Start()
    {
        LoadWinLoseSetUp((GameManager.instance.GetMaxCheckPoints() == GameManager.instance.GetCurrentCheckPoint() - 1) 
            && (GameManager.instance.GetMaxQuests() == GameManager.instance.GetCurrentQuest() - 1));
    }

    public void LoadWinLoseSetUp(bool win)
    {
        winTitle.SetActive(win);
        loseTitle.SetActive(!win);
        winPanel.SetActive(win);
        losePanel.SetActive(!win);
    }
}
