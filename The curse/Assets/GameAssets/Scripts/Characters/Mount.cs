using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour, ICharacter
{       

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
    private bool isLocked;
    private bool isUnlocked;
    Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerSoundsManager = player.GetComponent<PlayerSoundsManager>();
        cController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isUnlocked = GameManager.instance.data.LoadUnlockedMount();
    }

    private void Update()
    {
        if (!isLocked)
        {
            CharacterVelocity();
            JumpAndMoveCharacter();
            RotateCharacter();
        }
        anim.SetBool("Locked", isLocked);
        anim.SetFloat("Speed", cController.velocity.magnitude);
    }

    public void CharacterVelocity()
    {
        if (playerController.HasCallTheMount() && !hasSnorted)
        {
            playerSoundsManager.ManageMountRoar();
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
            FindObjectOfType<PlayerController>().gameObject.transform.position = playerMountPos.transform.position;
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
                    anim.SetTrigger("Jump");
                }

            }
            dirPos.y += gravity * Time.deltaTime;
            cController.Move(dirPos * mountSpeed * Time.deltaTime);
        }
        else if (playerController.HasCallTheMount())
        {
            hasSnorted = false;
            this.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y, player.transform.position.z + 2);
            playerController.SetMountWhistleCall(false);
            this.transform.LookAt(player.transform);
            anim.SetTrigger("Call");
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

                FindObjectOfType<PlayerController>().FPSCamera.transform.localRotation = Quaternion.Euler(pitch + iniMouseY, 0, 0);
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
        playerController.FPSCamera = playerController.cameras[1];
        playerController.cameras[1].m_Priority = 11;
        if (FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.PLAYER) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
    }

    public void PlayerGetsOff()
    {
        playerSoundsManager.StopSound();
        playerSoundsManager.ManageJumpSound();
        playerController.SetIsOnAMount(false);
        playerController.EnableWeapon(true);
        playerController.FPSCamera = playerController.cameras[0];
        playerController.cameras[1].m_Priority = 8;
        if(FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.EVERYTHING) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
        playerController.EnableOrDisableCharacterController(true);
        player.transform.position = new Vector3(playerGetOffMountPos.position.x, playerGetOffMountPos.position.y, playerGetOffMountPos.position.z);
        player.transform.rotation = Quaternion.Euler(playerGetOffMountPos.rotation.x, playerGetOffMountPos.rotation.y, playerGetOffMountPos.rotation.z);
        player.transform.parent = null;
        cController.Move(Vector3.zero);
        StartCoroutine(WaitForSnort());
    }

    public bool IsLocked() { return isLocked; }

    public void SetIsLocked(bool value)
    {
        isLocked = value;
    }

    IEnumerator WaitForSnort()
    {
        yield return new WaitForSeconds(1f);
        playerSoundsManager.ManageMountRoar();
    }

    public bool IsUnlocked() { return isUnlocked; }

    public void SetUnlocked(bool value) { isUnlocked = value; }

    private void OnTriggerStay(Collider other)
    {
        print("Choca con: " + other.name + " con tag: " + other.tag);
        if (!FindObjectOfType<PlayerController>().IsOnAMount())
        {
            if (other.CompareTag("Terrain"))
            {
                isLocked = true;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
                isLocked = false;
            }
            else if (other.CompareTag("Rocks"))
            {
                isLocked = true;
                this.transform.position = new Vector3(this.transform.position.x - 1.5f, this.transform.position.y, this.transform.position.z - 1.5f);
                isLocked = false;
            }
        }
    }
    

    /*
    private void OnCollisionEnter(Collision collision)
    {
        print("Choca con: " + collision.collider.name + " con tag: " + collision.collider.tag);
        if (!FindObjectOfType<PlayerController>().IsOnAMount())
        {
            if (collision.gameObject.CompareTag("Terrain"))
            {
                isLocked = true;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
                isLocked = false;
            }
            else if (collision.gameObject.CompareTag("Rocks") || !collision.gameObject.CompareTag("Player"))
            {
                isLocked = true;
                this.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z + 1);
                isLocked = false;
            }
        }
    }*/


}
