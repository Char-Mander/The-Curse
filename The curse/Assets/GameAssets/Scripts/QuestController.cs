using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    Quest currentQuest;
    Queue<Quest> questList = new Queue<Quest>();
    CheckPoint[] cpList; // 0 default CP

    private void Start()
    {
        cpList = FindObjectsOfType<CheckPoint>();
        //Rellenar lista de quest
        OrdenarCheckPoints();
    }

    private void OrdenarCheckPoints()
    {

    }

    public void ActiveCP(CheckPoint cp)
    {
        foreach (CheckPoint checkPoint in cpList)
        {
            checkPoint.SetAsCurrentCP(false);
        }
        cp.SetAsCurrentCP(true);
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
