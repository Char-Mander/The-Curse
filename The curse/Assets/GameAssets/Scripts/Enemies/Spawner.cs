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
    private float bombTime;
    [SerializeField]
    private Transform posSpawn;
    [SerializeField]
    private GameObject bomb;
    [SerializeField]
    private GameObject explosionParticle;
    [SerializeField]
    private Renderer indicator;
    [SerializeField]
    private List<GameObject> enemyToSpawn = new List<GameObject>();

    private bool isDestroyed = false;
    private bool isActive = false;
    private bool hasBomb = false;
    private bool canSpawn = true;
    private AudioSource aSource;

    Transform playerT;
    // Start is called before the first frame update
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        aSource = GetComponent<AudioSource>();
        indicator.material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {
            DetectPlayer();
        }
        UpdateIndicator();
    }

    void UpdateIndicator()
    {
        if (isDestroyed)
        {
            indicator.material.color = Color.red;
        }
        else if (isActive)
        {
            indicator.material.color = Color.green;
        }
        else
        {
            indicator.material.color = Color.yellow;
        }
    }

    void spawn()
    {

        if (canSpawn)
        {
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

    void DropBomb()
    {
        GameObject goBomb = Instantiate(bomb, playerT.position, playerT.rotation);
        StartCoroutine(DestroySpawner(goBomb));
    }

    IEnumerator DestroySpawner(GameObject goBomb)
    {
        yield return new WaitForSeconds(bombTime);
        aSource.Play();
        Destroy(Instantiate(explosionParticle, goBomb.transform.position, goBomb.transform.rotation), 5);
        isDestroyed = true;
        Destroy(goBomb);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            print("Detecta al player");
            if (Input.GetKeyDown(KeyCode.E) && !hasBomb)
            {
                print("Activa el spawner");
                DropBomb();
                hasBomb = true;
            }
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
