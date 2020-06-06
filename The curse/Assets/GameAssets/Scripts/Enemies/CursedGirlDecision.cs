using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGirlDecision : MonoBehaviour
{
    [SerializeField]
    GameObject smokeParticles;
    [SerializeField]
    GameObject fireworkParticles;
    [SerializeField]
    List<Transform> posSpawnEndParticles = new List<Transform>();
    bool end = false;
    CursedGirlEnemy cursedGirl;
    bool hasSavedTheGirl;
    // Start is called before the first frame update
    void Start()
    {
        cursedGirl = GetComponent<CursedGirlEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursedGirl.cursedGirlState == CursedGirlStates.DECISION && !end)
        {
                end = true;
                GetComponent<CursedGirlTalk>().SetDialogueMode(true);
                cursedGirl.anim.SetFloat("Speed", 0);
                ManageDecisionStates();
        }
            
    }

   

    private void ManageDecisionStates()
    {
        GetComponent<CursedGirlEnemy>().AimPlayer();
        hasSavedTheGirl = FindObjectOfType<DecisionState>().CheckBalanceState() > 0;
        FindObjectOfType<GeneralSoundManager>().ManageWinSound();
        if (hasSavedTheGirl)
        {
            GameManager.instance.SetDefeatedEnemies(GameManager.instance.GetDefeatedEnemies() + 1);
            StartCoroutine(Transformation());
        }
        else
        {
            GetComponent<CursedGirlTalk>().SetDialogueMode(false);
            StartCoroutine(WaitForDie());
        }
    }

    IEnumerator Transformation()
    {
        if (posSpawnEndParticles.Count > 0) Instantiate(fireworkParticles, posSpawnEndParticles[0]);
        if (posSpawnEndParticles.Count > 1) Instantiate(fireworkParticles, posSpawnEndParticles[1]);
        yield return new WaitForSeconds(1);
        Instantiate(smokeParticles, this.transform);
        cursedGirl.peacefulModel.SetActive(true);
        cursedGirl.peacefulModel.transform.position = this.transform.position;
        cursedGirl.peacefulModel.transform.rotation = this.transform.rotation;
        yield return new WaitForSeconds(0.2f);
        GetComponent<CursedGirlTalk>().SetDialogueMode(false);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        StartCoroutine(WaitForLoadGameOver());
    }

    IEnumerator WaitForDie()
    {
        if(posSpawnEndParticles.Count > 0) Instantiate(fireworkParticles, posSpawnEndParticles[0]);
        if (posSpawnEndParticles.Count > 1) Instantiate(fireworkParticles, posSpawnEndParticles[1]);
        GetComponent<Health>().LoseHealth(1000);
        yield return new WaitForSeconds(4f);
        GetComponent<Health>().SetGodMode(false);
    }

    IEnumerator WaitForLoadGameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.sceneC.LoadGameOver();
    }

    public bool HasEnd() { return end; }
}
