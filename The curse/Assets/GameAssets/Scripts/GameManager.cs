using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public DataManager data;
    [HideInInspector]
    public OptionsController optionsC;
    [HideInInspector]
    public SceneController sceneC;
    private int maxCheckPoints=6;
    [SerializeField]
    private int maxQuests = 5;

    private int currentCheckPoint = 0;
    private int currentQuest = 0;
    private float maxPlayerHealth = -1;
    private float currentPlayerHealth = -1;
    private int deaths = 0;
    private int defeatedEnemies = 0;
    private int decisionState = 0;


    private void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
        DontDestroyOnLoad(this.gameObject);

        //References
        data = GetComponent<DataManager>();
        optionsC = GetComponentInChildren<OptionsController>();
        sceneC = GetComponent<SceneController>();

        //Init
        data.LoadData();
        //InitData();
    }
    
    public void ResetData()
    {
        currentCheckPoint = 0;
        currentQuest = 0;
        currentPlayerHealth = maxPlayerHealth;
        deaths = 0;
        defeatedEnemies = 0;
        decisionState = 0;
    }

    public int GetMaxCheckPoints() { return maxCheckPoints; }

    public int GetCurrentCheckPoint() { return currentCheckPoint; }

    public void SetCurrentCheckPoint(int index)
    {
        currentCheckPoint = index;
    }

    public int GetMaxQuests() { return maxQuests; }

    public int GetCurrentQuest() { return currentQuest; }

    public void SetCurrentQuest(int index)
    {
        currentQuest = index;
    }

    public void SetPlayerMaxHealth(float health)
    {
        maxPlayerHealth = health;
    }

    public float GetPlayerMaxHealth() { return maxPlayerHealth; }

    public void SetCurrentPlayerHealth(float health)
    {
        currentPlayerHealth = health;
    }

    public float GetCurrentPlayerHealth() { return currentPlayerHealth; }

    public int GetDeaths() { return deaths; }

    public void SetDeaths(int value)
    {
        deaths = value;
    }

    public int GetDefeatedEnemies() { return defeatedEnemies; }

    public void SetDefeatedEnemies(int value)
    {
        defeatedEnemies = value;
    }

    public int GetDecision() { return decisionState; }

    public void SetDecision(int value)
    {
        decisionState = value;
    }
}
