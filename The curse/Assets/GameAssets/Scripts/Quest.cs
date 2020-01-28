using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    string text;
    Transform iniQuest;
    Transform endQuest;
    bool isActive;
    QuestController questController;
    // Start is called before the first frame update
    void Start()
    {
        questController = FindObjectOfType<QuestController>();
    }
    
    public void SetAsCurrentQuest(bool value)
    {
        isActive = value;
    }

    public bool IsActive() { return isActive; }
}
