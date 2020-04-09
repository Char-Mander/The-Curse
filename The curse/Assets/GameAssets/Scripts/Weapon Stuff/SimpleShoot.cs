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
    [SerializeField]
    private GameObject enemyHitParticle;
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
    private bool isZooming = false;
    private bool canShoot = true;
    private AudioSource aSource;


    void Start()
    {
        if (posDisp == null)
            posDisp = transform;
        aSource = GetComponent<AudioSource>();
    }

    //TODO Eliminar después de testear
    void Update()
    {
        if (isZooming)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomMaxValue, zoomSpeed);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, zoomSpeed * 2);
        }
        
    }

    public void StartShootAnimation()
    {
        if (canShoot)
        {
            print("Dispara");
            //GetComponent<Animator>().SetTrigger("Fire");
            canShoot = false;
            Shoot();
            StartCoroutine(Reload());
            CasingRelease();
        }
    }

    public void Shoot()
    {
        if (!canShoot)
        {
            print("Dispara");
            aSource.volume = 0.25f;
            aSource.PlayOneShot(shotAudioClips[0]);
            GameObject tempFlash;
            GameObject bullet = Instantiate(bulletPrefab, posDisp.position, posDisp.rotation);
            Vector3 dire = new Vector3();
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
            {
                //Dibuja la línea en el punto de impacto
                dire = hit.point - posDisp.transform.position;
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Partículas de sangre
                    CreateParticleAtPoint(enemyHitParticle, hit.point, false, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                dire = Camera.main.transform.forward * 1000;
            }
            bullet.transform.rotation = Quaternion.LookRotation(dire.normalized);
            bullet.GetComponent<Rigidbody>().AddForce(dire.normalized * shotPower, ForceMode.Impulse);
            Destroy(bullet, 10);
            tempFlash = Instantiate(muzzleFlashPrefab, posDisp.position, posDisp.rotation);
            Destroy(tempFlash, 0.5f);
        }
    }

    void CasingRelease()
    {
        if (!canShoot)
        {
            GameObject casing;
            casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
            casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
            casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
            aSource.volume = 0.4f;
            aSource.clip = shotAudioClips[1];
            aSource.PlayDelayed(0.65f);
            Destroy(casing, 5);
        }
    }

    void CreateParticleAtPoint(GameObject obj, Vector3 point, bool tint, Quaternion rotate)
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
