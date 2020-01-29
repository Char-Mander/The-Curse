using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string sentence { get; set;}
    public List<string> options = new List<string>();
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
}
