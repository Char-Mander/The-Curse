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

    private int currentCheckPoint=0;
    private int currentQuest = 0;
    private float maxPlayerHealth=-1;
    private float currentPlayerHealth=-1;


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

    public int GetMaxCheckPoints() { return maxCheckPoints; }

    public int GetCurrentCheckPoint() { return currentCheckPoint; }

    public void SetCurrentWayPoint(int index)
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

    public void SetCurrentPlayerHealth(float health)
    {
        currentPlayerHealth = health;
    }

    public float GetCurrentPlayerHealth() { return currentPlayerHealth; }
}
