using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataManager : MonoBehaviour
{
    private string currentCheckPointKey = "Current CheckPoint";
    private string currentQuestKey = "Current Quest";
    private string currentPlayerHealthKey = "Current Player Health";
    private string deathsKey = "Deaths";
    private string defeatedEnemiesKey = "Defeated Enemies";
    private string decisionStateKey = "Decision State";
    private string teleportPointKey = "Teleport Point";

    public void LoadData()
    {
        //Carga el punto en el que se quedó
        if (PlayerPrefs.HasKey(currentCheckPointKey))
            GameManager.instance.SetCurrentCheckPoint(PlayerPrefs.GetInt(currentCheckPointKey));
        //Carga la misión actual
        if (PlayerPrefs.HasKey(currentQuestKey))
            GameManager.instance.SetCurrentQuest(PlayerPrefs.GetInt(currentQuestKey));
        //Carga la vida actual del player
        if (PlayerPrefs.HasKey(currentPlayerHealthKey))
            GameManager.instance.SetCurrentPlayerHealth(PlayerPrefs.GetFloat(currentPlayerHealthKey));
        //Carga las muertes del player
        if (PlayerPrefs.HasKey(deathsKey))
            GameManager.instance.SetDeaths(PlayerPrefs.GetInt(deathsKey));
        //Carga los enemigos a los que ha matado el player
        if (PlayerPrefs.HasKey(defeatedEnemiesKey))
            GameManager.instance.SetDefeatedEnemies(PlayerPrefs.GetInt(defeatedEnemiesKey));
        //Carga la decisión del player
        if (PlayerPrefs.HasKey(decisionStateKey))
            GameManager.instance.SetDecision(PlayerPrefs.GetInt(decisionStateKey));
    }

    public void LoadTeleportPoints()
    {
       int length = FindObjectOfType<TeleportController>().GetTeleportPointListLength();
        for(int i = 0; i < length; i++)
        {
            if (PlayerPrefs.HasKey(teleportPointKey + (i + 1).ToString()) && 
                !GameManager.instance.ContainsDiscoveredTeleportPoint(PlayerPrefs.GetInt(teleportPointKey + (i + 1).ToString())))
            {
                GameManager.instance.AddTeleportPoint(PlayerPrefs.GetInt(teleportPointKey + (i+1).ToString()));
            }
        }
    }

    public void SaveHealth(float currentHealth)
    {
        //Guarda la vida del player
        PlayerPrefs.SetFloat(currentPlayerHealthKey, currentHealth);
        PlayerPrefs.Save();
    }

    public void SaveTeleportPoints(int index)
    {
        //Guarda como clave el "Teleport Point + index"
        PlayerPrefs.SetInt(teleportPointKey + (index+1).ToString(), index);
    }

    public void SaveData(int currentCheckPoint, int currentQuest, float currentHealth, int deaths, int defeatedEnemies, int decisionState)
    {
        FindObjectOfType<FixedElementCanvasController>().ShowSavePanel();
        //Guarda el punto en el que se quedó
        PlayerPrefs.SetInt(currentCheckPointKey, currentCheckPoint);
        //Guarda la misión actual
        PlayerPrefs.SetInt(currentQuestKey, currentQuest);
        //Guarda la vida del player
        PlayerPrefs.SetFloat(currentPlayerHealthKey, currentHealth);
        //Guarda las muertes del player
        PlayerPrefs.SetInt(deathsKey, deaths);
        //Guarda los enemigos a los que ha matado el player
        PlayerPrefs.SetInt(defeatedEnemiesKey, defeatedEnemies);
        //Guarda la decisión del player
        PlayerPrefs.SetInt(decisionStateKey, decisionState);

        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        DeletePrefs();
        GameManager.instance.ResetData();
    }

    [MenuItem("Utilidades/DeletePlayerPrefs")]
    public static void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public bool HasPreviousData()
    {
        return PlayerPrefs.HasKey(currentCheckPointKey) && PlayerPrefs.HasKey(currentQuestKey) && PlayerPrefs.HasKey(currentPlayerHealthKey);
    }
}
