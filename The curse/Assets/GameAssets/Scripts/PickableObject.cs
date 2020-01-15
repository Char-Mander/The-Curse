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
		}
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
		{
			other.GetComponentInParent<PickUpObjects>().SetObjectToPickUp(null);
		}
	}

    public bool isPickable() { return pickable; }

    public void setPickable(bool pick)
	{
		pickable = pick;
	}
}
