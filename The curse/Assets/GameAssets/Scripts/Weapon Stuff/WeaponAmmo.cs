using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    [SerializeField]
    private int numWeapon;
    [SerializeField]
    private int ammo;
    bool picked = false;

    private void OnTriggerEnter(Collider col)
    {
        if ((col.GetComponent<PlayerController>() != null || col.GetComponentInParent<PlayerController>() != null) && !picked)
        {
            if (FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo() < FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetMaxAmmo())
            {
                picked = true;
                FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).SetCurrentAmmo(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo() + ammo);
                FindObjectOfType<FixedElementCanvasController>().UpdateBulletPanel(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo());
                if(col.GetComponent<PlayerController>() != null) col.GetComponent<PlayerController>().soundsManager.ManageAmmoPickUp();
                else if (col.GetComponentInParent<PlayerController>() != null) col.GetComponentInParent<PlayerController>().soundsManager.ManageAmmoPickUp();
                Destroy(this.gameObject);
            }
            else if (col.CompareTag("Mount") && FindObjectOfType<PlayerController>().IsOnAMount())
            {
                picked = true;
                FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).SetCurrentAmmo(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo() + ammo);
                FindObjectOfType<FixedElementCanvasController>().UpdateBulletPanel(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo());
                FindObjectOfType<PlayerController>().soundsManager.ManageAmmoPickUp();
                Destroy(this.gameObject);
            }
        }
    }
}
