using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShoot : MonoBehaviour, IWeapon
{
    //Variables accesibles desde Unity
    [SerializeField]
    private float zoomMaxValue;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float constantDamage = 0.2f;
    [SerializeField]
    private LayerMask lm;
    //Partículas
    [SerializeField]
    private GameObject enemyHitParticle;

    //Variables privadas
    private bool isShooting = false;
    private bool isZooming = false;
    private AudioSource aSource;
    private Fuel fuel;
    private ParticleSystem weaponParticle;


    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
        fuel = GetComponentInChildren<Fuel>();
        weaponParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (isZooming)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomMaxValue, zoomSpeed);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, zoomSpeed * 2);
        }
        ModifyFuel();
    }

    public void Shoot()
    {
        if (isShooting)
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.3f, lm))
            {
                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Enemy Head") || hit.collider.CompareTag("Explosive Sphere"))
                {
                    //Partículas de sangre
                    CreateParticleAtPoint(enemyHitParticle, hit.point, Quaternion.LookRotation(hit.normal), 2);
                }

            }
        }
    }

    public void Zoom(bool isZoom)
    {
        isZooming = isZoom;
    }


    void CreateParticleAtPoint(GameObject obj, Vector3 point, Quaternion rotate, float destructionTime)
    {
        GameObject go = Instantiate(obj, point, rotate);
        Destroy(go, destructionTime);
    }

    void ModifyFuel()
    {
        if (isShooting)
        {
            fuel.LoseFuel(Time.deltaTime * 8);
            //Para que suene bien el efecto del lanzallamas
            if (aSource.time > 0.7f)
            {
                aSource.Stop();
                aSource.Play();
            }
            //Si se queda sin combustible
            if (fuel.hasNoFuel())
            {
                setShooting(false);
            }
        }
        else if (!fuel.hasMaxFuel())
        {
            fuel.GainFuel(Time.deltaTime * 4);
        }
    }

    public void setShooting(bool shoot)
    {
        if (shoot)
        {
            StartSound();
            PlayingParticles();
        }
        else
        {
            StopSound();
            StopParticles();
        }
        isShooting = shoot;
    }

    private void StartSound()
    {
        aSource.Play();
    }

    private void StopSound()
    {
        aSource.Stop();
    }

    private void PlayingParticles()
    {
        weaponParticle.Play();
    }

    private void StopParticles()
    {
        if (weaponParticle.isEmitting) weaponParticle.Stop();
    }

    public bool IsShooting() { return isShooting; }

    private void OnTriggerStay(Collider collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy Head") || collision.gameObject.CompareTag("Explosive Sphere")) && IsShooting())
        {
            float auxConstantDamage = collision.gameObject.CompareTag("Enemy Head") ? constantDamage * 2 : constantDamage;
            if (collision.gameObject.GetComponent<Health>() != null) collision.gameObject.GetComponent<Health>().ReceiveConstantDamage(auxConstantDamage);
            else collision.gameObject.GetComponentInParent<Health>().ReceiveConstantDamage(auxConstantDamage);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy Head") || collision.gameObject.CompareTag("Explosive Sphere"))
        {
            if (collision.gameObject.GetComponent<Health>() != null) collision.gameObject.GetComponent<Health>().StopReceivingConstantDamage();
            else collision.gameObject.GetComponentInParent<Health>().StopReceivingConstantDamage();
        }
    }
}
