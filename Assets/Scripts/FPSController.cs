using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2f;
    public float gravity = 10f;

    public float sensitivity = 2f;
    public float lookXLimit = 80f;

    public float sprintFOV = 80f;
    private float normalFOV;
    public float fovTransitionSpeed = 10f;

    public float crouchHeight = 1f;
    private float normalHeight;
    private bool isCrouching = false;

    float rotationX = 0f;
    public bool canMove = true;

    Vector3 moveDirection = Vector3.zero;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        normalFOV = playerCamera.fieldOfView;
        normalHeight = controller.height;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleMouseLook();
            HandleCrouch();
        }
        else
        {
            // Explicitly stop all movement
            moveDirection = Vector3.zero;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY = speed * Input.GetAxis("Horizontal");

        if (isGrounded)
        {
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        UpdateCameraFOV(isRunning, curSpeedX, curSpeedY);
    }

    private void HandleMouseLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && canMove)
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? normalHeight - crouchHeight : normalHeight;
        }
    }

    private void UpdateCameraFOV(bool isRunning, float curSpeedX, float curSpeedY)
    {
        bool isMoving = curSpeedX != 0 || curSpeedY != 0;
        float targetFOV = (isRunning && isMoving) ? sprintFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
    }
}
