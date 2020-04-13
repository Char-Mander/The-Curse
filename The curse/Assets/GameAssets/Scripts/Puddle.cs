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
    [SerializeField]
    private float offset = 1.5f;

    private ParticleSystem particles;
    private Rigidbody rb;
    private Vector3 dir;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        Vector3 playerPos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + offset, GameObject.FindGameObjectWithTag("Player").transform.position.z); ;
        dir = playerPos - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir * force * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().LoseHealth(damage);
            particles.Play();
        }
        else if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Rocks"))
        {
            Destroy(Instantiate(puddle, this.transform.position, puddle.transform.rotation), 5);
        }
        Destroy(this.gameObject);
    }

}
