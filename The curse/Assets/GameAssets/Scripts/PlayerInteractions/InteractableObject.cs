using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool interactable = true;
    //string objectText; 
    Dialogue dialogue;
    //Se iría fuera con el dialogue
    [SerializeField]
    private string objectText;
    [SerializeField]
    private float interactionTime;
    [SerializeField]
    private bool isNpc;
    [SerializeField]
    private bool complexNpc;

    private void Start()
    {
        dialogue = isNpc ? GetComponent<Dialogue>() : null;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().SetGameObjectToInteract(this.gameObject);
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().SetGameObjectToInteract(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().SetGameObjectToInteract(null);
        }
    }

    public bool isInteractable() { return interactable; }

    public void setInteractable(bool interact)
    {
        interactable = interact;
    }
    public float GetInteractionTime() { return interactionTime; }

    public string GetObjectText() { return objectText; }

    public bool IsNpc() { return isNpc; }

    public bool IsComplexNpc() { return complexNpc; }

    public Dialogue GetDialogue() { return dialogue; }
}
