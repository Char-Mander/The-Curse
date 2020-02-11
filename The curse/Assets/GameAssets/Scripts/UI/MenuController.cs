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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BtnStartNewGame()
    {
        GameManager.instance.data.ResetData();
        BtnLoadGame();
    }

    public void BtnLoadGame()
    {
        GameManager.instance.data.LoadData();
        GameManager.instance.sceneC.LoadSceneLvl();
    }

    public void BtnOptions()
    {
        print("Entra por btnOptions");
        GameManager.instance.optionsC.SwitchPause();
    }

    public void BtnExitGame()
    {
        Application.Quit();
    }
}
