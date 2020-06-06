using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    bool unlocked = false;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !unlocked && GameObject.FindGameObjectWithTag("InitialCinematic").GetComponent<PlayGenericCinematic>().HasBeenPlayed())
        {
            FindObjectOfType<TutorialManager>().UnlockTutorial(int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
            FindObjectOfType<PlayerController>().soundsManager.StopSound();
        }
    }

    public void SetUnlocked(bool value) { unlocked = value; }


}
