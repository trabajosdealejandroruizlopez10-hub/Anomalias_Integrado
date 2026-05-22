using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float acceleration = 8f;
    public float deceleration = 10f;

    public float mouseSensitivity = 0.1f;

    public float bobSpeed = 14f;
    public float bobAmount = 0.05f;

    public Transform cameraHolder;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 currentVelocity;

    private float xRotation;

    private bool isGrounded;

    private Vector3 cameraStartPos;

    private float bobTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        cameraStartPos = cameraHolder.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GroundCheck();
        Move();
        Look();
        HeadBob();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void Move()
    {
        Vector3 targetMove = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

        Vector3 targetVelocity = targetMove * walkSpeed;

        float smooth = targetMove.magnitude > 0 ? acceleration : deceleration;

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, smooth * Time.deltaTime);

        controller.Move(currentVelocity * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HeadBob()
    {
        if (moveInput.magnitude > 0.1f && isGrounded)
        {
            bobTimer += Time.deltaTime * bobSpeed;

            Vector3 newPosition = cameraStartPos;

            newPosition.y += Mathf.Sin(bobTimer) * bobAmount;

            cameraHolder.localPosition = newPosition;
        }
        else
        {
            bobTimer = 0;

            cameraHolder.localPosition = Vector3.Lerp(
                cameraHolder.localPosition,
                cameraStartPos,
                8f * Time.deltaTime
            );
        }
    }
}