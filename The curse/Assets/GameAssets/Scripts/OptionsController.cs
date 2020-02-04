using System.Collections;
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
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchPause();
    }

    public void BackToMenuBtn()
    {
        GameManager.instance.data.LoadData();
        if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
        GameManager.instance.sceneC.LoadMenu();
    }

    public void SwitchPause()
    {
        isPaused = !isPaused;
        if (isPaused && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            if (GameManager.instance.sceneC.IsALvlScene()) {
                FindObjectOfType<PlayerController>().SetIsLocked(true);
                FindObjectOfType<PlayerSoundsManager>().StopSound();
            }
        }
        else if (!isPaused && Cursor.lockState != CursorLockMode.Locked && GameManager.instance.sceneC.IsALvlScene() && !FindObjectOfType<CursedGirlEnemy>().IsOnFinalDecisionPhase())
        {
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<PlayerController>().SetIsLocked(false);
        }
        optionPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.000001f : 1;
    }
}
