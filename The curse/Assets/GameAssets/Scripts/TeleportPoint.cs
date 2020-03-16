using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField]
    string name;
    bool discovered = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!discovered) FindObjectOfType<TeleportController>().ActivateTP(this);

            if (Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<TeleportController>().ShowTP();
            }
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<TeleportController>().HideTP();
        }
    }*/

    public string GetName() { return name; }

    public bool HasBeenDiscovered() { return discovered; }

    public void SetDiscovered(bool value)
    {
        discovered = value;
    }
}
