using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    FixedElementCanvasController canvasC;
    Dialogue dialogue;
    bool isOnADialogue = false;

    private Queue<string> sentences = new Queue<string>();
    

    public void StartDialogue(Dialogue dialogue)
    {
        if (!isOnADialogue)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().EnableOrDisableCharacterController(false);
            isOnADialogue = true;
            sentences.Clear();
            this.dialogue = dialogue;
            foreach (string sentence in dialogue.GetSentences())
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence();
        }
    }

    private void DisplayNextSentence()
    {
        StartCoroutine(WaitForDisplay(0));
    }

    IEnumerator WaitForDisplay(int index)
    {   if (index < dialogue.GetSentences().Count)
        {
            string sentence = sentences.Dequeue();
            float textTime = sentence.Length > 30 ? (float)sentence.Length / 20 : 1f;
            canvasC.UpdateTextPanel(sentence, true, dialogue.GetName());
            yield return new WaitForSeconds(canvasC.GetTextTime());
            /*if (dialogue.CanPlayerChoose() && dialogue.GetIndexOfSentence(sentence) == dialogue.GetInteractableSentenceIndex())
            {
                dialogue.NextCurrentSentenceIndex();
                //Lo muestra y hace sus cosas 
                StartCoroutine(WaitForDisplay(index + 1));
            }
            else */ StartCoroutine(WaitForDisplay(index + 1));
        }
        else
        {
            EndDialogue();
            yield return null;
        }
    }

    private void EndDialogue()
    {
        isOnADialogue = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().EnableOrDisableCharacterController(true);
    }

    public bool IsOnADialogue() { return isOnADialogue; }
}
