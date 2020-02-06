using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataManager : MonoBehaviour
{
    private string currentCheckPointKey = "Current CheckPoint";
    private string currentQuestKey = "Current Quest";
    private string currentPlayerHealthKey = "Current Player Health";

    public void LoadData()
    {
        //Carga el punto en el que se quedó
        if (PlayerPrefs.HasKey(currentCheckPointKey))
            GameManager.instance.SetCurrentWayPoint(PlayerPrefs.GetInt(currentCheckPointKey));
        //Carga la misión actual
        if (PlayerPrefs.HasKey(currentQuestKey))
            GameManager.instance.SetCurrentQuest(PlayerPrefs.GetInt(currentQuestKey));
        //Carga la vida actual del player
        if (PlayerPrefs.HasKey(currentPlayerHealthKey))
            GameManager.instance.SetCurrentPlayerHealth(PlayerPrefs.GetFloat(currentPlayerHealthKey));
    }
    
    public void SaveData(int currentCheckPoint, int currentQuest, float currentHealth)
    {
        FindObjectOfType<FixedElementCanvasController>().ShowSavePanel();
        //Guarda el punto en el que se quedó
        PlayerPrefs.SetInt(currentCheckPointKey, currentCheckPoint);
        //Guarda la misión actual
        PlayerPrefs.SetInt(currentQuestKey, currentQuest);
        //Guarda la vida del player
        PlayerPrefs.SetFloat(currentPlayerHealthKey, currentHealth);
        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        DeletePrefs();
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
