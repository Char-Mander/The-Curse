using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    [SerializeField]
    private int numWeapon;
    bool picked = false;

    private void OnTriggerEnter(Collider col)
    {
        if ((col.GetComponentInParent<PlayerController>() != null || col.GetComponent<PlayerController>() !=null) 
            && !FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetIsUnlocked() && !picked)
        {
            picked = true;
            FindObjectOfType<FixedElementCanvasController>().ShowUnlockWeaponPanel(numWeapon+1);
            if (col.GetComponent<PlayerController>() != null)
            {
                col.GetComponent<PlayerController>().soundsManager.ManageObjectPickUp();
                col.GetComponent<WeaponController>().UnlockWeapon(numWeapon);
            }
            else
            {
                col.GetComponentInParent<PlayerController>().soundsManager.ManageObjectPickUp();
                col.GetComponentInParent<WeaponController>().UnlockWeapon(numWeapon);
            }
            Destroy(this.gameObject);
        }
        else if (col.CompareTag("Mount") && FindObjectOfType<PlayerController>().IsOnAMount())
        {
            picked = true;
            FindObjectOfType<FixedElementCanvasController>().ShowUnlockWeaponPanel(numWeapon + 1);
            FindObjectOfType<PlayerController>().soundsManager.ManageObjectPickUp();
            FindObjectOfType<PlayerController>().GetComponent<WeaponController>().UnlockWeapon(numWeapon);
            Destroy(this.gameObject);
        }
    }
}
