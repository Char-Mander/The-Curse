using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    float timeBetweenScene = 1f;
    bool lockSelection = false;

    public void LoadSceneLvl()
    {
        GameManager.instance.ShowOrHideLoadingPanel(true);
        if (!lockSelection)
        StartCoroutine(LoadLvlWait());   
    }

    public void LoadMenu()
    {
        GameManager.instance.ShowOrHideLoadingPanel(true);
        if(IsALvlScene()) GameManager.instance.data.SaveWeaponsAmmo(FindObjectOfType<WeaponController>().GetWeaponListLenght());
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.ShowOrHideLoadingPanel(false);
    }

    public void LoadGameOver()
    {
        GameManager.instance.ShowOrHideLoadingPanel(true);
        if (!lockSelection)
        {
            //Guarda la información antes de cargar la pantalla del GameOver
            GameManager.instance.SetCurrentPlayerHealth(FindObjectOfType<PlayerController>().GetComponent<Health>().GetCurrentHealth());
            GameManager.instance.data.SaveData(GameManager.instance.GetCurrentCheckPoint(), GameManager.instance.GetCurrentQuest(), 
                                                GameManager.instance.GetCurrentPlayerHealth(), GameManager.instance.GetDeaths(),
                                                GameManager.instance.GetDefeatedEnemies(), GameManager.instance.GetDecision());
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
        GameManager.instance.ShowOrHideLoadingPanel(false);
    }
    


    IEnumerator LoadGameOverWait()
    {
        lockSelection = true;
        yield return new WaitForSeconds(timeBetweenScene);
        SceneManager.LoadScene("GameOver");
        lockSelection = false;
        GameManager.instance.ShowOrHideLoadingPanel(false);
    }


}
