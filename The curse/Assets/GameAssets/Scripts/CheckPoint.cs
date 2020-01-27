using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    bool isActive;
    QuestController questController;

    private void Start()
    {
        questController = FindObjectOfType<QuestController>();
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
            questController.ActiveCP(this);
        }
    }

    public Transform GetSpawnPoint() { return spawnPoint; }
}
