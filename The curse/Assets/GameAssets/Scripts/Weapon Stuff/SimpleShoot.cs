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
    private Transform barrelLocation;
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
        if (barrelLocation == null)
            barrelLocation = transform;
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
            canShoot = false;
            GetComponent<Animator>().SetTrigger("Fire");
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
       aSource.volume = 0.25f;
       aSource.PlayOneShot(shotAudioClips[0]);
       GameObject tempFlash;
       GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
       Vector3 dire = new Vector3();
        if (target != null)
        {
            dire = target.position - barrelLocation.transform.position;
            dire.y += 0.25f;
        }
        else
        {
            dire = barrelLocation.forward;
        }
        
       /* RaycastHit hit;
        if (Physics.Raycast(barrelLocation.transform.position, Camera.main.transform.forward-barrelLocation.transform.position + Camera.main.transform.position, out hit, Mathf.Infinity, lm))
        {
            //Dibuja la línea en el punto de impacto
            dire = hit.point;
            */
            /*if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Enemy Head") || hit.collider.CompareTag("Explosive Sphere"))
            {
                //Partículas de sangre
                CreateParticleAtPoint(enemyHitParticle, hit.point, false, Quaternion.LookRotation(hit.normal));
                float auxDamage = hit.collider.CompareTag("Enemy Head") ? damage * 2 : damage;
                hit.collider.GetComponentInParent<Health>().LoseHealth(auxDamage);
            }
            else
            {
                //Partículas del láser
                CreateParticleAtPoint(endParticle, hit.point, true, Quaternion.identity);
            }*/

       /* }
        else
        {
            dire = Camera.main.transform.forward - barrelLocation.transform.position + Camera.main.transform.position;
        }*/
        bullet.GetComponent<Rigidbody>().AddForce(dire * shotPower);
        Destroy(bullet, 10);
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(tempFlash, 0.5f);
    }

    void CasingRelease()
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

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(cadency);
        canShoot = true;
    }

    public void Zoom(bool isZoom)
    {
        isZooming = isZoom;
    }

    public void SetCanShoot(bool shoot)
    {
        canShoot = shoot;
    }

}
