using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    bool isActive;
    bool hasBeenActivated;
    CheckPointController checkPointController;

    private void Start()
    {
        checkPointController = FindObjectOfType<CheckPointController>();
        hasBeenActivated = !((GameManager.instance.GetCurrentCheckPoint()) < int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
    }


    public void SetAsCurrentCP(bool value)
    {
        isActive = value;
    }

    public bool IsActive() { return isActive; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isActive && !hasBeenActivated)
        {
            hasBeenActivated = true;
            print("Checkpoint " + this.gameObject.name + " activated");
            checkPointController.ActiveCP(this);
        }
    }

    public Transform GetSpawnPoint() { return spawnPoint; }
}
