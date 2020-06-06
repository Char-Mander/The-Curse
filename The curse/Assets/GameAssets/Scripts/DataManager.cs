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
    private string weaponUnlockedKey = "Weapon";
    private string weaponAmmoKey = "Ammo";
    private string tutorialKey = "Tutorial";
    private string mountLockedKey = "Mount";
    private string initialCinematicKey = "Initial Cinematic";
    private string dataKey = "Data";
    bool hasPreviousData = false;

    private void Start()
    {
        if (PlayerPrefs.HasKey(dataKey))
            hasPreviousData = true;
    }

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

    public void LoadTeleportPoints(int size)
    {
        for(int i = 0; i < size; i++)
        {
            if (PlayerPrefs.HasKey(teleportPointKey + (i + 1).ToString()) && 
                !GameManager.instance.ContainsDiscoveredTeleportPoint(PlayerPrefs.GetInt(teleportPointKey + (i + 1).ToString())))
            {
                GameManager.instance.AddTeleportPoint(PlayerPrefs.GetInt(teleportPointKey + (i+1).ToString()));
            }
        }
    }

    public void LoadWeaponsUnlocked(int size)
    {
        for(int i=0; i<size; i++)
        {
            if (PlayerPrefs.HasKey(weaponUnlockedKey + i.ToString())) FindObjectOfType<WeaponController>().UnlockWeapon(i);
        }
    }

    public bool LoadTutorialPoint(int index)
    {
        if (PlayerPrefs.HasKey(tutorialKey + index.ToString())) return true;
        else return false;
    }

    public bool LoadInitialCinematic()
    {
        if (PlayerPrefs.HasKey(initialCinematicKey)) return true;
        else return false;
    }

    public bool LoadUnlockedMount()
    {
        if (PlayerPrefs.HasKey(mountLockedKey)) return true;
        else return false;
    }

    public void LoadWeaponsAmmo(int size)
    {
        for (int i = 0; i < size; i++)
        {
            if (FindObjectOfType<WeaponController>().HasAmmo(i))
            {
                if (PlayerPrefs.HasKey(weaponAmmoKey + i.ToString())) FindObjectOfType<WeaponController>().SetWeaponAmmo(i, PlayerPrefs.GetInt(weaponAmmoKey + i.ToString()));
                else FindObjectOfType<WeaponController>().SetWeaponAmmo(i, FindObjectOfType<WeaponController>().GetWeaponByIndex(i).GetMaxAmmo());
            }
        }
    }

    public void SaveWeaponsAmmo(int size)
    {
        for (int i = 0; i < size; i++)
        {
            if (FindObjectOfType<WeaponController>().HasAmmo(i)) PlayerPrefs.SetInt(weaponAmmoKey + i.ToString(), FindObjectOfType<WeaponController>().GetWeaponByIndex(i).GetCurrentAmmo());
        }
        SaveDataVariable();
    }

    public void SaveTutorialPoint(int index)
    {
        //Guarda como clave el "Teleport Point + index"
        PlayerPrefs.SetString(tutorialKey + (index).ToString(), "True");
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    public void SaveInitialCinematic()
    {
        PlayerPrefs.SetString(initialCinematicKey, "True");
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    public void SaveUnlockedMount()
    {
        PlayerPrefs.SetString(mountLockedKey, "True");
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    public void SaveHealth(float currentHealth)
    {
        //Guarda la vida del player
        PlayerPrefs.SetFloat(currentPlayerHealthKey, currentHealth);
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    public void SaveTeleportPoints(int index)
    {
        //Guarda como clave el "Teleport Point + index"
        PlayerPrefs.SetInt(teleportPointKey + (index+1).ToString(), index);
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    public void SaveWeaponUnlocked(int index)
    {
        if (!PlayerPrefs.HasKey(weaponUnlockedKey+index.ToString()))
            PlayerPrefs.SetInt(weaponUnlockedKey + (index).ToString(), index);
        SaveDataVariable();
        PlayerPrefs.Save();
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
        SaveDataVariable();
        PlayerPrefs.Save();
    }

    private void SaveDataVariable()
    {
        if (!PlayerPrefs.HasKey(dataKey))
        {
            PlayerPrefs.SetString(dataKey, "True");
        }
    }

    public void ResetData()
    {
        if (hasPreviousData)
        {
            hasPreviousData = false;
            if (PlayerPrefs.HasKey(currentCheckPointKey)) PlayerPrefs.DeleteKey(currentCheckPointKey);
            if (PlayerPrefs.HasKey(currentQuestKey)) PlayerPrefs.DeleteKey(currentQuestKey);
            if (PlayerPrefs.HasKey(currentPlayerHealthKey)) PlayerPrefs.DeleteKey(currentPlayerHealthKey);
            if (PlayerPrefs.HasKey(deathsKey)) PlayerPrefs.DeleteKey(deathsKey);
            if (PlayerPrefs.HasKey(defeatedEnemiesKey)) PlayerPrefs.DeleteKey(defeatedEnemiesKey);
            if (PlayerPrefs.HasKey(decisionStateKey)) PlayerPrefs.DeleteKey(decisionStateKey);


            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey(teleportPointKey + (i + 1).ToString())) PlayerPrefs.DeleteKey(teleportPointKey + (i + 1).ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey(weaponUnlockedKey + i.ToString())) PlayerPrefs.DeleteKey(weaponUnlockedKey + i.ToString());
                if (PlayerPrefs.HasKey(weaponAmmoKey + i.ToString())) PlayerPrefs.DeleteKey(weaponAmmoKey + i.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey(tutorialKey + i.ToString())) PlayerPrefs.DeleteKey(tutorialKey + i.ToString());
            }

            if (PlayerPrefs.HasKey(mountLockedKey)) PlayerPrefs.DeleteKey(mountLockedKey);
            if (PlayerPrefs.HasKey(initialCinematicKey)) PlayerPrefs.DeleteKey(initialCinematicKey);
            if (PlayerPrefs.HasKey(dataKey)) PlayerPrefs.DeleteKey(dataKey);
            GameManager.instance.ResetData();
        }
    }

    public bool HasPlayerData()
    {
        return PlayerPrefs.HasKey(currentCheckPointKey) && PlayerPrefs.HasKey(currentQuestKey) && PlayerPrefs.HasKey(currentPlayerHealthKey);
    }

    public bool HasPreviousData()
    {
        return hasPreviousData;
    }
}
