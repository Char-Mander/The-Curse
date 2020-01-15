using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Variables accesibles desde Unity
    [SerializeField]
    private float cadencia;
    [SerializeField]
    private GameObject posDisp;
    [SerializeField]
    private Color laserColor;
    [SerializeField]
    private float zoomMaxValue;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private LayerMask lm;
    [SerializeField]
    private int damage;

    //Partículas
    [SerializeField]
    private GameObject endParticle;
    [SerializeField]
    private GameObject enemyHitParticle;

    //Variables privadas
    private bool canShoot = true;
    private bool isZooming = false;
    private LineRenderer lineRend;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        aSource = GetComponent<AudioSource>();
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
    }

    void Init()
    {
        canShoot = true;
        isZooming = false;
    }

    public void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;

            //Sonido
            aSource.Play();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, lm))
            {
                //Dibuja la línea en el punto de impacto
                StartCoroutine(DrawLineLaser(hit.point));

                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Enemy Head") || hit.collider.CompareTag("Explosive Sphere"))
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
                }

            }
            else
            {
                StartCoroutine(DrawLineLaser(Camera.main.transform.position + Camera.main.transform.forward * 100));
            }

            //Cadencia
            StartCoroutine(CadencyTime());
        }
    }

    public void Zoom(bool isZoom)
    {
        isZooming = isZoom;
    }


    IEnumerator DrawLineLaser(Vector3 endPos)
    {
        lineRend.SetPosition(0, posDisp.transform.position);
        lineRend.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.02f);
        lineRend.SetPosition(0, posDisp.transform.position);
        lineRend.SetPosition(1, posDisp.transform.position);
    }

    //Corutina que hace que el player tenga que esperar entre disparo y disparo
    IEnumerator CadencyTime()
    {
        yield return new WaitForSeconds(cadencia);
        canShoot = true;
    }

    void CreateParticleAtPoint(GameObject obj, Vector3 point, bool tint, Quaternion rotate)
    {
        GameObject go = Instantiate(obj, point, rotate);
        if (tint)
        {
            go.GetComponent<ParticleSystem>().startColor = laserColor;
        }

        Destroy(go, 2);
    }
}
