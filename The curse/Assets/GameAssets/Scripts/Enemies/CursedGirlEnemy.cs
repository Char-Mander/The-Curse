using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlEnemy : Enemy
{
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
    private bool activation = false;
    private bool finalDecision = false;
    private bool canTeleport = true;
    private bool playerCanKillHer = true;
    private bool canCreateMonsters = true;
    private bool end = false;
    private bool spellAttackInCurse = false;
    private bool bodyAttackAnim = false;

    public override void Start()
    {
        peacefulModel.SetActive(false);
        cController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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
                if (!activation)
                {
                    activation = true;
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
                    if (!mechanismObj.GetComponent<BlockMecanism>().GetActivated()) mechanismObj.GetComponent<BlockMecanism>().SetActivated(true);
                }
            }
            else if (hasSpoken && !finalDecision)
            {
                AimPlayer();
                ManageAttackStates();
            }
            else if (finalDecision)
            {
                print("Activa la decisión final");
                ManageDecisionStates();
            }

        }
        anim.SetFloat("Speed", cController.velocity.magnitude);
    }

    public override void Attack()
    {
        DetectPlayerInArea();
        if(canAttack && GetPhase() != 3)
        {
            if (!spellAttackInCurse)
            {
                spellAttackInCurse = true;
                anim.SetTrigger("Attack");
                StartCoroutine(WaitForSpellAttack());
            }
        }
        else if (canCreateMonsters && GetPhase() == 3)
        {
            CreateMonster();
        }
    }
    
    private void ManageAttackStates()
    {
        print("Phase: " + GetPhase());
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
            print("Balance positivo");
            if (!end)
            {
                print("Entra a la función donde suelta el diálogo");
                end = true;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[2]);
                GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
                StartCoroutine(Transformation());
            }
            AimPlayer();
        }
        else
        {
            print("Balance negativo");
            StartAttackingMode();
            AimPlayer();
            StartCoroutine(WaitForDie());
        }
        GameManager.instance.sceneC.LoadGameOver();
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
        anim.SetTrigger("Attack");
        StartCoroutine(WaitForSpawnAttack());
    }

    public override void DetectPlayerInArea()
    {
        if (GetPhase() != 3)
        {
            if (distToPlayer < iniAttackDist && !bodyAttackAnim)
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
            if (distToPlayer < iniAttackDist && !canAttack && !bodyAttackAnim)
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
            canAttack = false;
            anim.SetTrigger("Attack");
            StartCoroutine(WaitForBodyAttack(hit));
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

    IEnumerator Transformation()
    {
        yield return new WaitForSeconds(1);
        print("Se transforma");
        peacefulModel.SetActive(true);
        Instantiate(goal, goalPos);
        print("En un segundo deja de verse el attackmodel");
        yield return new WaitForSeconds(1);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        //Destroy(this.gameObject,1);
    }

    IEnumerator WaitForSpellAttack()
    {
        anim.SetTrigger("SpellAttack");
        yield return new WaitForSeconds(2.15f);
        spellAttackInCurse = false;
        Action();
    }

    IEnumerator WaitForBodyAttack(ControllerColliderHit hit)
    {
        anim.SetTrigger("BodyAttack");
        bodyAttackAnim = true;
        yield return new WaitForSeconds(2f);
        bodyAttackAnim = true;
        Teleport();
        hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
        base.ReloadCoroutine();
    }

    IEnumerator WaitForSpawnAttack()
    {
        anim.SetTrigger("SpawnAttack");
        yield return new WaitForSeconds(2f);
        GameObject monster = Instantiate(monsters[Random.Range(0, monsters.Count)], instantiateMonstersPos[Random.Range(0, instantiateMonstersPos.Count)]);
        monster.transform.parent = null;
        StartCoroutine(MonstersCadency());
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
        if (!hasSpoken)
        {
            hasSpoken = true;
            print("Entra al startAttackingMode con el hasSpoken a " + hasSpoken);
            // if(!hasSpoken) hasSpoken = true;
            GetComponent<Health>().SetGodMode(false);
            enemyCanvas.SetActive(true);
            isAttacking = true;
        }
    }

    public void SetDialogueMode()
    {
        GetComponent<Health>().SetGodMode(true);
        enemyCanvas.SetActive(false);
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
