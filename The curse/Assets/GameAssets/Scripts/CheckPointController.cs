using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    CheckPoint[] cpList;

    private void Start()
    {
        cpList = FindObjectsOfType<CheckPoint>();
        OrderCheckPoints();
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
