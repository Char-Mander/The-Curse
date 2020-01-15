using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    [SerializeField]
	private GameObject ObjectToPickup;
    [SerializeField]
    private GameObject PickedObject;
    [SerializeField]
    private Transform interactionZone;

    private GameObject currentWeapon;
    private GameObject crossHair;
    // Update is called once per frame
    public void PickOrDropObject()
    {
        if(ObjectToPickup != null && ObjectToPickup.GetComponent<PickableObject>().isPickable())
		{
                //Cogemos el objeto
				PickedObject = ObjectToPickup;
				//Decimos que ya no se puede coger (pues estamos cogiéndolo)
				PickedObject.GetComponent<PickableObject>().setPickable(false);
                //Lo hacemos hijo de la interactionZone y le asignamos la posición
				PickedObject.transform.SetParent(interactionZone);
				PickedObject.transform.position = interactionZone.position;
                //Quitamos las físicas de objeto para que no caiga ni cambie de posición
				PickedObject.GetComponent<Rigidbody>().useGravity = false;
				PickedObject.GetComponent<Rigidbody>().isKinematic = true;
                currentWeapon = GameObject.FindGameObjectWithTag("Weapon");
                crossHair = GameObject.FindGameObjectWithTag("CrossHair");
                EnableWeapon(false);
                
		}
        else if(PickedObject != null)
		{
				//Decimos que se puede volver a coger
				PickedObject.GetComponent<PickableObject>().setPickable(true);
				//Le quitamos el padre
				PickedObject.transform.SetParent(null);
				//Volvemos a poner las físicas por defecto
				PickedObject.GetComponent<Rigidbody>().useGravity = true;
				PickedObject.GetComponent<Rigidbody>().isKinematic = false;
				PickedObject = null;
                EnableWeapon(true);
        }
    }

    //Función que desactiva o activa el arma y la mirilla
    private void EnableWeapon(bool enable)
    {
        currentWeapon.SetActive(enable);
        crossHair.SetActive(enable);
    }

    public void SetObjectToPickUp(GameObject obj)
    {
        this.ObjectToPickup = obj;
    }
}
