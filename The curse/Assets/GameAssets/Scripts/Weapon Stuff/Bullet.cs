using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private AudioSource aSource;
    /*[SerializeField]
    private GameObject hitParticle;
    [SerializeField]
    private GameObject bloodParticle;*/

    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }


    void CreateParticleAtPoint(GameObject obj, Vector3 point, Quaternion rotate)
    {
        GameObject go = Instantiate(obj, point, rotate);
        Destroy(go, 2);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //CreateParticleAtPoint(bloodParticle, col.transform.position, Quaternion rotate)
            col.gameObject.GetComponent<Health>().LoseHealth((float)damage);
        }
        else if (col.CompareTag("Enemy"))
        {
            aSource.PlayOneShot(aSource.clip);
            //CreateParticleAtPoint(bloodParticle, col.transform.position, Quaternion rotate)
            col.gameObject.GetComponentInParent<Health>().LoseHealth(damage);
            print("Ha chocado contra un enemigo");
        }
        else
        {
            //CreateParticleAtPoint(hitParticle, col.transform.position, Quaternion rotate)
        }

        //Destroy(Instantiate(hitParticle, transform.position, Quaternion.identity), 3);
        Destroy(this.gameObject);
    }
}
