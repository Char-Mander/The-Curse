using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    [SerializeField]
    private GameObject puddle;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float force;

    private ParticleSystem particles;
    private Rigidbody rb;
    private Vector3 dir;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        dir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir * force * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().LoseHealth(damage);
        }
        else if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(Instantiate(puddle, this.transform.position, puddle.transform.rotation), 5);
        }
        Destroy(this.gameObject);
    }

    /*/void Start()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Destroy(this.gameObject, 10);
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Health>().LoseHealth((float)damage);
        }
        Destroy(Instantiate(hitParticle, transform.position, Quaternion.identity), 3);
        Destroy(this.gameObject);
    }*/
}
