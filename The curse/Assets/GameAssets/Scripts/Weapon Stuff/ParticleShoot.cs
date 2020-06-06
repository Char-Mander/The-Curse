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
    public GameObject enemyHitParticle;

    //Variables privadas
    private bool isShooting = false;
    private bool isZooming = false;
    private AudioSource aSource;
    private Fuel fuel;
    private ParticleSystem weaponParticle;
    private GameObject lastEnemy;
    [HideInInspector]
    public Vector3 bulletHit = Vector3.zero;
    [HideInInspector]
    public Quaternion impactRotation = Quaternion.Euler(Vector3.zero);


    void Start()
    {
        aSource = GetComponent<AudioSource>();
        fuel = GetComponent<Fuel>();
        weaponParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (isZooming)
        {
            FindObjectOfType<PlayerController>().zoom.m_MaxFOV = Mathf.Lerp(FindObjectOfType<PlayerController>().zoom.m_MaxFOV, zoomMaxValue, zoomSpeed);
        }
        else
        {
            FindObjectOfType<PlayerController>().zoom.m_MaxFOV = Mathf.Lerp(FindObjectOfType<PlayerController>().zoom.m_MaxFOV, 60, zoomSpeed * 2);
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
                if (hit.collider.CompareTag("Enemy"))
                {
                    bulletHit = hit.point;
                    impactRotation = Quaternion.LookRotation(hit.normal);
                    //CreateParticleAtPoint(enemyHitParticle, hit.point, Quaternion.LookRotation(hit.normal), 2);
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
            DisableDamage();
            /*if(lastEnemy != null)
            {
                if (lastEnemy.GetComponent<Health>() != null)
                {
                    lastEnemy.GetComponent<Health>().StopReceivingConstantDamage();
                }
                else if(lastEnemy.GetComponentInParent<Health>() != null)
                {
                    lastEnemy.GetComponentInParent<Health>().StopReceivingConstantDamage();
                }
            }*/
        }
        isShooting = shoot;
    }

    private void StartSound()
    {
        aSource.Play();
    }

    private void StopSound()
    {
        if (aSource != null && aSource.isPlaying)
        {
            aSource.Stop();
        }
    }

    private void PlayingParticles()
    {
        weaponParticle.Play();
    }

    private void StopParticles()
    {
        if (weaponParticle != null && weaponParticle.isEmitting) weaponParticle.Stop();
    }

    public bool IsShooting() { return isShooting; }

    private void OnTriggerStay(Collider collision)
    {
        print("Colisiona con: " + collision.name + " con tag: " + collision.tag);
        if (collision.gameObject.CompareTag("Enemy") && IsShooting())
        {
            CreateParticleAtPoint(enemyHitParticle, bulletHit, impactRotation, 2);
            if (collision.gameObject.GetComponent<Health>() != null && !collision.gameObject.GetComponent<Health>().IsReceivingConstantDamage()) collision.gameObject.GetComponent<Health>().ReceiveConstantDamage(constantDamage);
            else if(!collision.gameObject.GetComponentInParent<Health>().IsReceivingConstantDamage()) collision.gameObject.GetComponentInParent<Health>().ReceiveConstantDamage(constantDamage);
            lastEnemy = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        print("Termina la colisión con: " + collision.name + " con tag: " + collision.tag);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Health>() != null) collision.gameObject.GetComponent<Health>().StopReceivingConstantDamage();
            else collision.gameObject.GetComponentInParent<Health>().StopReceivingConstantDamage();
            
            lastEnemy = null;
        }
    }

    private void DisableDamage()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy e in enemies)
        {
            if (e.GetComponent<Health>().IsReceivingConstantDamage()) e.GetComponent<Health>().StopReceivingConstantDamage();
        }
    }
}
