﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPause();
        }
    }

    public void BackToMenuBtn()
    {
        if (GameManager.instance.sceneC.IsALvlScene()) GameManager.instance.data.LoadData();
        GameManager.instance.sceneC.LoadMenu();
    }

    public void SwitchPause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
            if (GameManager.instance.sceneC.IsALvlScene() && !FindObjectOfType<CursedGirlTalk>().IsCinematicPlaying())
            {
                FindObjectOfType<PlayerController>().SetIsLocked(true);
                FindObjectOfType<Mount>().SetIsLocked(true);
            }
        }
        else if (!isPaused)
        {
            if (GameManager.instance.sceneC.IsALvlScene() && !FindObjectOfType<CursedGirlTalk>().IsCinematicPlaying())
            {
                if (FindObjectOfType<CursedGirlEnemy>() != null) Cursor.lockState = CursorLockMode.Locked;
                if (!FindObjectOfType<DialogueManager>().IsOnADialogue() && !FindObjectOfType<TeleportController>().GetTeleportOptionsAvailable())
                {
                    FindObjectOfType<PlayerController>().SetIsLocked(false);
                    FindObjectOfType<Mount>().SetIsLocked(false);
                }
            }
            else if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        optionPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.000001f : 1;
    }
}
