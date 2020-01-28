using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InteractableSentences
{

    public InteractableSentences(int index, List<string> options)
    {
        this.index = index;
        sentenceOptions = options;
    }

    public int index { get; set; }
    public List<string> sentenceOptions { get; set; }
}

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    private bool canPlayerChoose;
    [SerializeField]
    private List<InteractableSentences> answersToSentences;
    [SerializeField]
    private List<string> sentences;
    
    private int interactableSentenceIndex=0;

    //TODO
    private void Start()
    {
        LoadAnswers();
    }

    private void LoadAnswers()
    {

    }

    public string GetName() { return name; }

    public List<string> GetSentences(){ return sentences; }

    public int GetIndexOfSentence(string sentence) { return sentences.IndexOf(sentence); }

    public bool CanPlayerChoose() { return canPlayerChoose; }

    public int GetInteractableSentenceIndex() { return answersToSentences[interactableSentenceIndex].index; }

    public void NextInteractableSentenceIndex()
    {
        if (interactableSentenceIndex < answersToSentences.Count - 1)
            interactableSentenceIndex++;
        else canPlayerChoose = false;
    }
}
