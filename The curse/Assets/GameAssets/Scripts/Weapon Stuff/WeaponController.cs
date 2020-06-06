using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmaClass
{
    [SerializeField]
    int maxAmmo;
    int currentAmmo;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private bool isUnlocked;
    
    public void EnableWeapon() { weapon.SetActive(true); }

    public void DisableWeapon() { weapon.SetActive(false); }

    public bool GetIsUnlocked() { return isUnlocked; }

    public void SetIsUnlocked(bool unlock) { isUnlocked = unlock; }

    public GameObject GetWeapon() { return weapon; }

    public int GetMaxAmmo() { return maxAmmo; }

    public int GetCurrentAmmo() { return currentAmmo; }

    public void SetCurrentAmmo(int ammo)
    {
        currentAmmo = ammo;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }
}

public class WeaponController : MonoBehaviour
{
    //Variables accesibles desde Unity
    //Armas disponibles
    [SerializeField]
    private List<ArmaClass> weaponList = new List<ArmaClass>();
    [SerializeField]
    private float changeWeaponTime;
    [HideInInspector]
    public ArmaClass currentWeapon;
    private int currentWeaponIndex;

    //Variales privadas
    private bool canChangeWeapon = true;
    

    void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.instance.data.LoadWeaponsUnlocked(weaponList.Count);
        GameManager.instance.data.LoadWeaponsAmmo(weaponList.Count);
        if (!weaponList[0].GetIsUnlocked()) UnlockWeapon(0);
        Selectweapon(0);
    }


    IEnumerator Reload()
    {
        canChangeWeapon = false;
        yield return new WaitForSeconds(changeWeaponTime);
        canChangeWeapon = true;
    }


    void Update()
    {
        if (canChangeWeapon && !FindObjectOfType<PlayerController>().IsLocked() && !FindObjectOfType<PlayerController>().IsOnAMount())
        {
            DetectWeaponInput();
        }
    }

    void DetectWeaponInput()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (i < 9)
            {
                if (weaponList[i].GetIsUnlocked())
                {
                    if (Input.GetKeyDown((i + 1).ToString()))
                    {
                        Selectweapon(i);
                        StartCoroutine(Reload());
                    }
                }
            }
        }
    }

    public void Selectweapon(int weaponIndex)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].EnableWeapon();
            if (weaponList[i].GetWeapon().GetComponent<ParticleShoot>() != null)
            {
                weaponList[i].GetWeapon().GetComponent<ParticleShoot>().setShooting(false);
            }
            weaponList[i].DisableWeapon();
        }
        FixedElementCanvasController fixC = GameObject.FindGameObjectWithTag("FixedCanvas").GetComponent<FixedElementCanvasController>();
        fixC.EnableOrDisableFuelBar(false);
        weaponList[weaponIndex].EnableWeapon();
        if(weaponList[weaponIndex].GetWeapon().GetComponent<SimpleShoot>() != null)
        {
            weaponList[weaponIndex].GetWeapon().GetComponent<SimpleShoot>().SetCanShoot(true);
        }
        if (weaponList[weaponIndex].GetWeapon().GetComponent<Fuel>())
        {
            fixC.EnableOrDisableFuelBar(true);
        }
        currentWeapon = weaponList[weaponIndex];
        currentWeaponIndex = weaponIndex;
        FindObjectOfType<FixedElementCanvasController>().EnableBulletPanel(currentWeapon.GetWeapon().GetComponent<SimpleShoot>() != null);
        FindObjectOfType<FixedElementCanvasController>().UpdateBulletPanel(currentWeapon.GetCurrentAmmo());
        FindObjectOfType<PlayerSoundsManager>().ManageChangeWeapon();
    }

    public void UnlockWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponList.Count)
        {
            weaponList[weaponIndex].SetIsUnlocked(true);
            //weaponList[weaponIndex].SetCurrentAmmo(weaponList[weaponIndex].GetMaxAmmo());
            GameManager.instance.data.SaveWeaponUnlocked(weaponIndex);
        }
    }

    public int GetWeaponListLenght() { return weaponList.Count; }

    public int GetCurrentWeaponIndex() { return currentWeaponIndex; }

    public void SetWeaponAmmo(int index, int ammo) { weaponList[index].SetCurrentAmmo(ammo); }

    public ArmaClass GetWeaponByIndex(int index) { return weaponList[index]; }

    public bool HasAmmo(int index) { return weaponList[index].GetWeapon().GetComponent<SimpleShoot>() != null; }
}
