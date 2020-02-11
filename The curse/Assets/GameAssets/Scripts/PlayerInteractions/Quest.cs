using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{   [SerializeField]
    string text;
    bool isActive = false;
    bool completed;
    bool triggered = false;
    QuestController questController;
    // Start is called before the first frame update
    void Start()
    {
        questController = FindObjectOfType<QuestController>();
        completed = !((GameManager.instance.GetCurrentQuest()) < int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
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
        if(!completed && !triggered && other.CompareTag("Player"))
        {
            ActivateQuest();
        }
    }
    
    public void ActivateQuest()
    {
        triggered = true;
        questController.ActiveQuest();
    }

    public string GetText() { return text; }

    public bool IsActive() { return isActive; }

    public bool IsCompleted() { return completed; }

    public bool HasBeenTriggered() { return triggered; }
}
