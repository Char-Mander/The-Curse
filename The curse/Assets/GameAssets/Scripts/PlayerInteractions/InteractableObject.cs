using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool interactable = true;
    Dialogue dialogue;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().AddGameObjectToInteract(this.gameObject);
            if(GetComponentInChildren<InteractableObjectCanvasController>() != null 
                && !(FindObjectOfType<PlayerController>().IsOnAMount() && this.GetComponent<Mount>() != null) ) GetComponentInChildren<InteractableObjectCanvasController>().ShowOrHidePanel(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().RemoveGameObjectToInteract(this.gameObject);
            if(GetComponentInChildren<InteractableObjectCanvasController>() != null) GetComponentInChildren<InteractableObjectCanvasController>().ShowOrHidePanel(false);
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
