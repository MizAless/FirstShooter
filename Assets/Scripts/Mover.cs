using System.Collections;
using UnityEngine;

public class FirstPersonShooterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float verticalRotation = 0f;
    public Vector3 move;

    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalVelocity = 0f;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        move = (transform.right * horizontalMove + transform.forward * verticalMove).normalized * moveSpeed;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        //Quaternion newRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        //playerCamera.transform.localRotation *= newRotation;

        //float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        //float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        //verticalRotation = -mouseY;
        //verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        //Vector3 newRotation = new Vector3(verticalRotation, 0f, 0f);
        //playerCamera.transform.localEulerAngles += newRotation;

        transform.Rotate(Vector3.up * mouseX);
    }
}