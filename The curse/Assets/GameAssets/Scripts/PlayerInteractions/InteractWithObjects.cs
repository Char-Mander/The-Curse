using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToInteract;
    [SerializeField]
    private GameObject currentInteractionObject;
    [SerializeField]
    private Transform interactionZone;

    private bool canInteract=true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void InteractWithObject()
    {
        if (objectToInteract != null && objectToInteract.GetComponent<InteractableObject>().isInteractable() && canInteract)
        {
            //Cogemos el objeto
            currentInteractionObject = objectToInteract;
            //Hacemos que se actualice el texto del objeto en el panel del canvas
            GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>().UpdateTextPanel(currentInteractionObject.GetComponent<InteractableObject>().GetObjectInteractionText());
            //Decimos que ya no se puede interactuar con el durante un tiempo
            StartCoroutine(WaitForInteractAgain());
        }
    }

    public void SetObjectToInteract(GameObject obj)
    {
        this.objectToInteract = obj;
    }

    IEnumerator WaitForInteractAgain()
    {
        canInteract = false;
        currentInteractionObject.GetComponent<InteractableObject>().setInteractable(false);
        yield return new WaitForSeconds(currentInteractionObject.GetComponent<InteractableObject>().GetInteractionTime());
        canInteract = true;
        currentInteractionObject.GetComponent<InteractableObject>().setInteractable(true);
        currentInteractionObject = null;
    }
}
