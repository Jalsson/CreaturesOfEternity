using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(PlayerAnimContoller))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{


    #region speed values and multipliers
    private float currentSpeed;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float speedMultiplier = 2;
    [SerializeField]
    private float dashMultiplier = 3;
    [SerializeField]
    private float rotatingSpeed = 10;
    #endregion

    #region Stamina usages

    [SerializeField]
    private float sprintStaminaUsage = 0.5f;
    [SerializeField]
    private float dashStaminaUsage = 50f;

    #endregion

    private GameObject camera;
    private Rigidbody rb;

    Vector2 inputVector;

    Vector3 mouseLocation;
    Vector3 moveLocation;
    Quaternion targetRotation;

    float tempDashMultiplier;
    float tempSpeedMultiplier;

    StaminaController stamina;
    PlayerAnimContoller PlayerAnimContoller;
    CharacterStats characterStats;
    ArmorAndWeaponEquipper armorAndWeaponEquipper;

    EntityInventory inventory;

    private void Start()
    {
        armorAndWeaponEquipper = GetComponent<ArmorAndWeaponEquipper>();
        characterStats = GetComponent<CharacterStats>();
        inventory = GetComponent<EntityInventory>();

        rb = GetComponent<Rigidbody>();
        stamina = GetComponent<StaminaController>();
        PlayerAnimContoller = GetComponent<PlayerAnimContoller>();

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CameraController>().gameObject;

        // setup all the input delegates
        InputManager.S_INSTANCE.Fire1 += PrimaryAttack;
        InputManager.S_INSTANCE.Fire2 += SecondaryAttack;
        InputManager.S_INSTANCE.MouseLocation += PlayerLookAtTarget;
        InputManager.S_INSTANCE.InputVector += MovePlayer;

    }

    /// <summary>
    /// Verifying the identity of the item type and then either triggering animation or activating the crystal
    /// </summary>
    public void PrimaryAttack()
    {
        if (armorAndWeaponEquipper.EquipedWeapons[(int)WeaponSlotEnum.Primary])
        {
            BaseWeaponClass primaryWeapon = armorAndWeaponEquipper.EquipedWeapons[(int)WeaponSlotEnum.Primary].GetComponent<BaseWeaponClass>();

            if (primaryWeapon is WeaponClass)
            {
                PlayerAnimContoller.TriggerAttack = true;
                PlayerAnimContoller.AttackStyle = 1;
            }
            else if (primaryWeapon is CrystalClassThrowable)
            {
                PlayerAnimContoller.TriggerAttack = true;
                PlayerAnimContoller.AttackStyle = 3;
                tempMouseThrowingLocation = mouseLocation;
            }
            else if (primaryWeapon is CrystalClassActivatable)
            {
                PlayerAnimContoller.TriggerAttack = true;
                PlayerAnimContoller.AttackStyle = 5;
            }
        }
    }

    Vector3 tempMouseThrowingLocation;
    /// <summary>
    /// Checking if the crystal is throwable and then activating it. If throwable then it needs the location where it will be throwed.
    /// </summary>
    public void SecondaryAttack()
    {
        if (armorAndWeaponEquipper.EquipedWeapons[(int)WeaponSlotEnum.Secondary] != null)
        {
            BaseWeaponClass secondaryWeapon = armorAndWeaponEquipper.EquipedWeapons[(int)WeaponSlotEnum.Secondary].GetComponent<BaseWeaponClass>();

            if (secondaryWeapon is CrystalClassThrowable)
            {
                PlayerAnimContoller.TriggerAttack = true;
                PlayerAnimContoller.AttackStyle = 2;
                tempMouseThrowingLocation = mouseLocation;
            }
            else if (secondaryWeapon is CrystalClassActivatable)
            {
                PlayerAnimContoller.TriggerAttack = true;
                PlayerAnimContoller.AttackStyle = 4;
            }
        }
    }

    public void ActivateCrystal(int weaponSlot)
    {
        if (armorAndWeaponEquipper.EquipedWeapons[weaponSlot].GetComponent<BaseWeaponClass>() != null)
        {
            BaseWeaponClass secondaryWeapon = armorAndWeaponEquipper.EquipedWeapons[weaponSlot].GetComponent<BaseWeaponClass>();

            if (secondaryWeapon is CrystalClassThrowable)
            {
                CrystalClassThrowable throwableCrystal = (CrystalClassThrowable)secondaryWeapon;
                throwableCrystal.ActivateWeapon(tempMouseThrowingLocation);
            }
            else if (secondaryWeapon is CrystalClassActivatable)
            {
                secondaryWeapon.ActivateWeapon();
            }
        }
    }

    public void PlayerLookAtTarget(Vector3 target)
    {
        mouseLocation = target;
        targetRotation = Quaternion.LookRotation(target - transform.position);
    }

    public void MovePlayer(Vector3 input)
    {

        #region Sprinting
        //if player has stamina, he can sprint faster 

        if (stamina.getCurrenStamina() > 0.49f && Input.GetButton("Sprint"))
            tempSpeedMultiplier = speedMultiplier;
        else
            tempSpeedMultiplier = 1;

        //drain stamina when sprint button is held down
        if (Input.GetButton("Sprint"))
            stamina.useStamina(sprintStaminaUsage);

        #endregion

        #region Dash Ability



        // if player presses jump input and has enough stamina, character will dash by increasing it's movement speed temporarily.
        if (Input.GetButtonDown("Jump") && stamina.getCurrenStamina() > dashStaminaUsage)
            if (inputVector.x != 0 || inputVector.y != 0)
            {
                PlayerAnimContoller.TriggerDash = true;
                stamina.useStamina(dashStaminaUsage);
            }

        //if player is dashing, temp dash speed is set to predifined speed, otherwise its 1. 
        // incase player is sprinting we set it's multiplier to 1 so player is not going too fast
        if (PlayerAnimContoller.TriggerDash)
        {
            tempDashMultiplier = dashMultiplier;
            tempSpeedMultiplier = 1;
        }
        else
            tempDashMultiplier = 1;

        #endregion

        Vector2 tempInputVector = new Vector2(input.x, input.y);
        tempInputVector = Vector2.ClampMagnitude(tempInputVector, 1);

        //setting current input vector if player is not dashing. Íf player is dashing this keeps the velocity same for dash duration
        if (!PlayerAnimContoller.TriggerDash)
            inputVector = tempInputVector;

        // getting camera forward and right vectors
        Vector3 camF = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
        Vector3 camR = new Vector3(camera.transform.right.x, 0, camera.transform.right.z).normalized;

        // calculating movement targetlocation based on input and camera vectors
        Vector3 targetLocation = (camF * inputVector.y + camR * inputVector.x);


        moveLocation = targetLocation;


        // passing input and our target location to Animation controller class
        PlayerAnimContoller.UpdateAnimator(inputVector, targetLocation);


        // moving rigid body to target location and 
        if (!PlayerAnimContoller.P_animator.GetBool("AttackAnimationPlaying"))
        {
            PlayerAnimContoller.P_animator.SetFloat("RunSpeed", characterStats.MovementModfiers.GetValue());
            if (characterStats.MovementModfiers != null)
            {
                MoveRigidBody(moveLocation, targetRotation, tempDashMultiplier * tempSpeedMultiplier * speed * characterStats.MovementModfiers.GetValue());
            }
            else
                characterStats = GetComponent<CharacterStats>();
        }
    }


    /// <summary>
    /// moves rigidbody to given position
    /// </summary>
    /// <param name="position"></param>
    private void MoveRigidBody(Vector3 position, Quaternion targetRotation, float speed)
    {
           
            rb.MovePosition(transform.position + (position * speed) * Time.deltaTime);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotatingSpeed * Time.deltaTime));
        
    }


    /// <summary>
    /// Moves player during attack animation
    /// </summary>
    private void OnAnimatorMove()
    {
        if (PlayerAnimContoller.P_animator.GetBool("AttackAnimationPlaying"))
        {
            rb.MovePosition(PlayerAnimContoller.P_animator.rootPosition);
        }

    }


}
