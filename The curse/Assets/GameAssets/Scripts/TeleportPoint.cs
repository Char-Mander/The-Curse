using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField]
    string name;
    bool discovered = false;
    bool usingTeleport = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!discovered)
            {
                FindObjectOfType<TeleportController>().ActivateTP(this);

            }

            if (Input.GetKeyDown(KeyCode.E) && !usingTeleport)
            {
                FindObjectOfType<TeleportController>().ShowTP();
                usingTeleport = true;
            }
        }
    }

    public string GetName() { return name; }

    public bool HasBeenDiscovered() { return discovered; }

    public void SetDiscovered(bool value)
    {
        discovered = value;
    }

    public void SetUsingTeleport(bool value)
    {
        usingTeleport = value;
    }
}
