using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    public static PlayerMotor LocalInstance { get; private set; }

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 8f;

    [SerializeField] private float slideSpeed = 13f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private bool lerpCrouch;
    [SerializeField] private bool isCrouching;
    [SerializeField] private float crouchTimer;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isSliding;
    [SerializeField] private float sprintTimer;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standupHeight = 2f;
    [SerializeField] private bool isWalking;
    [SerializeField] private float speed;
    [SerializeField] private bool canSlide;
    [SerializeField] private Vector3 controllerVelocity;
    [SerializeField] private Vector3 slideDirection;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [SerializeField] private float slideTimer;
    [SerializeField] private TextMeshProUGUI movementText;
    private PlayerWeapon playerWeapon;

    private void Awake()
    {
        


    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        controller = GetComponent<CharacterController>();
        playerWeapon = GetComponent<PlayerWeapon>();
        Weapon.OnShootingStarted += Weapon_OnShootingStarted;
        movementText = GameObject.Find("/Canvas/MovementText").GetComponent<TextMeshProUGUI>();
    }

    private void Weapon_OnShootingStarted(object sender, System.EventArgs e)
    {
        if (!isSliding && isGrounded)
        {
            StopSprint();
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = walkSpeed;
        gravity = -23f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;
        isGrounded = controller.isGrounded;
        HandleSliding();
        HandleCrouch();
        UpdateMovementText();
    }
    private void UpdateMovementText()
    {
        if (!isGrounded)
        {
            movementText.text = "FLOATING";
        }
        else if (isSliding)
        {
            movementText.text = "SLIDING";
        }
        else if (isSprinting)
        {
            movementText.text = "SPRINTING";

        }
        else if (isCrouching)
        {
            movementText.text = "CROUCHING";

        }
        else if (isWalking)
        {
            movementText.text = "WALKING";
        }
        else
        {
            movementText.text = "STANDING STILL";
        }
        movementText.text += ". speed:" + speed.ToString("0.0");
    }
    private void HandleSliding()
    {
        // to be able to slide, the player should sprint for some time
        if (controllerVelocity.magnitude >= sprintSpeed - 1 && !isSliding)
        {
            slideTimer += Time.deltaTime;
        }
        else
        {
            slideTimer = 0;
        }

        if (isGrounded && slideTimer > 1 && isCrouching && !isSliding)
        {
            canSlide = true;
        }
        else
        {
            canSlide = false;
        }
        if (canSlide)
        {
            Slide();
        }
        // if player stands up, cancel sliding
        if (isSliding && !isCrouching)
        {
            speed = walkSpeed;
            isSliding = false;

        }
        // slowdown player gradually when sliding
        if (isSliding && speed > 0)
        {
            if (speed > 8)
            {
                speed -= Time.deltaTime * 10;

            }
            else if (speed > 4)
            {
                speed -= Time.deltaTime * 6;

            }
            else if (speed > 2)
            {
                speed -= Time.deltaTime * 4;
            }
            else
            {
                isSliding = false;
                speed = crouchSpeed;
            }
        }
        // when player is not sliding anymore, make him move with crouch speed
        if (isCrouching && !isSliding)
        {
            speed = crouchSpeed;
        }

    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
            if (isSliding)
            {
                controller.Move(slideDirection * speed * Time.deltaTime);

            }
            else
            {
                controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

            }
        }

        else
        {
            isWalking = false;
            controller.Move(transform.TransformDirection(moveDirection) * speed / 1.1f * Time.deltaTime);

        }
        controllerVelocity = controller.velocity;

        // handle falling
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);



    }
    public float GetSlideTimeFraction()
    {
        return slideTimer;
    }
    public void Slide()
    {
        isSliding = true;
        canSlide = false;
        speed = slideSpeed;
        slideTimer = 0;
        slideDirection = controllerVelocity.normalized;
    }
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
    public void Crouch()
    {
        isCrouching = true;
        lerpCrouch = true;
        isSprinting = false;
    }
    public void StandUp()
    {
        speed = walkSpeed;
        isCrouching = false;
        lerpCrouch = true;
    }
    public void Sprint()
    {
        if (isCrouching || !isWalking || isSliding || playerWeapon.IsShooting())
        {
            isSprinting = false;
            return;
        }

        isSprinting = true;
        speed = sprintSpeed;
    }
    public void StopSprint()
    {
        speed = walkSpeed;
        isSprinting = false;
    }
    public bool IsGrounded()
    {
        return controller.isGrounded;
    }
    public bool IsCrouching()
    {
        return isCrouching;
    }
    public bool IsSprinting()
    {
        return isSprinting;
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleCrouch()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            if (isCrouching && controller.height > crouchHeight)
            {
                controller.height -= Time.deltaTime * 6;

                if (controller.height < crouchHeight)
                {
                    controller.height = crouchHeight;
                    lerpCrouch = false;
                    crouchTimer = 0f;
                }

            }
            else if (!isCrouching && controller.height < standupHeight)
            {
                controller.height += Time.deltaTime * 6;
                if (controller.height > standupHeight)
                {
                    controller.height = standupHeight;
                    lerpCrouch = false;
                    crouchTimer = 0f;
                }
            }

        }
    }
}
