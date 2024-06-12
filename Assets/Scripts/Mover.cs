using UnityEngine;
using UnityEngine.SceneManagement;
public class Mover : MonoBehaviour {
    public float moveSpeed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float Health = 100;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float verticalRotation = 0f;
    public Vector3 move;
    public GameObject damageHUD;
    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalVelocity = 0f;
    private bool isGrounded;
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        if (isGrounded && verticalVelocity < 0){
            verticalVelocity = -2f;
        }
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        move = (transform.right * horizontalMove + transform.forward * verticalMove).normalized * moveSpeed;
        if (isGrounded && Input.GetButtonDown("Jump")) {
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
        transform.Rotate(Vector3.up * mouseX);
    }
    public void TakeDamage(int damage) {
        Health -= damage;
        Color newColor = damageHUD.GetComponent<UnityEngine.UI.Image>().color;
        newColor.a = (100 - Health) * 0.01f;
        damageHUD.GetComponent<UnityEngine.UI.Image>().color = newColor;
        if (Health <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}