using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float detectDist;
    [SerializeField]
    private float minSpawnTime;
    [SerializeField]
    private Transform posSpawn;
    [SerializeField]
    int maxEnemies = 5;
    [SerializeField]
    private List<GameObject> enemyToSpawn = new List<GameObject>();

    private bool isDestroyed = false;
    private bool isActive = false;
    private bool hasBomb = false;
    private bool canSpawn = true;
    private int enemyCount = 0;
    private AudioSource aSource;

    Transform playerT;
    // Start is called before the first frame update
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {
            DetectPlayer();
        }
    }

    void spawn()
    {

        if (canSpawn && enemyCount < maxEnemies)
        {
            enemyCount++;
            canSpawn = false;
            GameObject choosenEnemy = enemyToSpawn[Random.RandomRange(0,enemyToSpawn.Count)];
            Instantiate(choosenEnemy, posSpawn.position, posSpawn.rotation);
            StartCoroutine(Reload());
        }
    }

    void DetectPlayer()
    {
        float distToPlayer = (playerT.position - transform.position).magnitude;

        if(distToPlayer < detectDist)
        {
            isActive = true;
            spawn();
        }
        else
        {
            isActive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectDist);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(minSpawnTime);
        canSpawn = true;
    }
}
