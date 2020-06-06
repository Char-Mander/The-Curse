using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    [SerializeField]
	private GameObject objectToPickup;
    [SerializeField]
    private GameObject pickedObject;
    [SerializeField]
    private Transform interactionZone;

    public void PickOrDropObject()
    {
        if(objectToPickup != null && pickedObject == null && objectToPickup.GetComponent<PickableObject>().isPickable() && !FindObjectOfType<PlayerController>().IsOnAMount())
        {
                FindObjectOfType<PlayerController>().soundsManager.ManageObjectPickUp();
                //Cogemos el objeto
                pickedObject = objectToPickup;
				//Decimos que ya no se puede coger (pues estamos cogiéndolo)
				pickedObject.GetComponent<PickableObject>().setPickable(false);
                //Lo hacemos hijo de la interactionZone y le asignamos la posición
				pickedObject.transform.SetParent(interactionZone);
				pickedObject.transform.position = interactionZone.position;
                //Quitamos las físicas de objeto para que no caiga ni cambie de posición
				pickedObject.GetComponent<Rigidbody>().useGravity = false;
				pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<PlayerController>().GetActiveWeaponAndCrossHair();
                GetComponent<PlayerController>().EnableWeapon(false);
                
		}
        else if(pickedObject != null && !FindObjectOfType<PlayerController>().IsOnAMount())
		{
				//Decimos que se puede volver a coger
				pickedObject.GetComponent<PickableObject>().setPickable(true);
				//Le quitamos el padre
				pickedObject.transform.SetParent(null);
				//Volvemos a poner las físicas por defecto
				pickedObject.GetComponent<Rigidbody>().useGravity = true;
				pickedObject.GetComponent<Rigidbody>().isKinematic = false;
				pickedObject = null;
                GetComponent<PlayerController>().EnableWeapon(true);
                FindObjectOfType<PlayerController>().soundsManager.ManageObjectPickUp();
        }
    }

    public void SetObjectToPickUp(GameObject obj)
    {
        this.objectToPickup = obj;
    }

    public bool IsPickingAnObject() { return pickedObject != null; }
}
