using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataManager : MonoBehaviour
{
    private string currentCheckPointKey = "Current CheckPoint";
    private string currentQuestKey = "Current Quest";
    
    public void LoadData()
    {
        //Carga el punto en el que se quedó
        if (PlayerPrefs.HasKey(currentCheckPointKey))
            GameManager.instance.SetCurrentWayPoint(PlayerPrefs.GetInt(currentCheckPointKey));
        //Carga la misión actual
        if (PlayerPrefs.HasKey(currentQuestKey))
            GameManager.instance.SetCurrentQuest(PlayerPrefs.GetInt(currentQuestKey));
    }
    
    public void SaveData(int currentWayPoint, int currentQuest)
    {
        //Guarda el punto en el que se quedó
        PlayerPrefs.SetInt(currentCheckPointKey, currentWayPoint);
        //Guarda la misión actual
        PlayerPrefs.SetInt(currentQuestKey, currentQuest);
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
        return PlayerPrefs.HasKey(currentCheckPointKey) && PlayerPrefs.HasKey(currentQuestKey);
    }
}
