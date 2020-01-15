using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
	public GameObject ObjectToPickup;
	public GameObject PickedObject;
	public Transform interactionZone;
    //private GameObject idleArms;
    //private GameObject pickingUpArms;
    //private GameObject[] arms;

    private void Start()
    {

        /* arms = GameObject.FindGameObjectsWithTag("Arm");
         if (arms.Length > 0)
         {
             Debug.Log("Encontrados los brazos");
             //idleArms.position = new Vector3(0, -1.23f, 0.5f);
             //idleArms.rotation = Quaternion.Euler(new Vector3(arms[0].transform.eulerAngles.x + 90f, 0, 0));
             pickingUpArms = new GameObject();
             pickingUpArms.transform.localPosition = new Vector3(0, 0, 0);
             pickingUpArms.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
             idleArms = new GameObject();
             idleArms.transform.position = arms[0].transform.localPosition;
             idleArms.transform.rotation = arms[0].transform.localRotation;

         */
        /* for(int i=0; i<arms.Length; i++)
         {
             idleArms[i] = arms[i].transform;
             pickingUpArms[i] = arms[i].transform;
             pickingUpArms[i].position = new Vector3(arms[0].transform.position.x, arms[0].transform.position.y - 1.23f, arms[0].transform.position.z - 0.5f);
             pickingUpArms[i].rotation = Quaternion.Euler(new Vector3(arms[0].transform.eulerAngles.x + 90f, 0f, 0f));
         }*/

        //}
    }


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

             /*  for(int i=0; i<arms.Length; i++)
                {
                    Debug.Log("Poniendo los brazos en alto");
                    arms[i].transform.position = idleArms.transform.localPosition;
                    arms[i].transform.rotation = idleArms.transform.localRotation;
                }*/
            
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

                /*for (int i = 0; i < arms.Length; i++)
                {
                    Debug.Log("Poniendo los brazos abajo");
                    arms[i].transform.position = pickingUpArms.transform.localPosition;
                    arms[i].transform.rotation = pickingUpArms.transform.localRotation;
                }*/
            
		}
    }
}
