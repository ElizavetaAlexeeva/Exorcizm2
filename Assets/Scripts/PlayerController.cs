using UnityEngine;

public class OlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool canJump = true;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform playerBody; // Ссылка на тело игрока (родитель камеры)

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Скрываем и фиксируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Гравитация
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшой "прижим" к земле
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // Поворот камеры по вертикали (только у камеры)
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        // Поворот тела игрока по горизонтали
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (canJump && controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }
}

