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
    //Tiempo de espera para volver a interaccionar con un objeto
    private bool canInteract=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void InteractWithGameObject()
    {
        if (objectToInteract != null && objectToInteract.GetComponent<InteractableObject>().isInteractable() 
            && canInteract && !FindObjectOfType<DialogueManager>().IsOnADialogue())
        {
            //Cogemos el objeto
            currentInteractionObject = objectToInteract;
            InteractableObject interactableObj = objectToInteract.GetComponent<InteractableObject>();
            Dialogue dialogue = interactableObj.GetDialogue();
            //Hacemos que se actualice el texto del objeto en el panel del canvas
            if (interactableObj.IsNpc() && interactableObj.IsComplexNpc() && !FindObjectOfType<PlayerController>().IsOnAMount())
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
            else if (interactableObj.CompareTag("Mount"))
            {
                if (!GetComponent<PlayerController>().IsOnAMount()) interactableObj.GetComponent<Mount>().PlayerClimbsOn();
                else interactableObj.GetComponent<Mount>().PlayerGetsOff();
            }
            else if(!FindObjectOfType<PlayerController>().IsOnAMount())
            {
                string chosenText = (dialogue != null) ? dialogue.GetSentences()[(int)Random.Range(0, dialogue.GetSentences().Count)].sentence
                                    : interactableObj.GetObjectText();
                if (interactableObj.IsNpc()) interactableObj.GetComponent<NPC>().StartTalking();
                FindObjectOfType<FixedElementCanvasController>().UpdateTextPanel(chosenText, interactableObj.IsNpc(),
                    (dialogue!=null) ? dialogue.GetName() : null);

                if (currentInteractionObject.GetComponentInChildren<Quest>() != null && !currentInteractionObject.GetComponentInChildren<Quest>().HasBeenTriggered())
                {
                    StartCoroutine(WaitForActivateQuest());
                }
            }
            else if (FindObjectOfType<PlayerController>().IsOnAMount() && !objectToInteract.CompareTag("Mount"))
            {
                objectToInteract = FindObjectOfType<Mount>().gameObject;
            }
        }
    }

    public void SetGameObjectToInteract(GameObject obj)
    {
        this.objectToInteract = obj;
    }

    IEnumerator WaitForActivateQuest()
    {
        yield return new WaitForSeconds(1);
        currentInteractionObject.GetComponentInChildren<Quest>().ActivateQuest();
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
