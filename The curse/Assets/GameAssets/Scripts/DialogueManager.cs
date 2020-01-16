using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    FixedElementCanvasController canvasC;

    private Queue<string> sentences = new Queue<string>();
    

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach(string sentence in dialogue.GetSentences())
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence(dialogue);
    }

    private void DisplayNextSentence(Dialogue dialogue)
    {
        for (int i = 0; i < sentences.Count; i++)
        {
            string sentence = sentences.Dequeue();
            float textTime = sentence.Length > 30 ? (float)sentence.Length / 20 : 1f;
            canvasC.UpdateTextPanel(sentence, true, dialogue.GetName());
        }
    }
}
