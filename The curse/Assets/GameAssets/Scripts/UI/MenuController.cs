using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button btnLoadGame;

    private void Start()
    {
        btnLoadGame.enabled = GameManager.instance.data.HasPreviousData();
    }

    public void BtnStartNewGame()
    {
        GameManager.instance.data.ResetData();
        BtnLoadGame();
    }

    public void BtnLoadGame()
    {
        GameManager.instance.sceneC.LoadSceneLvl();
    }

    public void BtnOptions()
    {
        GameManager.instance.optionsC.SwitchPause();
    }

    public void BtnExitGame()
    {
        Application.Quit();
    }
}
