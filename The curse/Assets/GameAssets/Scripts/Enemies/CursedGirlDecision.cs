using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlDecision : MonoBehaviour
{
    [SerializeField]
    GameObject goal;
    [SerializeField]
    Transform goalPos;
    bool end = false;
    CursedGirlEnemy cursedGirl;
    CursedGirlStates cursedGirlState;
    // Start is called before the first frame update
    void Start()
    {
        cursedGirl = GetComponent<CursedGirlEnemy>();
        cursedGirlState = cursedGirl.cursedGirlState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cursedGirl.locked && cursedGirl.activation && cursedGirlState == CursedGirlStates.DECISION && !end)
        {
            print("Está en el decision ahora");
            end = true;
            GetComponent<CursedGirlTalk>().SetDialogueMode(true);
            cursedGirl.anim.SetFloat("Speed", 0);
            DestroyEnemies();
            ManageDecisionStates();
        }
    }

    private void DestroyEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {

            if (enemy.GetComponent<CursedGirlEnemy>() == null)
            {
                Destroy(enemy.GetComponentInParent<Transform>().gameObject);
            }
        }
    }

    private void ManageDecisionStates()
    {
        GetComponent<CursedGirlEnemy>().AimPlayer();
        if (FindObjectOfType<DecisionState>().CheckBalanceState() > 0)
        {
                //lanza la cinemática del diálogo
               // FindObjectOfType<DialogueManager>().StartDialogue(dialogues[2]);
            GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
            StartCoroutine(Transformation());
            cursedGirlState = CursedGirlStates.TALKING;
        }
        else
        {
            GetComponent<CursedGirlTalk>().SetDialogueMode(false);
            StartCoroutine(WaitForDie());
            cursedGirl.cursedGirlState = CursedGirlStates.ATTACKING;
        }
    }

    IEnumerator Transformation()
    {
        yield return new WaitForSeconds(1);
        cursedGirl.peacefulModel.SetActive(true);
        cursedGirl.peacefulModel.transform.position = this.transform.position;
        cursedGirl.peacefulModel.transform.rotation = this.transform.rotation;
        Instantiate(goal, goalPos);
        yield return new WaitForSeconds(0.2f);
        GetComponent<CursedGirlTalk>().SetDialogueMode(false);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    IEnumerator WaitForDie()
    {
        Instantiate(goal, goalPos);
        yield return new WaitForSeconds(1f);
        GetComponent<Health>().SetGodMode(false);
        GetComponent<Health>().LoseHealth(1000);
    }
}
