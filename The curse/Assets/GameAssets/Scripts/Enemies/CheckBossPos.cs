using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBossPos : MonoBehaviour
{
    CursedGirlEnemy cursedGirl;
    bool isOnTheArea = false;
    bool canTeleport = false;
    // Start is called before the first frame update
    void Start()
    {
        cursedGirl = FindObjectOfType<CursedGirlEnemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CursedGirlEnemy>() != null)
        {
            isOnTheArea = true;
            canTeleport = false;
        }
        else isOnTheArea = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CursedGirlEnemy>() != null && !isOnTheArea)
        {
            canTeleport = true;
        }
    }

    public bool IsInsideTheCrypt() { return !canTeleport; }
}
