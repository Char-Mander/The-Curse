using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    Quest[] auxList;
    private List<Quest> questList = new List<Quest>();
    Quest currentQuest;

    private void Start()
    {
        auxList = FindObjectsOfType<Quest>();
        OrderQuests();
        currentQuest = GameManager.instance.data.HasPreviousData() ? questList[GameManager.instance.GetCurrentQuest()] : null;
        if(currentQuest!=null) FindObjectOfType<FixedElementCanvasController>().UpdateQuestPanel(currentQuest.GetText());
    }

    private void OrderQuests()
    {
        Quest aux;
        for (int i = 1; i < auxList.Length; i++)
        {
            for (int j = auxList.Length - 1; j >= i; j--)
            {
                if (int.Parse(auxList[j - 1].gameObject.name.Substring(auxList[j - 1].gameObject.name.Length - 1)) > int.Parse(auxList[j].gameObject.name.Substring(auxList[j].gameObject.name.Length - 1)))
                {
                    aux = auxList[j - 1];
                    auxList[j - 1] = auxList[j];
                    auxList[j] = aux;
                }
            }
        }

        for(int i=0; i < auxList.Length; i++){
            questList.Add(auxList[i]);
        }
    }

    public void ActiveQuest()
    {
            int index = 0;
            if (currentQuest != null)
            {
                currentQuest.CompleteQuest();
                currentQuest.SetAsCurrentQuest(false);
                index = questList.IndexOf(currentQuest) + 1;
            }
            currentQuest = questList[index];
            currentQuest.SetAsCurrentQuest(true);
            FindObjectOfType<FixedElementCanvasController>().UpdateQuestPanel(currentQuest.GetText());
    }

    public int GetIndexOfCurrentQuest() { return questList.IndexOf(currentQuest); }
 
}
