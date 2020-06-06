using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportController : MonoBehaviour
{
    TeleportPoint[] tpList;
    PostProcessVolume volume;
    Vignette vLayerDV = null;

    bool teleportOptionsAvailable = false;

    private void Start()
    {
        tpList = FindObjectsOfType<TeleportPoint>();
        OrderTeleportPoints();
        //Carga los teleportpoints en el gamemanager
        GameManager.instance.data.LoadTeleportPoints(tpList.Length);
        //Los activa
        for (int i=0; i< GameManager.instance.GetDiscoveredTeleportPointLenght(); i++)
        {
            int tpIndex = GameManager.instance.GetDiscoveredTeleportPointIndex(i);
            tpList[tpIndex].SetDiscovered(true);
        }
        volume = GetComponent<PostProcessVolume>();
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
        if (!GameManager.instance.ContainsDiscoveredTeleportPoint(index))
        {
            GameManager.instance.AddTeleportPoint(index);
            GameManager.instance.data.SaveTeleportPoints(index);
        }
    }

    public void ShowTP()
    {
        FindObjectOfType<FixedElementCanvasController>().TeleportPointsDeployment(tpList);
    }


    public void HideTP()
    {
        foreach (TeleportPoint tp in tpList)
            tp.SetUsingTeleport(false);
    }

    public void Teleport(int index)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = tpList[index].tpPos.position;
        SetFastTravelProfile();
    }

    void SetFastTravelProfile()
    {
         StartCoroutine(FastTravelEffect());
    }

    IEnumerator FastTravelEffect()
    {
        if (vLayerDV == null)
        {
            volume.profile.TryGetSettings(out vLayerDV);
            vLayerDV.intensity.value = 1;
        }
        if (vLayerDV.intensity > 0)
        {
            vLayerDV.intensity.value -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(FastTravelEffect());
        }
        else
        {
            vLayerDV = null;
            FindObjectOfType<PostProcessManager>().SetGeneralProfile();
        }
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

    public bool GetTeleportOptionsAvailable() { return teleportOptionsAvailable; }

    public void SetTeleportOptionsAvailable(bool value)
    {
        teleportOptionsAvailable = value;
    }
}
