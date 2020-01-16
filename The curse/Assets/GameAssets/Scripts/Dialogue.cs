using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private string name;

    [SerializeField]
    private List<string> sentences;

    public string GetName() { return name; }

    public List<string> GetSentences(){ return sentences; }
}
