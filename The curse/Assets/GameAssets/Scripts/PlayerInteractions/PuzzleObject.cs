using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    Puzzle puzzle;
    bool isTrigger = false;

    private void Start()
    {
        puzzle = GetComponentInParent<Puzzle>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pickable Object") && !isTrigger)
        {
            isTrigger = true;
            puzzle.SetActiveObj(puzzle.GetActiveObj() + 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable Object") && isTrigger)
        {
            isTrigger = false;
            puzzle.SetActiveObj(puzzle.GetActiveObj() - 1);
        }
    }
}
