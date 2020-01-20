using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private const float gravity = -9.8f;

    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private Transform posDisp;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject column;

    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private GameObject explosiveEnemy;

    [SerializeField]
    private Transform puddlePosDisp;
    [SerializeField]
    private GameObject puddleShotObject;

    [SerializeField]
    private float activationDistance;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;


    [SerializeField]
    private float cadency;
    [SerializeField]
    private float puddleCadency;
    [SerializeField]
    private float spawnVelocity;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private LayerMask lm;



    private GameObject player;
    private int currentPhase=1;
    private bool isPlayerAround = false;
    private Vector3 direToPlayer;
    private float distToPlayer;
    private CharacterController cController;
    private bool canShoot = true;
    private bool canSpawn = true;
    private bool canShootPuddle = true;
    private bool exitBlocked = false;

    private void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        // Está quieto hasta que llega el player
        if (isPlayerAround)
        {
            if (!exitBlocked)
            {
                exitBlocked = true;
                column.transform.localRotation = Quaternion.Euler(new Vector3(30.201f, -61.803f, -56.4f));
                column.GetComponent<BoxCollider>().enabled = true;
            }
            currentPhase = CheckPhase();
            Movement();
            AimPlayer();
            Attack();
        }
        else
        {
            CheckIfPlayerIsAround();
        }
    }


    //En cuanto el player entra a la zona en la que el boss le detecta, ya no puede escapar
    void CheckIfPlayerIsAround()
    {
        float currentDistance = Vector3.Distance(player.transform.position, this.transform.position);
        isPlayerAround = (currentDistance <= activationDistance);
    }

    void Movement()
    {
        direToPlayer = player.transform.position - this.transform.position;
        distToPlayer = direToPlayer.magnitude;
        if (distToPlayer > minDistance)
        {
            EnemyMovement(moveSpeed, transform.forward);
        }
    }

    //Función que comprueba la fase actual del boss
    int CheckPhase()
    {
        int fase = -1;
        if(GetComponent<Health>().GetCurrentHealth() > 2 * (GetComponent<Health>().GetMaxHealth() / 3))
        {
            fase = 1;
        }
        else if(GetComponent<Health>().GetCurrentHealth() > GetComponent<Health>().GetMaxHealth() / 3)
        {
            fase = 2;
        }
        else
        {
            fase = 3;
        }
        return fase;
    }

    //Ataca en función de la fase en la que esté
    void Attack()
    {
        
        switch (currentPhase)
        {
            case 1: if(canShoot) Shoot(cadency, false);
                break;
            case 2: if (canShoot) Shoot(cadency, false);
                
                if (!IsExplosiveSphereAlive() && canSpawn)
                {
                    canSpawn = false;
                    GenerateExplosiveEnemy();
                    StartCoroutine(BabosaCoolDown(spawnVelocity));
                }
                break;
            case 3:
                if (canShootPuddle) Shoot(puddleCadency, true);
                if (!IsExplosiveSphereAlive() && canSpawn)
                {
                    canSpawn = false;
                    GenerateExplosiveEnemy();
                    StartCoroutine(BabosaCoolDown(2*spawnVelocity/3));
                }
                break;
            default:
                break;
        }
    }

    //Genera una esfera explosiva a una distancia del boss
    void GenerateExplosiveEnemy()
    {
        Vector3 newExplosiveEnemyPos = this.transform.position;
        newExplosiveEnemyPos.x += 2;
        newExplosiveEnemyPos.y += 2;

        Instantiate(explosiveEnemy, this.transform.position, this.transform.rotation);
    }

    void Shoot(float cadency, bool radioactive)
    {
            direToPlayer = player.transform.position - this.transform.position;
            distToPlayer = direToPlayer.magnitude;

        if (!radioactive)
        {

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direToPlayer.normalized, out hit, maxDistance, lm))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Instantiate(bullet, posDisp.position, posDisp.rotation);
                    canShoot = false;
                    StartCoroutine(Reload(cadency));
                }

            }
        }
        else
        {
            canShootPuddle = false;
            Destroy(Instantiate(puddleShotObject, puddlePosDisp.position, puddlePosDisp.rotation), 7);
            StartCoroutine(ReloadPuddle(puddleCadency));
        }
    }

    IEnumerator Reload(float cadency)
    {
        yield return new WaitForSeconds(cadency);
        canShoot = true;
    }

    IEnumerator ReloadPuddle(float cadency)
    {
        yield return new WaitForSeconds(cadency);
        canShootPuddle = true;
    }

    public IEnumerator BabosaCoolDown(float velocity)
    {
        yield return new WaitForSeconds(velocity);
        canSpawn = true;
    }

    bool IsExplosiveSphereAlive()
    {
        return GameObject.FindGameObjectWithTag("Explosive Sphere") != null;
    }

    void EnemyMovement(float speed, Vector3 dire)
    {
        Vector3 auxDir = dire;
        auxDir.y += gravity;
        cController.Move(auxDir * Time.deltaTime);
    }

    void AimPlayer()
    {
        Vector3 aux = direToPlayer;
        aux.y = 0;
        transform.rotation = Quaternion.LookRotation(aux, Vector3.up);
        weapon.rotation = Quaternion.LookRotation(direToPlayer);
        Vector3 playerHead = new Vector3(player.transform.position.x, player.transform.position.y + 0.25f, player.transform.position.z);
        posDisp.transform.LookAt(playerHead);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, minDistance);
        //Ini área
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, maxDistance);
        //End área
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, activationDistance);
    }

    //Getters y setters necesarios

    public void SetCanSpawn(bool can)
    {
        canSpawn = can;
    }

    public float GetSpawnVelocity() { return spawnVelocity; }

    public int GetPhase() { return currentPhase; }

    public GameObject GetGoal() { return goal; }
}
