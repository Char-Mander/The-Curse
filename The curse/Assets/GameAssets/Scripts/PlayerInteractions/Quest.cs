using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{   [SerializeField]
    string text;
    bool isActive = false;
    bool completed = false;
    bool triggered = false;
    QuestController questController;
    // Start is called before the first frame update
    void Start()
    {
        questController = FindObjectOfType<QuestController>();
        completed = !((GameManager.instance.GetCurrentQuest()+1) < int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
        triggered = !((GameManager.instance.GetCurrentQuest()+1) < int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
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
        if(!completed && !triggered && other.GetComponentInParent<PlayerController>() != null)
        {
            ActivateQuest();
        }
    }
    
    public void ActivateQuest()
    {
        triggered = true;
        questController.ActiveQuest(this);
    }

    public string GetText() { return text; }

    public bool IsActive() { return isActive; }

    public bool IsCompleted() { return completed; }

    public bool HasBeenTriggered() { return triggered; }

    public void SetTriggered(bool value)
    {
        triggered = value;
    }
}
