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
    public void InteractWithGameObject()
    {
        if (objectToInteract != null && objectToInteract.GetComponent<InteractableObject>().isInteractable() && canInteract)
        {
            //Cogemos el objeto
            currentInteractionObject = objectToInteract;
            InteractableObject interactableObj = objectToInteract.GetComponent<InteractableObject>();
            Dialogue dialogue = interactableObj.GetDialogue();
            //Hacemos que se actualice el texto del objeto en el panel del canvas
            if (interactableObj.IsNpc() && interactableObj.IsComplexNpc())
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
            else
            {
                string chosenText = (dialogue != null) ? dialogue.GetSentences()[(int)Random.Range(0, dialogue.GetSentences().Count - 1)]
                                    : interactableObj.GetObjectText();
                FindObjectOfType<FixedElementCanvasController>().UpdateTextPanel(chosenText, interactableObj.IsNpc(),
                    (dialogue!=null) ? dialogue.GetName() : null);
            }
        }
    }

    public void SetGameObjectToInteract(GameObject obj)
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
