using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    FixedElementCanvasController canvasC;
    Dialogue dialogue;

    private Queue<string> sentences = new Queue<string>();
    

    public void StartDialogue(Dialogue dialogue)
    {   
        sentences.Clear();
        this.dialogue = dialogue;
        foreach(string sentence in dialogue.GetSentences())
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        StartCoroutine(WaitForDisplay(0));
    }

    IEnumerator WaitForDisplay(int index)
    {   if (index < sentences.Count)
        {
            string sentence = sentences.Dequeue();
            float textTime = sentence.Length > 30 ? (float)sentence.Length / 20 : 1f;
            canvasC.UpdateTextPanel(sentence, true, dialogue.GetName());
            yield return new WaitForSeconds(canvasC.GetTextTime());
            StartCoroutine(WaitForDisplay(index+1));
        }
        else yield return null;
    }
}
