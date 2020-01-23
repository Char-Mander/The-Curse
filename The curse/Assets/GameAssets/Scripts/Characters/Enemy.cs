using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public const float gravity = -9.8f;
    public CharacterController cControler;
    public GameObject player;
    public LayerMask lm;
    [HideInInspector]
    public Vector3 direToPlayer;
    private float detectDist;
    [SerializeField]
    private float endAttackDist;
    [SerializeField]
    private float endShootDist;
    [SerializeField]
    private float cadency;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform posDisp;
    private bool isAttacking = false;
    private bool canAttack = true;
    private float distToPlayer;
    // Start is called before the first frame update
    public void Start()
    {
        GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        if (DetectPlayer())
        {
            isAttacking = true;
            Attack();
        }
        else
        {
            isAttacking = false;
            Patrol();
        }
    }

    public bool DetectPlayer()
    {
        direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
        distToPlayer = direToPlayer.magnitude;

        Debug.DrawRay(this.transform.position, direToPlayer.normalized, Color.white);
        //Si está dentro de los límites
        if (distToPlayer <= detectDist)
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

    public void Patrol()
    {
        if (!isAttacking)
        {
            Vector3 auxDir = this.transform.forward;
            auxDir.y += gravity;
            cControler.Move(auxDir * moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        if (!isAttacking)
        {
            this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
        StartCoroutine(Rotate());
    }

    public void Attack()
    {
        if (canAttack)
        {
            Instantiate(projectile, posDisp.position, posDisp.rotation);
            canAttack = false;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(cadency);
        canAttack = true;
    }

    public void AimPlayer()
    {
        Vector3 aux = direToPlayer;
        aux.y = 0;
        transform.rotation = Quaternion.LookRotation(aux, Vector3.up);
        if(weapon!=null) weapon.rotation = Quaternion.LookRotation(direToPlayer);
        Vector3 playerHead = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);
        posDisp.transform.LookAt(playerHead);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);

            //Mover hacia el player
        }
        if (hit.collider.CompareTag("Player"))
        {
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);

           /* //Si el enemigo es una babosa
            if (hit.collider.gameObject.CompareTag("Explosive Sphere"))
            {
                FinalBoss boss = FindObjectOfType<FinalBoss>();
                boss.SetCanSpawn(false);
                float velSpawn = boss.GetSpawnVelocity();
                if (boss.GetPhase() == 2) velSpawn = 2 * velSpawn / 3;
                StartCoroutine(boss.BabosaCoolDown(velSpawn));
            }
            //Para inmolar al enemigo
            GetComponent<Health>().LoseHealth(GetComponent<Health>().GetMaxHealth());*/
        }

        /* Para generar enemigos con la puerta
         * if (hit.collider.CompareTag("Puerta"))
        {
            hit.collider.GetComponent<Rigidbody>().AddForce(hit.moveDirection * 10, ForceMode.Impulse);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectDist);
    }
}
