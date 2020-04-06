using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SentenceOptions
{
    public string optionTxt;
    public int decisionBalance;
}

[System.Serializable]
public class Sentence
{
    public string sentence;
    public List<SentenceOptions> options = new List<SentenceOptions>();
}

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    private bool canPlayerChoose;
    [SerializeField]
    private List<Sentence> sentencesList;
        
    public string GetName() { return name; }

    public List<Sentence> GetSentences(){ return sentencesList; }

    public bool CanPlayerChoose(int index) { return sentencesList[index].options.Count>0; }

    public bool CanPlayerChoose() { return canPlayerChoose; }
}
