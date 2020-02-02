using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    
    public Queue<Quest> questList = new Queue<Quest>();
    Quest currentQuest;

    private void Start()
    {

    }
    
    public void ActiveQuest()
    {
        currentQuest.CompleteQuest();
        currentQuest.SetAsCurrentQuest(false);
        currentQuest = questList.Dequeue();
        currentQuest.SetAsCurrentQuest(true);
    }

 

 
}
