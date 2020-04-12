using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollMode : MonoBehaviour
{
    private bool isRagdoll = false;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ApplyRagdoll(isRagdoll);
    }

    public void ApplyRagdoll(bool isActive) {
        anim.enabled = !isActive;

        Rigidbody[] rigis = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigi in rigis) {
            rigi.isKinematic = !isActive;
        }
    }

    public bool IsRagdoll() { return isRagdoll; }

}
