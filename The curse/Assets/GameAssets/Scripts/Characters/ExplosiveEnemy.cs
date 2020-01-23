using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    private const float gravity = -9.8f;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float detectDist;

    /*[SerializeField]
    private GameObject particle;*/
    [SerializeField]
    private LayerMask lm;
   

    private CharacterController cControler;
    private Vector3 direToPlayer;
    private bool isAtacking = false;
    // Start is called before the first frame update
    void Start()
    {
        cControler = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectPlayerInFront())
        {
            isAtacking = true;
        }
        else
        {
            isAtacking = false;
        }
        
        Patrol();
        Atack();
    }

    bool DetectPlayerInFront()
    {
        direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
        float distToPlayer = direToPlayer.magnitude;

        Debug.DrawRay(this.transform.position, direToPlayer.normalized, Color.white);
        //Si está dentro de los límites
        if(distToPlayer <= detectDist)
        {
            Debug.DrawLine(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direToPlayer.normalized, out hit, detectDist, lm))
            {
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.blue);
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.DrawLine(this.transform.position, hit.transform.position, Color.red);
                    return true;
                }

            }
        }

        return false;
    }

    void Patrol()
    {
        if (!isAtacking)
        {
            Vector3 auxDir = this.transform.forward;
            auxDir.y += gravity;
            cControler.Move(auxDir * moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        if (!isAtacking)
        {
            this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
        StartCoroutine(Rotate());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);

            //Mover hacia el player
        }
        if (hit.collider.CompareTag("Player"))
        {
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);

            //Si el enemigo es una babosa
            if (hit.collider.gameObject.CompareTag("Explosive Sphere"))
            {
                FinalBoss boss = FindObjectOfType<FinalBoss>();
                boss.SetCanSpawn(false);
                float velSpawn = boss.GetSpawnVelocity();
                if (boss.GetPhase() == 2) velSpawn = 2 * velSpawn / 3;
                StartCoroutine(boss.BabosaCoolDown(velSpawn));
            }
            //Para inmolar al enemigo
            GetComponent<Health>().LoseHealth(GetComponent<Health>().GetMaxHealth());
        }

        /* Para generar enemigos con la puerta
         * if (hit.collider.CompareTag("Puerta"))
        {
            hit.collider.GetComponent<Rigidbody>().AddForce(hit.moveDirection * 10, ForceMode.Impulse);
        }*/
    }

    void Atack()
    {
        if (isAtacking)
        {
            Vector3 direAux = direToPlayer;
            direAux.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direAux);
            cControler.Move(this.transform.forward * moveSpeed * 2 * Time.deltaTime);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectDist);
    }
}
