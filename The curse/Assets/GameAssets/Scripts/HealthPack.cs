using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
	[SerializeField]
	private float ammount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ((other.gameObject.GetComponent<Health>() != null && !other.gameObject.GetComponent<Health>().hasMaxHealth())
            || (other.gameObject.GetComponentInParent<Health>() != null && !other.gameObject.GetComponentInParent<Health>().hasMaxHealth())))
        {
            FindObjectOfType<PlayerController>().soundsManager.ManageObjectPickUp();
            if (other.gameObject.GetComponent<Health>() != null) other.gameObject.GetComponent<Health>().GainHealth(ammount);
            else if (other.gameObject.GetComponentInParent<Health>() != null) other.gameObject.GetComponentInParent<Health>().GainHealth(ammount);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Mount") && FindObjectOfType<PlayerController>().IsOnAMount())
        {
            FindObjectOfType<PlayerController>().soundsManager.ManageObjectPickUp();
            FindObjectOfType<PlayerController>().GetComponent<Health>().GainHealth(ammount);
            Destroy(this.gameObject);
        }
    }
}
