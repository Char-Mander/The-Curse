using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    float timeBetweenScene = 0.75f;
    bool lockSelection = false;

    public void LoadSceneLvl()
    {
        if(!lockSelection)
        StartCoroutine(LoadLvlWait());   
    }

    public void LoadMenu()
    {
        if (!lockSelection)
            StartCoroutine(LoadMenuWait());
    }

    public void LoadGameOver()
    {
        if (!lockSelection)
        {
            //Guarda la información antes de cargar la pantalla del GameOver
            GameManager.instance.SetCurrentPlayerHealth(FindObjectOfType<PlayerController>().GetComponent<Health>().GetCurrentHealth());
            GameManager.instance.data.SaveData(GameManager.instance.GetCurrentCheckPoint(), GameManager.instance.GetCurrentQuest(), GameManager.instance.GetCurrentPlayerHealth());
            StartCoroutine(LoadGameOverWait());
        }
    }

    public bool IsALvlScene()
    {
        return SceneManager.GetActiveScene().name.Contains("Lvl");
    }

    IEnumerator LoadLvlWait()
    {
        lockSelection = true;
        yield return new WaitForSeconds(timeBetweenScene);
        SceneManager.LoadScene("Lvl");
        lockSelection = false;
    }

    IEnumerator LoadMenuWait()
    {
        lockSelection = true;
        yield return new WaitForSeconds(timeBetweenScene);
        SceneManager.LoadScene("MainMenu");
        lockSelection = false;
    }


    IEnumerator LoadGameOverWait()
    {
        lockSelection = true;
        yield return new WaitForSeconds(timeBetweenScene);
        SceneManager.LoadScene("GameOver");
        lockSelection = false;
    }


}
