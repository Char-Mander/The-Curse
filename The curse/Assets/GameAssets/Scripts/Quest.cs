using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    string texto;
    CheckPoint cP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitQuest(string texto, CheckPoint cP)
    {
        this.texto = texto;
        this.cP = cP;
    }
}
