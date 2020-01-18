using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour
{       //TODO revisar por qué solo se puede bajar 1 vez de la montura
    private const float gravity = -9.8f;

    [SerializeField]
    Transform playerMountPos;
    [SerializeField]
    Transform playerGetOffMountPos;
    [SerializeField]
    float mountSpeed;
    [SerializeField]
    float mountJumpForce;
    [SerializeField]
    float mountRotationSpeed;

    private GameObject player;
    private CharacterController cController;
    private Vector3 dirPos;
    private bool isWalking = false;
    private bool iniRotateMouseY = false;
    private float iniMouseY;
    private float pitch;
    private float yaw;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ManageHorseStates();
        JumpAndMoveHorse();
        RotateHorse();
    }

    private void ManageHorseStates()
    {
        if (player.GetComponent<PlayerController>().IsOnAMount())
        {
            if (!isWalking && cController.velocity.magnitude != 0 && cController.isGrounded)
            {
                isWalking = true;
                GetComponentInChildren<PlayerSoundsManager>().ManageMountSound();
            }
            //Cuando pasa de estar andando a estar completamente quieto
            else if (cController.velocity.magnitude == 0 && isWalking)
            {
                isWalking = false;
                GetComponentInChildren<PlayerSoundsManager>().StopSound();
            }
        }
        else if (player.GetComponent<PlayerController>().HasCallTheMount())
        {

        }
    }

    private void JumpAndMoveHorse()
    {
        if (player.GetComponent<PlayerController>().IsOnAMount())
        {
            if (cController.isGrounded)
            {
                dirPos = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isWalking && cController.velocity.magnitude == 0)
                    {
                        isWalking = false;
                        GetComponentInChildren<PlayerSoundsManager>().StopSound();
                    }
                    dirPos.y = mountJumpForce;
                    GetComponentInChildren<PlayerSoundsManager>().ManageJumpSound();
                }

            }
            dirPos.y += gravity * Time.deltaTime;
            cController.Move(dirPos * mountSpeed * Time.deltaTime);
        }
    }

    private void RotateHorse()
    {
        if (player.GetComponent<PlayerController>().IsOnAMount())
        {
            if (iniRotateMouseY)
            {
                pitch -= mountRotationSpeed * Input.GetAxis("Mouse Y");
                yaw += mountRotationSpeed * Input.GetAxis("Mouse X");
                pitch = Mathf.Clamp(pitch, -60, 60);

                Camera.main.transform.localRotation = Quaternion.Euler(pitch + iniMouseY, 0, 0);
                transform.rotation = Quaternion.Euler(0, yaw + mountRotationSpeed, 0);
            }
            else if (Input.GetAxis("Mouse Y") != 0)
            {
                iniMouseY = Input.GetAxis("Mouse Y");
                iniRotateMouseY = true;
            }
            player.GetComponent<PlayerController>().transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, this.transform.localRotation.y, this.transform.localRotation.z);
        }
    }

    public void PlayerClimbsOn()
    {
        player.GetComponent<PlayerController>().SetIsOnAMount(true);
        player.transform.position = new Vector3(playerMountPos.position.x, playerMountPos.position.y, playerMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerMountPos.rotation.x, playerMountPos.rotation.y, playerMountPos.rotation.z);
        player.transform.parent = this.gameObject.transform;
        player.GetComponent<PlayerController>().GetActiveWeaponAndCrossHair();
        player.GetComponent<PlayerController>().EnableWeapon(false);
        GetComponentInChildren<PlayerSoundsManager>().ManageJumpSound();
        player.GetComponent<PlayerController>().EnableOrDisableCharacterController(false);
    }

    public void PlayerGetsOff()
    {
        player.GetComponent<PlayerSoundsManager>().ManageJumpSound();
        player.GetComponent<PlayerController>().SetIsOnAMount(false);
        player.GetComponent<PlayerController>().EnableWeapon(true);
        player.GetComponent<PlayerController>().EnableOrDisableCharacterController(true);
        player.transform.position = new Vector3(playerGetOffMountPos.position.x, playerGetOffMountPos.position.y, playerGetOffMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerGetOffMountPos.rotation.x, playerGetOffMountPos.rotation.y, playerGetOffMountPos.rotation.z);
        player.transform.parent = null;
    }
    
    
}
