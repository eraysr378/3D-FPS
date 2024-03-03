using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public PlayerInput.OnFootActions onFoot;

    private PlayerInput playerInput;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerWeapon playerWeapon;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        playerWeapon = GetComponent<PlayerWeapon>();


        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Reload.performed += ctx => playerWeapon.Reload();
        onFoot.Scope.performed += ctx => playerWeapon.EnableDisableScope();


    }




    // Update is called once per frame
    void Update()
    {
        // tell the player motor to move by giving the input
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        HandleCrouchInput();
        HandleSprintInput();
        HandleShootInput();

    }
    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {

    }
    private void HandleShootInput()
    {
        if (playerWeapon.GetWeapon() == null)
        {
            //Debug.Log("Input manager: gun is null");
            return;
        }
        if (onFoot.Shoot.IsInProgress())
        {
            playerWeapon.Shoot();
        }
        else
        {
            playerWeapon.StopShooting();
        }

    }
    private void HandleSprintInput()
    {
        if (onFoot.Movement.ReadValue<Vector2>().y > 0 && !motor.IsSprinting() && onFoot.Sprint.IsInProgress())
        {
            motor.Sprint();
        }
        else if (motor.IsSprinting() && (!onFoot.Sprint.IsInProgress() || onFoot.Movement.ReadValue<Vector2>().y <= 0))
        {
            motor.StopSprint();
        }
    }
    private void HandleCrouchInput()
    {
        if (!motor.IsGrounded())
        {
            return;
        }
        if (!motor.IsCrouching() && onFoot.Crouch.IsInProgress())
        {
            motor.Crouch();
        }
        else if (motor.IsCrouching() && !onFoot.Crouch.IsInProgress())
        {
            motor.StandUp();
        }
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }

}
