using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{

    [SerializeField]
    private List<string> questTextList = new List<string>();
    Quest currentQuest;
    private Queue<Quest> questList = new Queue<Quest>();
    CheckPoint[] cpList; // 0 default CP

    private void Start()
    {
        cpList = FindObjectsOfType<CheckPoint>();
        OrderCheckPoints();
        AsignCheckPoints();
    }

    private void OrderCheckPoints()
    {
        CheckPoint aux;
        for (int i = 1; i < cpList.Length; i++)
        {
            for (int j = cpList.Length - 1; j >= i; j--)
            {
                if (int.Parse(cpList[j - 1].gameObject.name.Substring(cpList[j - 1].gameObject.name.Length - 1)) > int.Parse(cpList[j].gameObject.name.Substring(cpList[j].gameObject.name.Length - 1)))
                {
                    aux = cpList[j - 1];
                    cpList[j - 1] = cpList[j];
                    cpList[j] = aux;
                }
            }
        }
    }

    private void AsignCheckPoints()
    {
        for(int i=0; i<cpList.Length; i++)
        {
            Quest quest = new Quest();
            quest.InitQuest(questTextList[i], cpList[i]);
            questList.Enqueue(quest);
        }
    }

    public void ActiveCP(CheckPoint cp)
    {
        foreach (CheckPoint checkPoint in cpList)
        {
            checkPoint.SetAsCurrentCP(false);
        }
        cp.SetAsCurrentCP(true);
        currentQuest = questList.Dequeue();
    }

    public void Respawn()
    {
        foreach (CheckPoint checkPoint in cpList)
        {
            if (checkPoint.IsActive())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Health>().GainHealth(100000000);
                player.transform.position = checkPoint.GetSpawnPoint().position;
            }
        }
    }
}
