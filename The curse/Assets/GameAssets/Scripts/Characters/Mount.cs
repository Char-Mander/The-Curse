using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour, ICharacter
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
    [SerializeField]
    float timeAvoidingObstacles;

    GameObject player;
    CharacterController cController;
    PlayerController playerController;
    PlayerSoundsManager playerSoundsManager;
    private Vector3 dirPos;
    private bool isWalking = false;
    private bool iniRotateMouseY = false;
    private float iniMouseY;
    private float pitch;
    private float yaw;
    private bool hasSnorted = false;
    private bool avoidObstacles = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerSoundsManager = player.GetComponent<PlayerSoundsManager>();
        cController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CharacterVelocity();
        JumpAndMoveCharacter();
        RotateCharacter();
    }

    public void CharacterVelocity()
    {
        if (playerController.HasCallTheMount() && !hasSnorted)
        {
            playerSoundsManager.ManageMountSnort();
            hasSnorted = true;
        }

        if (!isWalking && cController.velocity.magnitude != 0 && cController.isGrounded)
        {
            isWalking = true;
            playerSoundsManager.ManageMountSound();
        }
        //Cuando pasa de estar andando a estar completamente quieto
        else if (cController.velocity.magnitude == 0 && isWalking)
        {
            isWalking = false;
            playerSoundsManager.StopSound();
        }
       
    }

    public void JumpAndMoveCharacter()
    {
        if (playerController.IsOnAMount())
        {
            if (cController.isGrounded)
            {
                dirPos = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isWalking && cController.velocity.magnitude == 0)
                    {
                        isWalking = false;
                        playerSoundsManager.StopSound();
                    }
                    dirPos.y = mountJumpForce;
                    playerSoundsManager.ManageJumpSound();
                }

            }
            dirPos.y += gravity * Time.deltaTime;
            cController.Move(dirPos * mountSpeed * Time.deltaTime);
        }
        else if (playerController.HasCallTheMount())
        {
            hasSnorted = false;
            this.transform.position = new Vector3(player.transform.position.x + 4, player.transform.position.y, player.transform.position.z + 4);
            playerController.SetMountWhistleCall(false);
            this.transform.LookAt(player.transform);
        }
    }

    public void RotateCharacter()
    {
        if (playerController.IsOnAMount())
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
            playerController.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, this.transform.localRotation.y, this.transform.localRotation.z);
        }
    }

    public void PlayerClimbsOn()
    {
        playerController.SetIsOnAMount(true);
        player.transform.position = new Vector3(playerMountPos.position.x, playerMountPos.position.y, playerMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerMountPos.rotation.x, playerMountPos.rotation.y, playerMountPos.rotation.z);
        player.transform.parent = this.gameObject.transform;
        playerController.GetActiveWeaponAndCrossHair();
        playerController.EnableWeapon(false);
        playerSoundsManager.StopSound();
        playerSoundsManager.ManageJumpSound();
        playerController.EnableOrDisableCharacterController(false);
    }

    public void PlayerGetsOff()
    {
        playerSoundsManager.StopSound();
        playerSoundsManager.ManageJumpSound();
        playerController.SetIsOnAMount(false);
        playerController.EnableWeapon(true);
        playerController.EnableOrDisableCharacterController(true);
        player.transform.position = new Vector3(playerGetOffMountPos.position.x, playerGetOffMountPos.position.y, playerGetOffMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerGetOffMountPos.rotation.x, playerGetOffMountPos.rotation.y, playerGetOffMountPos.rotation.z);
        player.transform.parent = null;
    }

}
