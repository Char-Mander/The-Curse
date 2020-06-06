using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private AudioSource aSource;

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
        print("impacta con: " + col.name + " con tag: " + col.tag);
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Health>().LoseHealth((float)damage);
            Destroy(this.gameObject);
        }
        else if (col.CompareTag("Enemy"))
        {
            aSource.PlayOneShot(aSource.clip);
            FindObjectOfType<SimpleShoot>().CreateParticleAtPoint(FindObjectOfType<SimpleShoot>().enemyHitParticle,
                FindObjectOfType<SimpleShoot>().bulletHit, false, FindObjectOfType<SimpleShoot>().impactRotation);
            if (col.gameObject.GetComponent<Health>() != null) col.gameObject.GetComponent<Health>().LoseHealth(damage);
            else if (col.gameObject.GetComponentInParent<Health>() != null) col.gameObject.GetComponentInParent<Health>().LoseHealth(damage);
            Destroy(this.gameObject);
        }
        else if(col.CompareTag("Terrain") || col.CompareTag("Rocks"))
        {
            Destroy(this.gameObject);
        }

    }
}
