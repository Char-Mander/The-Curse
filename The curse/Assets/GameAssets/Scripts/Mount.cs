using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour
{
    [SerializeField]
    Transform playerMountPos;
    [SerializeField]
    Transform playerGetOffMountPos;
    [SerializeField]
    float mountSpeed;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void PlayerClimbsOn()
    {
        player.GetComponent<PlayerController>().SetIsOnAMount(true);
        player.transform.position = new Vector3(playerMountPos.position.x, playerMountPos.position.y, playerMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerMountPos.rotation.x, playerMountPos.rotation.y, playerMountPos.rotation.z);
        player.transform.parent = this.gameObject.transform;
    }

    public void PlayerGetsOff()
    {
        player.GetComponent<PlayerController>().SetIsOnAMount(true);
        player.transform.position = new Vector3(playerGetOffMountPos.position.x, playerGetOffMountPos.position.y, playerGetOffMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerGetOffMountPos.rotation.x, playerGetOffMountPos.rotation.y, playerGetOffMountPos.rotation.z);
        player.transform.parent = null;
    }

    public float GetMountSpeed() { return mountSpeed; }
    
}
