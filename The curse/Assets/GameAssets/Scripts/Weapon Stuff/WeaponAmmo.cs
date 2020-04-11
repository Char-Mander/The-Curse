using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    [SerializeField]
    private int numWeapon;
    [SerializeField]
    private int ammo;

    private void OnTriggerEnter(Collider col)
    {
        print("Nombre: " + col.name + "   tag: " + col.tag);
        if (col.GetComponentInParent<PlayerController>() != null && FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo() < FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetMaxAmmo())
        {
            FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).SetCurrentAmmo(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo() + ammo);
            FindObjectOfType<FixedElementCanvasController>().UpdateBulletPanel(FindObjectOfType<WeaponController>().GetWeaponByIndex(numWeapon).GetCurrentAmmo());
            Destroy(this.gameObject);
        }
    }
}
