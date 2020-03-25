using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedZone : MonoBehaviour
{
    [SerializeField]
    float damagePerFrame;

    private void Start()
    {
        StartCoroutine(WaitForStopDoingDamage());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Health>().ReceiveConstantDamage(damagePerFrame*Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopDoingDamage();
        }
    }

    IEnumerator WaitForStopDoingDamage()
    {
        yield return new WaitForSeconds(4.8f);
        StopDoingDamage();
    }

    private void StopDoingDamage()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Health>().StopReceivingConstantDamage();
    }
}
