using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMechanism : MonoBehaviour
{
    [SerializeField]
    Puzzle puzzle;

    private void OnTriggerStay(Collider other)
    {
        if (puzzle.HasBeenActivated() && (other.GetComponent<PlayerController>() != null || other.GetComponentInParent<PlayerController>()!=null) && !puzzle.restarted)
        {
            puzzle.dir = puzzle.GetTargetIniPos().position - puzzle.GetTargetObj().transform.position;
            puzzle.distance = puzzle.dir.magnitude;
            puzzle.restarted = true;
            puzzle.reached = false;
            puzzle.SetActivated(true);
            puzzle.PlayDoorCinematic();
        }
    }
}
