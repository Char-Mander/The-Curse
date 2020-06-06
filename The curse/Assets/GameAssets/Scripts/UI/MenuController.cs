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
        btnLoadGame.enabled = GameManager.instance.data.HasPlayerData();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BtnLoadMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.sceneC.LoadMenu();
    }

    public void BtnStartNewGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.data.ResetData();
        GameManager.instance.sceneC.LoadSceneLvl();
    }

    public void BtnLoadGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.data.LoadData();
        GameManager.instance.sceneC.LoadSceneLvl();
    }

    public void BtnDeleteGame()
    {
        GameManager.instance.data.ResetData();
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
