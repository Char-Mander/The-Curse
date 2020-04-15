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
    private bool idle = false;
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
            Vector3 playerPos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1.4f, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            Vector3 currentPos = new Vector3(this.transform.position.x, this.transform.position.y + viewOffset, this.transform.position.z);
            direToPlayer = playerPos - currentPos;
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
                ManageDecisionStates();
            }

        }

        if(idle) anim.SetFloat("Speed", 0);
        else if (!finalDecision || GetPhase() != 4) anim.SetFloat("Speed", cController.velocity.magnitude);
        else anim.SetFloat("Speed", 0);
    }

    public override void Attack()
    {
        DetectPlayerInArea();
        if(canAttack && GetPhase() != 3)
        {
            if (!spellAttackInCurse)
            {
                spellAttackInCurse = true;
                ChangeLayerWeight(true);
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
                    if (enemy.GetComponent<CursedGirlEnemy>() == null)
                    {

                        Destroy(enemy.GetComponentInParent<Transform>().gameObject);
                    }
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
                end = true;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[2]);
                GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
                StartCoroutine(Transformation());
            }
            AimPlayer();
        }
        else
        {

            if (!end)
            {
                end = true;
                StartAttackingMode();
                AimPlayer();
                StartCoroutine(WaitForDie());
            }
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
        ChangeLayerWeight(true);
        StartCoroutine(WaitForSpawnAttack());
    }

    public override void DetectPlayerInArea()
    {
        if (GetPhase() != 3)
        {
            if (distToPlayer < iniAttackDist && !bodyAttackAnim)
            {
                idle = false;
                base.EnemyMovement(attackSpeed, -transform.forward);
            }
            else if (distToPlayer > endAttackDist)
            {
                idle = false;
                base.EnemyMovement(attackSpeed, transform.forward);
            }
            else idle = true;

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
        if (hit.collider.GetComponentInParent<PlayerController>() == null && !hit.collider.CompareTag("Terrain"))
        {
            Vector3 direVec = hit.normal;
            direVec.y = 0;
            this.transform.rotation = Quaternion.LookRotation(direVec);
            //Mover hacia el player
        }
        else if (hit.collider.GetComponentInParent<PlayerController>() != null && (GetPhase() == 3 && canAttack))
        {
            canAttack = false;
            ChangeLayerWeight(true);
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

    void ChangeLayerWeight(bool isAttacking)
    {
        if (isAttacking)
        {
            anim.SetLayerWeight(0, 0);
            anim.SetLayerWeight(2, 1);
        }
        else
        {
            anim.SetLayerWeight(0, 1);
            anim.SetLayerWeight(2, 0);
        }
    }

    IEnumerator Transformation()
    {
        yield return new WaitForSeconds(1);
        peacefulModel.SetActive(true);
        peacefulModel.transform.position = this.transform.position;
        peacefulModel.transform.rotation = this.transform.rotation;
        Instantiate(goal, goalPos);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    IEnumerator WaitForSpellAttack()
    {
        anim.SetInteger("AttackType", 0);
        yield return new WaitForSeconds(2.15f);
        spellAttackInCurse = false;
        ChangeLayerWeight(false);
        Action();
    }

    IEnumerator WaitForBodyAttack(ControllerColliderHit hit)
    {
        anim.SetInteger("AttackType", 1);
        bodyAttackAnim = true;
        yield return new WaitForSeconds(2f);
        bodyAttackAnim = true;
        ChangeLayerWeight(false);
        Teleport();
        hit.collider.gameObject.GetComponent<Health>().LoseHealth(damage);
        base.ReloadCoroutine();
    }

    IEnumerator WaitForSpawnAttack()
    {
        anim.SetInteger("AttackType", 2);
        yield return new WaitForSeconds(2f);
        ChangeLayerWeight(false);
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
        GetComponent<Health>().SetGodMode(false);
        GetComponent<Health>().LoseHealth(1000);
    }

    public bool HasBeenActivated() { return activation; }

}
