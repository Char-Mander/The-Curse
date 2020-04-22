using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CursedGirlAttackStates {MOVING, ATTACKING}

public class CursedGirlAttack : MonoBehaviour
{
    CursedGirlAttackStates attackState;
    [SerializeField]
    private float teleportDistance;
    [SerializeField]
    private float teleportCoolDown;
    [SerializeField]
    private float monstersCoolDown;
    [SerializeField]
    List<GameObject> monsters = new List<GameObject>();
    [SerializeField]
    List<Transform> instantiateMonstersPos = new List<Transform>();
    CursedGirlEnemy cursedGirl;
    bool canAttack = true;
    bool attackInCurse = false;
    bool canTeleport = true;
    bool canCreateMonsters = true;

    // Start is called before the first frame update
    void Start()
    {
        cursedGirl = GetComponent<CursedGirlEnemy>();
        attackState = CursedGirlAttackStates.ATTACKING;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!cursedGirl.locked && cursedGirl.activation && cursedGirl.cursedGirlState == CursedGirlStates.ATTACKING)
        {
              cursedGirl.AimPlayer();
              print("Está atacando ahora");
              if (!cursedGirl.enemyCanvas.activeInHierarchy) GetComponent<CursedGirlTalk>().SetDialogueMode(false);
                 
             //if (!canAttack && !attackInCurse) attackState = CursedGirlAttackStates.MOVING;
             //else if (canAttack) attackState = CursedGirlAttackStates.ATTACKING;

            // if (attackState == CursedGirlAttackStates.MOVING)
             //{
                 cursedGirl.DetectPlayerInArea();
             //}
           //else if (attackState == CursedGirlAttackStates.ATTACKING)
           // {
              //  cursedGirl.anim.SetFloat("Speed", 0);
                ManageAttackStates();
           // }
            cursedGirl.anim.SetFloat("Speed", cursedGirl.speed);
            print("Velocity: " + cursedGirl.cController.velocity.magnitude);
        }
        else if (cursedGirl.enemyCanvas.activeInHierarchy) 
        {
            cursedGirl.enemyCanvas.SetActive(false);
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
                Attack();
                SpawnMonster();
                break;
            case 4:
                cursedGirl.cursedGirlState = CursedGirlStates.TALKING;
                break;
        }
    }

    public void Attack()
    {
        if (canAttack && !attackInCurse)
        {
            canAttack = false;
            attackInCurse = true;
            ChangeLayerWeight(true);
            StartCoroutine(WaitForSpellAttack());
        }
    }

    public void SpawnMonster()
    {
        if (!attackInCurse && canCreateMonsters)
        {
            attackInCurse = true;
            CreateMonster();
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
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
            attackInCurse = true;
            ChangeLayerWeight(true);
            StartCoroutine(WaitForBodyAttack(hit));
        }
    }

    private void Teleport()
    {
        if (canTeleport)
        {
            canTeleport = false;
            Vector3 nextPos = new Vector3(cursedGirl.player.transform.position.x + teleportDistance, this.transform.position.y, cursedGirl.player.transform.position.z - teleportDistance);
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

    IEnumerator WaitForSpellAttack()
    {
        cursedGirl.anim.SetInteger("AttackType", 0);
        yield return new WaitForSeconds(2.15f);
        attackInCurse = false;
        ChangeLayerWeight(false);
        cursedGirl.Action();
    }

    IEnumerator WaitForBodyAttack(ControllerColliderHit hit)
    {
        cursedGirl.anim.SetInteger("AttackType", 1);
        yield return new WaitForSeconds(2f);
        attackInCurse = false;
        ChangeLayerWeight(false);
        Teleport();
        hit.collider.gameObject.GetComponent<Health>().LoseHealth(cursedGirl.damage);
        cursedGirl.ReloadCoroutine();
    }

    IEnumerator WaitForSpawnAttack()
    {
        cursedGirl.anim.SetInteger("AttackType", 2);
        yield return new WaitForSeconds(2f);
        attackInCurse = false;
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

    void ChangeLayerWeight(bool isAttacking)
    {
        if (isAttacking)
        {
            cursedGirl.anim.SetLayerWeight(0, 0);
            cursedGirl.anim.SetLayerWeight(2, 1);
        }
        else
        {
            cursedGirl.anim.SetLayerWeight(0, 1);
            cursedGirl.anim.SetLayerWeight(2, 0);
        }
    }

    private int GetPhase()
    {
        int phase = -1;
        if (GetComponent<Health>().GetCurrentHealth() > 2 * GetComponent<Health>().GetMaxHealth() / 3) phase = 1;
        else if (GetComponent<Health>().GetCurrentHealth() > GetComponent<Health>().GetMaxHealth() / 3) phase = 2;
        else if (GetComponent<Health>().GetCurrentHealth() > 0.1f * GetComponent<Health>().GetMaxHealth()) phase = 3;
        else phase = 4;
        return phase;
    }

}
