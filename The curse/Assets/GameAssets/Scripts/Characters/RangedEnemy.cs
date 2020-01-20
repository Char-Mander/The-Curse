using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private const float gravity = -9.8f;

    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform posDetect;
    [SerializeField]
    private Transform posDisp;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float detectionDist;
    [SerializeField]
    private float iniShootDist;
    [SerializeField]
    private float endShootDist;
    [SerializeField]
    private bool isRoofShooter;
    [SerializeField]
    private float cadency;
    [SerializeField]
    private LayerMask lm;


    private bool isAttacking = false;
    private GameObject player;
    private CharacterController cController;
    private Vector3 direToPlayer;
    private float distToPlayer;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cController = GetComponent<CharacterController>();
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectPlayer())
        {
            isRoofShooter = false;
            isAttacking = true;
            Atack();
        }
        else
        {
            isAttacking = false;
            Patrol();
        }
    }

    void Patrol()
    {
       EnemyMovement(moveSpeed, transform.forward);
        if (isRoofShooter)
        {
            DetectRoofEdge();
        }
    }

    void DetectRoofEdge()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.posDetect.position, posDetect.forward, out hit, Mathf.Infinity))
        {

            if (hit.collider.gameObject.CompareTag("Terrain"))
            {
                Debug.DrawLine(posDetect.position, posDetect.forward * 20, Color.red);
                this.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
            }
            else
            {
                Debug.DrawLine(posDetect.position, posDetect.forward * 20, Color.green);
            }
        }
    }

    void Atack()
    {

        //Se mueve en función del área donde esté
        DetectPlayerInArea();
        //Que el enemigo esté mirando al player
        AimPlayer();
        //Instancia balas en dirección al player con cadencia
        Shoot();
    }

    void AimPlayer()
    {
        Vector3 aux = direToPlayer;
        aux.y = 0;
        transform.rotation = Quaternion.LookRotation(aux, Vector3.up);
        weapon.rotation = Quaternion.LookRotation(direToPlayer);
        Vector3 playerHead = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);
        posDisp.transform.LookAt(playerHead);
    }

    void Shoot()
    {
        if (canShoot)
        {
            Instantiate(bullet, posDisp.position, posDisp.rotation);
            canShoot = false;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(cadency);
        canShoot = true;
    }

    void DetectPlayerInArea()
    {
        /*if(distToPlayer < iniShootDist)
        {
            EnemyMovement(moveSpeed, -transform.forward);
        }
        else */
        if (distToPlayer > endShootDist)
        {
            EnemyMovement(moveSpeed, transform.forward);
        }
    }

    void EnemyMovement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime);
    }

    bool DetectPlayer()
    {
        direToPlayer = player.transform.position - this.transform.position;
        distToPlayer = direToPlayer.magnitude;

        //Si está dentro de los límites
        if (distToPlayer <= detectionDist)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direToPlayer.normalized, out hit, detectionDist, lm))
            {
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.blue);
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.DrawLine(this.transform.position, hit.transform.position, Color.red);
                    return true;
                }

            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        //Detect
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, detectionDist);
        //Ini área
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, iniShootDist);
        //End área
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, endShootDist);

    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(3,7));
        if (!isAttacking)
        {
            this.transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);
        }
        StartCoroutine(Rotate());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isRoofShooter)
        {
            if(!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
            {
                this.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            }

            /* Para generar enemigos con la puerta
             * 
            if (hit.collider.CompareTag("Puerta"))
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(hit.moveDirection * 10, ForceMode.Impulse);
            }*/
        }
    }

    public bool IsRoofShooter() { return isRoofShooter; }

}
