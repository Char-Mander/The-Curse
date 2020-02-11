using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    CheckPoint[] cpList;
    CheckPoint currentCP;

    private void Start()
    {
        cpList = FindObjectsOfType<CheckPoint>();
        OrderCheckPoints();
        ActiveCP(GameManager.instance.data.HasPreviousData() ? cpList[GameManager.instance.GetCurrentCheckPoint()] : cpList[0]);
        Respawn();
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

    public void ActiveCP(CheckPoint cp)
    {
        foreach (CheckPoint checkPoint in cpList)
        {
            checkPoint.SetAsCurrentCP(false);
        }
        cp.SetAsCurrentCP(true);
        currentCP = cp;
        int index = GetIndexOfCP(cp);
        GameManager.instance.SetCurrentCheckPoint(index);
        print("En el checkpoint " + cp.name + " el índice de la quest es: " + GameManager.instance.GetCurrentQuest());
        GameManager.instance.data.SaveData(index, FindObjectOfType<QuestController>().GetIndexOfCurrentQuest(), 
            FindObjectOfType<PlayerController>().GetComponent<Health>().GetCurrentHealth(),  GameManager.instance.GetDeaths(), 
            GameManager.instance.GetDefeatedEnemies(), GameManager.instance.GetDecision());
    }

    public void Respawn()
    {
        foreach (CheckPoint checkPoint in cpList)
        {
            if (checkPoint.IsActive())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Health>().GainHealth(0);
                player.transform.position = checkPoint.GetSpawnPoint().position;
            }
        }
    }

    public int GetIndexOfCP(CheckPoint cp)
    {
        int index = 0;
        bool found = false;
        while (!found)
        {
            if (cpList[index] == cp) found = true;
            else index++;
        }
        return index;
    }

    public int GetIndexOfCurrentCheckPoint(){ return GetIndexOfCP(currentCP); }
}
