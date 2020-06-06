using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    private bool isPaused = false;
    
    void Start()
    {
        optionPanel.SetActive(false);
    }
    
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
        if (GameManager.instance.sceneC.IsALvlScene())
        {
            bool isPlaying = false;
            int index = 0;
            PlayGenericCinematic[] cinematics = FindObjectsOfType<PlayGenericCinematic>();
            while(index < cinematics.Length && !isPlaying)
            {
                if (!cinematics[index].IsNotPlaying()) isPlaying = true;
                index++;
            }
            if (isPlaying) return;
        }
            isPaused = !isPaused;
            if (isPaused)
            {
                if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (GameManager.instance.sceneC.IsALvlScene() && !FindObjectOfType<DialogueManager>().IsOnADialogue())
                {
                    FindObjectOfType<PlayerController>().soundsManager.StopSound();
                    FindObjectOfType<PlayerController>().SetIsLocked(true);
                    FindObjectOfType<Mount>().SetIsLocked(true);
                }
            }
            else if (!isPaused)
            {
                if (GameManager.instance.sceneC.IsALvlScene())
            {
                    Cursor.visible = false;
                    if (!FindObjectOfType<DialogueManager>().IsOnADialogue())
                    {
                        if (FindObjectOfType<CursedGirlEnemy>() != null) Cursor.lockState = CursorLockMode.Locked;
                        if (!FindObjectOfType<DialogueManager>().IsOnADialogue() && !FindObjectOfType<TeleportController>().GetTeleportOptionsAvailable())
                        {
                            FindObjectOfType<PlayerController>().SetIsLocked(false);
                            FindObjectOfType<Mount>().SetIsLocked(false);
                        }
                    }
                }
                else if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            optionPanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0.000001f : 1;
    }

}
