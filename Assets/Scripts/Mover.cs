using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mover : MonoBehaviour {
    public float Health = 100;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float verticalRotation = 0f;
    public Vector3 move;
    public GameObject damageHUD;
    public CharacterController characterController;
    public Camera playerCamera;
    private float verticalVelocity = 0f;
    private bool isGrounded;
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        if (isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
        move = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).normalized * 9f;
        if (isGrounded && Input.GetButtonDown("Jump")) verticalVelocity = Mathf.Sqrt(2f * -2f * -9.81f);
        verticalVelocity += -9.81f * Time.deltaTime;
        move.y = verticalVelocity;
        characterController.Move(move * Time.deltaTime);
        verticalRotation -= Input.GetAxis("Mouse Y") * 2f;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);
    }
    public void TakeDamage(int damage) {
        Health -= damage;
        Color newColor = Color.white;
        newColor.a = (100 - Health) * 0.01f;
        damageHUD.GetComponent<UnityEngine.UI.Image>().color = newColor;
        if (Health <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}