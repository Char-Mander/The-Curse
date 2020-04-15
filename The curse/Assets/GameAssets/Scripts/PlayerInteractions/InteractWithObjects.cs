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
    public List<GameObject> objectsToInteract = new List<GameObject>();
    //Tiempo de espera para volver a interaccionar con un objeto
    private bool canInteract=true;

    // Update is called once per frame
    public void InteractWithGameObject()
    {
        if(objectsToInteract.Count > 0)
        {
            //Cogemos la referencia del objeto más cercano
            GetNearest();
            print("Nearest: " + objectToInteract.name);
            if (objectToInteract != null && objectToInteract.GetComponent<InteractableObject>().isInteractable() 
            && canInteract && !FindObjectOfType<DialogueManager>().IsOnADialogue())
            {
                currentInteractionObject = objectToInteract;
                InteractableObject interactableObj = objectToInteract.GetComponent<InteractableObject>();
                Dialogue dialogue = interactableObj.GetDialogue();
                print("interactableobj: " + interactableObj.gameObject.name);
                //Hacemos que se actualice el texto del objeto en el panel del canvas
                if (interactableObj.IsNpc() && interactableObj.IsComplexNpc() && !FindObjectOfType<PlayerController>().IsOnAMount())
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
                else if (interactableObj.CompareTag("Mount"))
                {
                    if (!FindObjectOfType<PickUpObjects>().IsPickingAnObject() && FindObjectOfType<Mount>().IsUnlocked())
                    {
                        if (!GetComponent<PlayerController>().IsOnAMount()) interactableObj.GetComponent<Mount>().PlayerClimbsOn();
                        else interactableObj.GetComponent<Mount>().PlayerGetsOff();
                    }
                }
                else
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
            }
        }
    }

    public void GetNearest()
    {
        float minDistance = Vector3.Distance(interactionZone.position, objectsToInteract[0].transform.position);
        objectToInteract = objectsToInteract[0];
        if (objectsToInteract.Count > 1)
            for(int i = 1; i<objectsToInteract.Count; i++)
            {
                if(Vector3.Distance(interactionZone.position, objectsToInteract[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(interactionZone.position, objectsToInteract[i].transform.position);
                    objectToInteract = objectsToInteract[i];
                }
            }
    }

    public void AddGameObjectToInteract(GameObject obj)
    {
        if(!objectsToInteract.Contains(obj))  objectsToInteract.Add(obj);
    }

    public void RemoveGameObjectToInteract(GameObject obj)
    {
        objectsToInteract.Remove(obj);
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
        CanInteractAgain();
    }

    void CanInteractAgain()
    {
        canInteract = true;
        currentInteractionObject.GetComponent<InteractableObject>().setInteractable(true);
        currentInteractionObject = null;
    }
}
