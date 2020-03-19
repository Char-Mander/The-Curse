using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlEnemy : Enemy
{
    [SerializeField]
    private GameObject attackModel;
    [SerializeField]
    private GameObject peacefulModel;
    [SerializeField]
    private float teleportDistance;
    [SerializeField]
    private float teleportCoolDown;
    [SerializeField]
    private float iniAttackDist;
    [SerializeField]
    private float monstersCoolDown;
    [SerializeField]
    GameObject enemyCanvas;
    [SerializeField]
    GameObject interactableCanvas;
    [SerializeField]
    GameObject goal;
    [SerializeField]
    Transform goalPos;
    [SerializeField]
    List<Transform> instantiateMonstersPos = new List<Transform>();
    [SerializeField]
    List<GameObject> monsters = new List<GameObject>();
    [SerializeField]
    List<Dialogue> dialogues = new List<Dialogue>();
    [SerializeField]
    GameObject mechanismObj;

    private int currentPhase = 1;
    private bool hasSpoken = false;
    private bool finalDecision = false;
    private bool canTeleport = true;
    private bool playerCanKillHer = true;
    private bool canCreateMonsters = true;
    private bool end = false;

    public override void Start()
    {
        cController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        patrolSpeed = attackSpeed * 0.75f;
        attackSpeed = attackSpeed * 1.5f;
        attackSpeed = patrolSpeed;
        SetDialogueMode();
    }

    public override void Update()
    {
        if (!locked)
        {
            direToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
            distToPlayer = direToPlayer.magnitude;

            if (distToPlayer < detectDist && !hasSpoken)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
                if (!mechanismObj.GetComponent<BlockMecanism>().GetActivated()) mechanismObj.GetComponent<BlockMecanism>().SetActivated(true);


            }
            else if (hasSpoken && !finalDecision)
            {
                AimPlayer();
                ManageAttackStates();
            }
            else if (finalDecision)
            {
                ManageDecisionStates();
            }

        }
        
    }

    public override void Attack()
    {
        DetectPlayerInArea();
        if(canAttack && GetPhase() != 3)
        {
            Action();
        }
        else if (canCreateMonsters && GetPhase() == 3)
        {
            CreateMonster();
        }
    }
    
    private void ManageAttackStates()
    {
        switch (GetPhase())
        {
            case 1:
                Attack();
                break;
            case 2:
                Attack();
                Teleport();
                break;
            case 3:
                attackSpeed = attackSpeed;
                Attack();
                break;
            case 4:

                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.name != "CursedGirl")
                        Destroy(enemy.gameObject);
                }
                //Suelta el diálogo 
                SetDialogueMode();
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[1]);
                break;
        }
    }

    private void ManageDecisionStates()
    {
        if (FindObjectOfType<DecisionState>().CheckBalanceState() > 0)
        {
            if (!end)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[2]);
                Instantiate(goal, goalPos);
                end = true;
                GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
            }
            AimPlayer();
        }
        else
        {
            StartAttackingMode();
            AimPlayer();
            StartCoroutine(WaitForDie());
        }
    }

    private void Teleport()
    {
        if (canTeleport)
        {
            canTeleport = false;
            Vector3 nextPos = new Vector3(player.transform.position.x + teleportDistance, this.transform.position.y, player.transform.position.z - teleportDistance);
            this.transform.position = nextPos;
            StartCoroutine(WaitForTeleport());
        }
    }

    private void CreateMonster()
    {
        canCreateMonsters = false;
        GameObject monster = Instantiate(monsters[Random.Range(0, monsters.Count)], instantiateMonstersPos[Random.Range(0, instantiateMonstersPos.Count)]);
        monster.transform.parent = null;
        StartCoroutine(MonstersCadency());
    }

    public override void DetectPlayerInArea()
    {
        if (GetPhase() != 3)
        {
            if (distToPlayer < iniAttackDist)
            {
                base.EnemyMovement(attackSpeed, -transform.forward);
            }
            else if (distToPlayer > endAttackDist)
            {
                base.EnemyMovement(attackSpeed, transform.forward);
            }

        }
        else
        {
            if (distToPlayer < iniAttackDist && !canAttack)
            {
                base.EnemyMovement(attackSpeed, -transform.forward);
            }
            else
            {
                base.EnemyMovement(attackSpeed, transform.forward);
            }
        }
        
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
            //Mover hacia el player
        }
        else if (hit.collider.CompareTag("Player") && (GetPhase() == 3 && canAttack))
        {
            Teleport();
            canAttack = false;
            hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
            base.ReloadCoroutine();
        }
    }
    
    private int GetPhase()
    {
        int phase = -1;
        if (GetComponent<Health>().GetCurrentHealth() > 2 * GetComponent<Health>().GetMaxHealth() / 3) phase = 1;
        else if (GetComponent<Health>().GetCurrentHealth() > GetComponent<Health>().GetMaxHealth() / 3) phase = 2;
        else if (GetComponent<Health>().GetCurrentHealth() > 0.06 * GetComponent<Health>().GetMaxHealth()) phase = 3;
        else phase = 4;
        return phase;
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(teleportCoolDown);
        canTeleport = true;
    }

    IEnumerator MonstersCadency()
    {
        yield return new WaitForSeconds(monstersCoolDown);
        canCreateMonsters = true;
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, iniAttackDist);
    }
    

    public void StartAttackingMode()
    {
        if(!hasSpoken) hasSpoken = true;
        GetComponent<Health>().SetGodMode(false);
        enemyCanvas.SetActive(true);
        interactableCanvas.SetActive(false);
        isAttacking = true;
    }

    public void SetDialogueMode()
    {
        GetComponent<Health>().SetGodMode(true);
        enemyCanvas.SetActive(false);
        interactableCanvas.SetActive(true);
        GetComponent<Health>().StopReceivingConstantDamage();
    }

    public void ApplyDecisionState()
    {
        finalDecision = true;
    }

    public bool IsOnFinalDecisionPhase() { return finalDecision; }

    public bool GetHasSpoken() { return hasSpoken; }

    IEnumerator WaitForDie()
    {
        Instantiate(goal, goalPos);
        yield return new WaitForSeconds(1f);
        GetComponent<Health>().LoseHealth(1000);
    }
}
