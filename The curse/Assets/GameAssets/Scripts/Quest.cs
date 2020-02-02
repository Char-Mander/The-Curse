using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{   [SerializeField]
    string text;
    bool isActive = false;
    bool completed = false;
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

    public void CompleteQuest()
    {
        completed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!completed && other.CompareTag("Player"))
        {
            questController.ActiveQuest();
        }
    }

    public bool IsActive() { return isActive; }

    public bool IsCompleted() { return completed; }
}
