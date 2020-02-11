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
        if(this.gameObject.name == "Quest4")
        {
            print("Valores de la quest4: triggered - " + triggered + "   completed - " + completed);
        }
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
        print("Entra al activateQuest");
        triggered = true;
        questController.ActiveQuest();
    }

    public string GetText() { return text; }

    public bool IsActive() { return isActive; }

    public bool IsCompleted() { return completed; }

    public bool HasBeenTriggered() { return triggered; }
}
