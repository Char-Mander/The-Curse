using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    Puzzle puzzle;

    private void Start()
    {
        puzzle = GetComponentInParent<Puzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pickable Object"))
        {
            puzzle.SetActiveObj(puzzle.GetActiveObj() + 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable Object"))
        {
            puzzle.SetActiveObj(puzzle.GetActiveObj() - 1);
        }
    }
}
