using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    private const float gravity = -9.8f;

    //Variables de movimiento del player
    //Variables accesibles desde Unity
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpForce;
    public GameObject FPSCamera;


    //Variables privadas
    private CharacterController cController;
    private Vector3 dirPos;
    private float moveSpeed;
    private float pitch;
    private float yaw;
    private float iniRotationY;
    private float iniMouseY;
    private bool iniRotateMouseY;
    //Stamina
    private bool isRunning = false;
    private bool canRunAgain = true;
    private bool isWalking = false;
    private bool isOnAMount = false;
    private bool canWhistleAgain = true;
    private bool mountWhistleCall = false;
    private bool locked = false;
    private Stamina stamina;

    private PlayerSoundsManager soundsManager;
    private PickUpObjects pickobj;
    private InteractWithObjects interactobj;
    private GameObject weapon;
    private GameObject crossHair;
    private Mount mount;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        iniRotateMouseY = false;
        cController = GetComponent<CharacterController>();
        stamina = GetComponent<Stamina>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        iniRotationY = transform.rotation.eulerAngles.y;
        soundsManager = GetComponent<PlayerSoundsManager>();
        pickobj = GetComponent<PickUpObjects>();
        interactobj = GetComponent<InteractWithObjects>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (!isOnAMount)
            {
                CharacterVelocity();
                JumpAndMoveCharacter();
                RotateCharacter();
            }
            ModifyStamina();
            WeaponInputs();
            InteractionWithObjects();
            CallTheMount();
            anim.SetFloat("Speed", cController.velocity.magnitude);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    //Función que gestiona cualquier interacción con objetos
    private void InteractionWithObjects()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pickobj.PickOrDropObject();
            interactobj.InteractWithGameObject();
        }
    }

    private void CallTheMount()
    {
        if (Input.GetKeyDown(KeyCode.X) && canWhistleAgain && !FindObjectOfType<CursedGirlEnemy>().activation && FindObjectOfType<Mount>().IsUnlocked())
        {
            soundsManager.ManageWhistleSound();
            mountWhistleCall = true;
            StartCoroutine(WhistleAgain());
        }
    }

    //Función que hace que el player se mueva y salte con las ArrowKeys
    public void JumpAndMoveCharacter()
    {
        if (cController.isGrounded)
        {
            dirPos = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if ((isRunning || isWalking) && cController.velocity.magnitude == 0)
                {
                    isRunning = false;
                    isWalking = false;
                    soundsManager.StopSound();
                }
                dirPos.y = jumpForce;
                soundsManager.ManageJumpSound();
            }

        }
        dirPos.y += gravity * Time.deltaTime;
        cController.Move(dirPos * moveSpeed * Time.deltaTime);
    }

    //Función que controla la velocidad del player al moverse
    public void CharacterVelocity()
    {
        ManagePlayerStates();
        moveSpeed = isRunning ? runSpeed : walkSpeed;
    }

    //Función que hace que el player y la cámara roten con el ratón
    public void RotateCharacter()
    {
        if (iniRotateMouseY)
        {
            pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
            yaw += rotationSpeed * Input.GetAxis("Mouse X");

            pitch = Mathf.Clamp(pitch, -60, 60);

            FPSCamera.transform.localRotation = Quaternion.Euler(pitch + iniMouseY, 0, 0);
            transform.rotation = Quaternion.Euler(0, yaw + iniRotationY, 0);
        }
        else if (Input.GetAxis("Mouse Y") != 0)
        {
            iniMouseY = Input.GetAxis("Mouse Y");
            iniRotateMouseY = true;
        }
    }

    ///Función que hace que el player dispare
    private void WeaponInputs()
    {
        
        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.T))
        {
            if (checkTypeOfActiveWeapon() == 1 && FindObjectOfType<WeaponController>().currentWeapon.GetCurrentAmmo() > 0) 
            {
                transform.GetComponentInChildren<SimpleShoot>().StartShootAnimation();
            }
            else if (checkTypeOfActiveWeapon() == 2)
            {
                if (!transform.GetComponentInChildren<ParticleShoot>().IsShooting())
                    transform.GetComponentInChildren<ParticleShoot>().setShooting(true);
                transform.GetComponentInChildren<ParticleShoot>().Shoot();
            }
            else if(FindObjectOfType<WeaponController>().currentWeapon.GetCurrentAmmo() == 0)
            {
                FindObjectOfType<FixedElementCanvasController>().ShowOutOfAmmoPanel();
            }
        }
        else if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.T))
        {
            if (checkTypeOfActiveWeapon() == 2)
            {
                if (transform.GetComponentInChildren<ParticleShoot>().IsShooting())
                    transform.GetComponentInChildren<ParticleShoot>().setShooting(false);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (checkTypeOfActiveWeapon() == 1)
            {
                transform.GetComponentInChildren<SimpleShoot>().Zoom(true);
            }
            else if (checkTypeOfActiveWeapon() == 2)
            {
                transform.GetComponentInChildren<ParticleShoot>().Zoom(true);
            }
        }
        else
        {
            if (checkTypeOfActiveWeapon() == 1)
            {
                transform.GetComponentInChildren<SimpleShoot>().Zoom(false);
            }
            else if (checkTypeOfActiveWeapon() == 2)
            {
                transform.GetComponentInChildren<ParticleShoot>().Zoom(false);
            }
        }
    }


    //Funciones que modifican la stamina del player
    void ModifyStamina()
    {
        if (isRunning && !isOnAMount)
        {
            stamina.LoseStamina(Time.deltaTime * 10);
            if (stamina.hasNoStamina())
            {
                isRunning = false;
                canRunAgain = false;
                StartCoroutine(RegenerationWaitTime());
            }
        }
        else if (!stamina.hasMaxStamina())
        {
            stamina.GainStamina(Time.deltaTime * 5);
        }
    }

    IEnumerator RegenerationWaitTime()
    {
        yield return new WaitForSeconds(2f);
        canRunAgain = true;
    }

    void ManagePlayerStates()
    {
            //Cuando empieza a correr
            if (cController.velocity.magnitude != 0 && Input.GetKey(KeyCode.LeftShift) && !stamina.hasNoStamina() && !isRunning && cController.isGrounded && canRunAgain)
            {
                isRunning = true;
                soundsManager.ManageRunSound();
            }

            //Cuando para de correr
            if (((isRunning && Input.GetKeyUp(KeyCode.LeftShift)) || stamina.hasNoStamina()))
            {
                //Si pasa de correr a andar
                if (isRunning && cController.velocity.magnitude != 0 && !isWalking && cController.isGrounded)
                {
                    isWalking = true;
                    soundsManager.ManageWalkSound();
                }
                else if (cController.velocity.magnitude != 0)
                {
                    isWalking = false;
                    soundsManager.StopSound();
                }
                isRunning = false;
            }
            else
            {
                //Cuando pasa de estar quieto a estar andando
                if (!isWalking && cController.velocity.magnitude != 0 && cController.isGrounded)
                {
                    isWalking = true;
                    soundsManager.ManageWalkSound();
                }
                //Cuando pasa de estar andando a estar completamente quieto
                else if (cController.velocity.magnitude == 0 && isWalking)
                {
                    isWalking = false;
                    isRunning = false;
                    soundsManager.StopSound();
                }
            }
        
            
    }

    int checkTypeOfActiveWeapon()
    {
         int res = 0;

         if (transform.GetComponentInChildren<SimpleShoot>()) res = 1;
         else if (transform.GetComponentInChildren<ParticleShoot>()) res = 2;
         return res;
    }

    IEnumerator WhistleAgain()
    {
        canWhistleAgain = false;
        yield return new WaitForSeconds(2);
        canWhistleAgain = true;
    }

    //Función que desactiva o activa el arma y la mirilla
    public void EnableWeapon(bool enable)
    {
        weapon.SetActive(enable);
        anim.SetBool("Locked", !enable);
        if (enable && weapon.GetComponent<SimpleShoot>())
        {
            weapon.GetComponent<SimpleShoot>().SetCanShoot(true);
        }
        crossHair.SetActive(enable);
    }

    public bool CanRunAgain() { return this.canRunAgain; }
    
    public bool IsOnAMount() { return isOnAMount; }

    public void SetIsOnAMount(bool mount)
    {
        anim.SetBool("OnAMount", mount);
        isOnAMount = mount;
    }

    public void GetActiveWeaponAndCrossHair()
    {
        this.weapon = checkTypeOfActiveWeapon()==1 ? GetComponentInChildren<SimpleShoot>().gameObject 
                                                   : GetComponentInChildren<ParticleShoot>().gameObject;
        this.crossHair = GameObject.FindGameObjectWithTag("CrossHair");
    }
    
    public void EnableOrDisableCharacterController(bool enable)
    {
        print("entra al EnableOrDisableCharacterController del player con el enabled a " + enable);
        cController.enabled = enable;
    }

    public bool HasCallTheMount() { return mountWhistleCall; }

    public void SetMountWhistleCall(bool isActive)
    {
        mountWhistleCall = isActive;
    }

    public bool IsLocked() { return locked; }

    public void SetIsLocked(bool value)
    {
        print("entra al setisLocked del player con el locked a " + value);
        locked = value;
        if (locked && !isOnAMount)
        {
          //  if (!locked)
            //{
                GetActiveWeaponAndCrossHair();
                soundsManager.StopSound();
            //}
        }
        if (!isOnAMount)
        {
           EnableWeapon(!value);
            if (!value)
            {
                anim.SetLayerWeight(0, 1);
                anim.SetLayerWeight(1, 0);
                anim.SetLayerWeight(2, 0);
            }
            else
            {
                anim.SetLayerWeight(0, 0);
                anim.SetLayerWeight(1, 0);
                anim.SetLayerWeight(2, 1);
            }
        }
    }
}
