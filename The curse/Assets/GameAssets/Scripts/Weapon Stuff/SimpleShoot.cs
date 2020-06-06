using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour, IWeapon
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject casingPrefab;
    [SerializeField]
    private GameObject muzzleFlashPrefab;
    [SerializeField]
    private Transform posDisp;
    public GameObject enemyHitParticle;
    [SerializeField]
    private Transform casingExitLocation;
    [SerializeField]
    private LayerMask lm;
    [SerializeField]
    private float shotPower = 100f;
    [SerializeField]
    private float zoomMaxValue;
    [SerializeField]
    private float cadency;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private List<AudioClip> shotAudioClips = new List<AudioClip>();
    [SerializeField]
    private Transform target;
    [SerializeField]
    private GameObject zoomEffect;
    private bool isZooming = false;
    private bool canShoot = true;
    private AudioSource aSource;
    [HideInInspector]
    public Vector3 bulletHit = Vector3.zero;
    [HideInInspector]
    public Quaternion impactRotation = Quaternion.Euler(Vector3.zero);

    void Start()
    {
        if (zoomEffect != null) zoomEffect.SetActive(false);
        if (posDisp == null)
            posDisp = transform;
        aSource = GetComponent<AudioSource>();
    }

    //TODO Eliminar después de testear
    void Update()
    {
        if (isZooming)
        {
            FindObjectOfType<PlayerController>().zoom.m_MaxFOV = Mathf.Lerp(FindObjectOfType<PlayerController>().zoom.m_MaxFOV, zoomMaxValue, zoomSpeed);
            if (zoomEffect != null) zoomEffect.SetActive(true);
        }
        else
        {
            FindObjectOfType<PlayerController>().zoom.m_MaxFOV = Mathf.Lerp(FindObjectOfType<PlayerController>().zoom.m_MaxFOV, 60, zoomSpeed * 2);
            if (zoomEffect != null) zoomEffect.SetActive(false);
        }

    }

    public void StartShootAnimation()
    {
        if (canShoot)
        {
            canShoot = false;
            Shoot();
            StartCoroutine(Reload());
            CasingRelease();
        }
    }

    public void Shoot()
    {
            aSource.PlayOneShot(shotAudioClips[0]);
            GameObject tempFlash;
            GameObject bullet = Instantiate(bulletPrefab, posDisp.position, posDisp.rotation);
            Vector3 dire = new Vector3();
            Ray ray = new Ray(FindObjectOfType<PlayerController>().FPSCamera.transform.position, FindObjectOfType<PlayerController>().FPSCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
            {
                //Dibuja la línea en el punto de impacto
                dire = hit.point - posDisp.transform.position;
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Partículas de sangre
                    bulletHit = hit.point;
                    impactRotation = Quaternion.LookRotation(hit.normal);
                    //CreateParticleAtPoint(enemyHitParticle, hit.point, false, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                dire = FindObjectOfType<PlayerController>().FPSCamera.transform.forward * 1000;
            }
            bullet.transform.rotation = Quaternion.LookRotation(dire.normalized);
            bullet.GetComponent<Rigidbody>().AddForce(dire.normalized * shotPower, ForceMode.Impulse);
            Destroy(bullet, 10);
            tempFlash = Instantiate(muzzleFlashPrefab, posDisp.position, posDisp.rotation);
            Destroy(tempFlash, 0.5f);
        FindObjectOfType<WeaponController>().currentWeapon.SetCurrentAmmo(FindObjectOfType<WeaponController>().currentWeapon.GetCurrentAmmo() - 1);
        FindObjectOfType<FixedElementCanvasController>().UpdateBulletPanel(FindObjectOfType<WeaponController>().currentWeapon.GetCurrentAmmo());
    }

    void CasingRelease()
    {
            GameObject casing;
            casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
            casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
            casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
            aSource.clip = shotAudioClips[1];
            aSource.PlayDelayed(0.65f);
            Destroy(casing, 5);
        
    }

    public void CreateParticleAtPoint(GameObject obj, Vector3 point, bool tint, Quaternion rotate)
    {
        GameObject go = Instantiate(obj, point, rotate);
        Destroy(go, 2);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(cadency);
        canShoot = true;
    }

    public void SetCanShoot(bool value)
    {
        canShoot = true;
    }

    public void Zoom(bool isZoom)
    {
        isZooming = isZoom;
    }

}
