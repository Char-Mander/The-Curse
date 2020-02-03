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

    public void LoadSceneLvl(bool reset)
    {
        if (reset)
        {
            GameManager.instance.data.ResetData();
        }
        SceneManager.LoadScene("Lvl");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public bool IsALvlScene()
    {
        return SceneManager.GetActiveScene().name.Contains("Lvl");
    }

}
