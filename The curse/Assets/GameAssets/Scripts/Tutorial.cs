using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    bool unlocked = false;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !unlocked){
            FindObjectOfType<TutorialManager>().UnlockTutorial(int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1)));
        }
    }

    public void SetUnlocked(bool value) { unlocked = value; }


}
