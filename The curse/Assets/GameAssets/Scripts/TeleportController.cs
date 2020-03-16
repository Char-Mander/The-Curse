using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    TeleportPoint[] tpList;

    private void Start()
    {
        tpList = FindObjectsOfType<TeleportPoint>();
        OrderTeleportPoints();
    }

    private void OrderTeleportPoints()
    {
        TeleportPoint aux;
        for (int i = 1; i < tpList.Length; i++)
        {
            for (int j = tpList.Length - 1; j >= i; j--)
            {
                if (int.Parse(tpList[j - 1].gameObject.name.Substring(tpList[j - 1].gameObject.name.Length - 1)) > int.Parse(tpList[j].gameObject.name.Substring(tpList[j].gameObject.name.Length - 1)))
                {
                    aux = tpList[j - 1];
                    tpList[j - 1] = tpList[j];
                    tpList[j] = aux;
                }
            }
        }
    }

    public void ActivateTP(TeleportPoint tp)
    {
        tp.SetDiscovered(true);
        int index = GetIndexOfTP(tp);
        GameManager.instance.AddTeleportPoint(index);
        //GameManager.instance.data.SaveData();
    }

    public void ShowTP()
    {
        FindObjectOfType<FixedElementCanvasController>().TeleportPointsDeployment(tpList);
    }

    /*
    public void HideTP()
    {
        FindObjectOfType<FixedElementCanvasController>().EnableOrDisableOptionsPanel(false);
    }
    */

    public void Teleport(int index)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = tpList[index].transform.position;
    }

    public int GetIndexOfTP(TeleportPoint tp)
    {
        int index = 0;
        bool found = false;
        while (!found)
        {
            if (tpList[index] == tp) found = true;
            else index++;
        }
        return index;
    }

    public int GetTeleportPointListLength() { return tpList.Length; }

}
