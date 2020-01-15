using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool interactable = true;
    [SerializeField]
    private string objectText;
    [SerializeField]
    private float interactionTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().SetObjectToInteract(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<InteractWithObjects>().SetObjectToInteract(null);
        }
    }

    public bool isInteractable() { return interactable; }

    public void setInteractable(bool pick)
    {
        interactable = pick;
    }

    public string GetObjectInteractionText() { return objectText; }

    public float GetInteractionTime() { return interactionTime; }
}
