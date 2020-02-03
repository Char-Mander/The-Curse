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
    [SerializeField]
    private int maxCheckPoints=1;
    [SerializeField]
    private int maxQuests = 1;

    private int currentWayPoint=0;
    private int currentQuest = 0;

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

    public int GetCurrentCheckPoint() { return currentWayPoint; }

    public void SetCurrentWayPoint(int index)
    {
        currentWayPoint = index;
    }

    public int GetMaxQuests() { return maxQuests; }

    public int GetCurrentQuest() { return currentQuest; }

    public void SetCurrentQuest(int index)
    {
        currentQuest = index;
    }
}
