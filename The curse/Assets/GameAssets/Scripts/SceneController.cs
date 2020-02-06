using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public void LoadSceneLvl()
    {
        SceneManager.LoadScene("Lvl");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver()
    {
        //Guarda la información antes de cargar la pantalla del GameOver
        GameManager.instance.SetCurrentPlayerHealth(FindObjectOfType<PlayerController>().GetComponent<Health>().GetCurrentHealth());
        GameManager.instance.data.SaveData(GameManager.instance.GetCurrentCheckPoint(), GameManager.instance.GetCurrentQuest(), GameManager.instance.GetCurrentPlayerHealth());
        SceneManager.LoadScene("GameOver");
    }

    public bool IsALvlScene()
    {
        return SceneManager.GetActiveScene().name.Contains("Lvl");
    }

}
