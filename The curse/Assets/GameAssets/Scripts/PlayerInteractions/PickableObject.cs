using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
	private bool pickable = true;

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PlayerInteractionZone"))
        {
            other.GetComponentInParent<PickUpObjects>().SetObjectToPickUp(this.gameObject);
            GetComponentInChildren<InteractableObjectCanvasController>().ShowOrHidePanel(true);
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
		{
			other.GetComponentInParent<PickUpObjects>().SetObjectToPickUp(null);
            GetComponentInChildren<InteractableObjectCanvasController>().ShowOrHidePanel(false);
        }
	}

    public bool isPickable() { return pickable; }

    public void setPickable(bool pick)
	{
		pickable = pick;
	}
}
