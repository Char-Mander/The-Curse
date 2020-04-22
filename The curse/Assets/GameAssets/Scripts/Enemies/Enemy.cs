using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { PATROL, ATTACK}
public class Enemy : MonoBehaviour
{
    public const float gravity = -9.8f;
    public EnemyStates state;

    public float detectDist;
    public float endAttackDist;
    public float cadency;
    public int damage;
    public float attackSpeed;
    public float patrolSpeed;
    public float viewOffset;
    public Transform weapon;
    [SerializeField]
    private GameObject projectile;
    public Transform posDisp;
    public float rotationSpeed = 0.5f;
    [HideInInspector]
    public CharacterController cController;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public GameObject player;
    public LayerMask lm;
    [HideInInspector]
    public Vector3 direToPlayer;
    [HideInInspector]
    public bool isAttacking = false;
    //[HideInInspector]
    public bool canAttack = true;
    public bool locked { get; set; }
    [HideInInspector]
    public float distToPlayer;
    [SerializeField]
    private float patrolWaitingTime = 0.5f;
    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();

    private bool canDealDamage = true;
    private bool canRotateToPlayer = true;
    int wpIndex = 0;
    bool isMoving = false;
    Coroutine wpStop;
    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    public virtual void Start()
    {
        state = EnemyStates.PATROL;
        cController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.SetBool("PlayerDetected", false);
        locked = false;
        wpStop = StartCoroutine(WaitOnWP());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!locked)
        {

            if (DetectPlayer())
            {
                if (state == EnemyStates.PATROL)
                {
                    StopCoroutine(wpStop);
                    isMoving = false;
                    agent.speed = 0;
                    state = EnemyStates.ATTACK;
                    anim.SetBool("PlayerDetected", true);
                }
            }
            else
            {
                if (state == EnemyStates.ATTACK)
                {
                    isMoving = false;
                    wpStop = StartCoroutine(WaitOnWP());
                    agent.speed = patrolSpeed;
                    isAttacking = false;
                    state = EnemyStates.PATROL;
                    anim.SetBool("PlayerDetected", false);
                }
            }

            switch (state)
            {
                case EnemyStates.PATROL:
                    anim.SetFloat("Speed", patrolSpeed);
                    Patrol();
                    break;
                case EnemyStates.ATTACK:
                    anim.SetFloat("Speed", attackSpeed);
                    isAttacking = true;
                    Attack();
                    //Que el enemigo esté mirando al player en cuanto lo detecte
                    AimPlayer();
                    break;

            }
        }
    
    }

    public virtual bool DetectPlayer()
    {
        Vector3 playerPos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1.4f, GameObject.FindGameObjectWithTag("Player").transform.position.z);
        Vector3 currentPos = new Vector3(this.transform.position.x, this.transform.position.y + viewOffset, this.transform.position.z);
        direToPlayer = playerPos - currentPos;
        distToPlayer = direToPlayer.magnitude;

        Debug.DrawRay(currentPos, direToPlayer.normalized, Color.white);
        //Si está dentro de los límites
        if (distToPlayer <= detectDist)
        {
            Debug.DrawLine(currentPos, playerPos, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(currentPos, direToPlayer.normalized, out hit, detectDist, lm))
            {
                Debug.DrawLine(currentPos, hit.transform.position, Color.blue);
                if (hit.collider.GetComponentInParent<PlayerController>() != null)
                {
                    Debug.DrawLine(currentPos, hit.transform.position, Color.red);
                    return true;
                }

            }
        }
        return false;
    }

    public void Patrol()
    {
        if (wpIndex < wayPoints.Count)
        {
            if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
            {
                wpStop = StartCoroutine(WaitOnWP());
                isMoving = false;
            }
        }
        else
        {
            wpIndex = 0;
        }
    }

    public virtual void EnemyMovement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime * speed);
    }

    public virtual void Attack()
    {
        //Se mueve en función del área donde esté
        DetectPlayerInArea();
        if (canAttack)
        {
           // if(this.gameObject.GetComponent<SlugEnemy>() != null)
            //Instancia balas en dirección al player con cadencia
            Action();
        }
    }

    public virtual void DetectPlayerInArea()
    {
        if (distToPlayer > endAttackDist)
        {
            EnemyMovement(attackSpeed, transform.forward);
        }
    }

    public void AimPlayer()
    {
        if (canRotateToPlayer)
        {
            canRotateToPlayer = false;
            Vector3 aux = direToPlayer;
            aux.y = 0;
            transform.rotation = Quaternion.LookRotation(aux);
            if(weapon!=null) weapon.rotation = Quaternion.LookRotation(direToPlayer);
            Vector3 playerHead = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);
            if(posDisp != null) posDisp.transform.LookAt(playerHead);
            RotateCoroutine();
        }
    }

    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.GetComponentInParent<PlayerController>() == null && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
        }
        if (hit.collider.GetComponentInParent<PlayerController>() != null && canDealDamage)
        {
            canDealDamage = false;
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
            StartCoroutine(ReloadBodyAttack());
        }
    }

    public virtual void Action()
    {
        if (canAttack || GetComponent<CursedGirlEnemy>() != null)
        {
            canAttack = false;
            anim.SetTrigger("Attack");
            Instantiate(projectile, posDisp.position, posDisp.rotation);
            ReloadCoroutine();
        }
    }

    public virtual void ReloadCoroutine()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        print("Pone el canAttack a true");
        yield return new WaitForSeconds(cadency);
        canAttack = true;
    }

    IEnumerator ReloadBodyAttack()
    {
        yield return new WaitForSeconds(cadency);
        canDealDamage = true;
    }

    IEnumerator RotateAgain()
    {
        yield return new WaitForSeconds(rotationSpeed);
        canRotateToPlayer = true;
    }

    IEnumerator WaitOnWP()
    {
        yield return new WaitForSeconds(patrolWaitingTime);
        agent.SetDestination(wayPoints[wpIndex].position);
        wpIndex++;
        isMoving = true;
    }

    public void RotateCoroutine()
    {
        StartCoroutine(RotateAgain());
    }

    public virtual void OnDrawGizmos()
    {
        //Detect
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, detectDist);
        //End área
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, endAttackDist);
    }

   
}
