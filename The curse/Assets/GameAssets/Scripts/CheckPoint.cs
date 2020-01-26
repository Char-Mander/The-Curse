using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    bool isActive;
    //CheckPointController cpController;

    private void Start()
    {
        //cpController = FindObjectOfType<CheckPointController>();
    }


    public void SetAsCurrentCP(bool value)
    {
        isActive = value;
    }

    public bool IsActive() { return isActive; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isActive)
        {
            //cpController.ActiveCP(this);
        }
    }

    public Transform GetSpawnPoint() { return spawnPoint; }
}
