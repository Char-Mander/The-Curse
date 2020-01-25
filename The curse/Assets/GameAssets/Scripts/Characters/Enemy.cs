using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public const float gravity = -9.8f;

    [SerializeField]
    private float detectDist;
    [SerializeField]
    private float endAttackDist;
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
    [SerializeField]
    private float rotationSpeed = 0.5f;
    [HideInInspector]
    public CharacterController cController;
    public GameObject player;
    public LayerMask lm;
    [HideInInspector]
    public Vector3 direToPlayer;

    private bool isAttacking = false;
    private bool canAttack = true;
    private bool canRotateToPlayer = true;
    private float distToPlayer;
    // Start is called before the first frame update
    public virtual void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (DetectPlayer())
        {
            isAttacking = true;
            Attack();
            //Que el enemigo esté mirando al player en cuanto lo detecte
            AimPlayer();
        }
        else
        {
            isAttacking = false;
            Patrol();
        }
    }

    public virtual bool DetectPlayer()
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
        EnemyMovement(moveSpeed, transform.forward);
    }

    void EnemyMovement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime);
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
            //Se mueve en función del área donde esté
            DetectPlayerInArea();
            //Instancia balas en dirección al player con cadencia
            Action();
        }
           
        
    }

    void DetectPlayerInArea()
    {
        if (distToPlayer > endAttackDist)
        {
            EnemyMovement(moveSpeed, transform.forward);
        }
    }

    public void AimPlayer()
    {
        if (canRotateToPlayer)
        {

            canRotateToPlayer = false;
            Vector3 aux = direToPlayer;
            aux.y = 0;
            transform.rotation = Quaternion.LookRotation(aux, Vector3.up);
            if(weapon!=null) weapon.rotation = Quaternion.LookRotation(direToPlayer);
            Vector3 playerHead = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);
            posDisp.transform.LookAt(playerHead);
            StartCoroutine(RotateAgain());
        }
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
            //Para inmolar al enemigo
            GetComponent<Health>().LoseHealth(GetComponent<Health>().GetMaxHealth()); 
        }
    }

    public virtual void Action()
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

    IEnumerator RotateAgain()
    {
        yield return new WaitForSeconds(rotationSpeed);
        canRotateToPlayer = true;
    }

    private void OnDrawGizmos()
    {
        //Detect
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, detectDist);
        //End área
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, endAttackDist);
    }

   
}
