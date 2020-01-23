using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedZone : MonoBehaviour
{
    [SerializeField]
    float damagePerFrame;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<Health>().ReceiveConstantDamage(damagePerFrame*Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<Health>().StopReceivingConstantDamage();
        }
    }
}
