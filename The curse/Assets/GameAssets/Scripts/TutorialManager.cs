using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    Tutorial[] tList;

    private void Start()
    {
        tList = FindObjectsOfType<Tutorial>();
        OrderTutorials();
        //Carga los teleportpoints en el gamemanager
        GameManager.instance.data.LoadTeleportPoints(tList.Length);
        //Los activa
        for (int i = 0; i < tList.Length; i++)
        {
            tList[i].SetUnlocked(GameManager.instance.data.LoadTutorialPoint(i));
        }
    }

    private void OrderTutorials()
    {
        Tutorial aux;
        for (int i = 1; i < tList.Length; i++)
        {
            for (int j = tList.Length - 1; j >= i; j--)
            {
                if (int.Parse(tList[j - 1].gameObject.name.Substring(tList[j - 1].gameObject.name.Length - 1)) > int.Parse(tList[j].gameObject.name.Substring(tList[j].gameObject.name.Length - 1)))
                {
                    aux = tList[j - 1];
                    tList[j - 1] = tList[j];
                    tList[j] = aux;
                }
            }
        }
    }

    public void UnlockTutorial(int index)
    {
        tList[index].SetUnlocked(true);
        FindObjectOfType<DialogueManager>().StartDialogue(tList[index].GetComponent<Dialogue>());
        if (tList[index].gameObject.name.Contains("Mount"))
        {
            FindObjectOfType<Mount>().SetUnlocked(true);
            GameManager.instance.data.SaveUnlockedMount();
        }
        GameManager.instance.data.SaveTutorialPoint(index);
    }
}
