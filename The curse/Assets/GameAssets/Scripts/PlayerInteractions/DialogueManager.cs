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

    private Queue<Sentence> sentences = new Queue<Sentence>();
    

    public void StartDialogue(Dialogue dialogue)
    {
        if (!isOnADialogue)
        {
            FindObjectOfType<PlayerController>().SetIsLocked(true);
            isOnADialogue = true;
            sentences.Clear();
            this.dialogue = dialogue;
            foreach (Sentence sentence in dialogue.GetSentences())
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence(0);
        }
    }

    public void DisplayNextSentence(int index)
    {
        StartCoroutine(WaitForDisplay(index));
    }

    IEnumerator WaitForDisplay(int index)
    {   if (index < dialogue.GetSentences().Count)
        {
            Sentence s = sentences.Dequeue();
            float textTime = s.sentence.Length > 30 ? (float)s.sentence.Length / 20 : 1f;
            canvasC.UpdateTextPanel(s.sentence, true, dialogue.GetName());
            if (dialogue.CanPlayerChoose(index))
            {
                canvasC.UpdateSentenceOptionsPanel(s, index);
            }
            else {
                yield return new WaitForSeconds(canvasC.GetTextTime());
                DisplayNextSentence(index + 1);
            }
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
        FindObjectOfType<PlayerController>().SetIsLocked(false);
        if (Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
        if (dialogue.gameObject.GetComponent<CursedGirlEnemy>() && !dialogue.gameObject.GetComponent<CursedGirlEnemy>().GetHasSpoken())
        {
            dialogue.gameObject.GetComponent<CursedGirlEnemy>().StartAttackingMode();
        }
        else if (GetComponentInChildren<Quest>() && !GetComponentInChildren<Quest>().HasBeenTriggered())
        {
            GetComponentInChildren<Quest>().ActivateQuest();
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().EnableOrDisableCharacterController(true);

    }

    public bool IsOnADialogue() { return isOnADialogue; }
}
