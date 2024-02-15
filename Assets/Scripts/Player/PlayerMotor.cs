using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
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

    [SerializeField]  private float slideTimer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

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
        isGrounded = controller.isGrounded;
        if (controllerVelocity.magnitude >= sprintSpeed-1 && !isSliding)
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
        if(isSliding && !isCrouching)
        {
            speed = walkSpeed;
            isSliding = false;
            
        }


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

        if (isCrouching && !isSliding)
        {
            speed = crouchSpeed;
        }
        HandleCrouch();

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
            if(isSliding)
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
        if (isCrouching || !isWalking ||isSliding)
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
