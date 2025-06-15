using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float runSpeed = 40f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float crouchSpeed = 0.5f;
    public float groundDamping = 20f; // NEW: How quickly horizontal movement slows down on the ground
    public float airDamping = 0.5f;   // NEW: How quickly horizontal movement slows down in the air

    private float horizontalInput; // Renamed for clarity
    private Vector3 velocity;
    private bool isJumping;
    private bool isCrouching;
    private float originalHeight;
    private float currentHorizontalVelocity; // NEW: Stores the actual horizontal velocity

    [Space]

    [Header("Animasi Duniawi")]
    [SerializeField] GameObject myMesh;
    [SerializeField] Animator myAnimator;

    [Space]
    [Header("crouching")]

    bool isOnCrouchArea;
    public GameObject originalMesh;

    void Start()
    {
        originalHeight = controller.height;
    }

    void Update()
    {
        // Input horizontal
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Get raw input here

        if (horizontalInput != 0)
        {
            myAnimator.SetBool("isJalan", true);
        }
        else
        {
            myAnimator.SetBool("isJalan", false);
        }

        // Input Jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            myAnimator.SetTrigger("JUMP");
            isJumping = true;
        }

        // Input Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            myAnimator.SetBool("isCrouch", true);
            myAnimator.SetTrigger("IsStartCrouch");
            isCrouching = true;
            Debug.Log("crouch");
            originalMesh.SetActive(false);
            // Adjust center based on the new height
            controller.height = 1;
            controller.center = new Vector3(0, 0, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!isOnCrouchArea)
            {
            originalMesh.SetActive(true);
                DoneCrouch();
            }
        }
    }

    void DoneCrouch()
    {
        // Check if there's enough space to stand up before uncrouching
        // This prevents the player from standing up into an obstacle
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, originalHeight - controller.height, ~LayerMask.GetMask("Player")))
        {
            // If something is above, don't stand up
            return;
        }

        myAnimator.SetBool("isCrouch", false);
        isCrouching = false;
            controller.height = 1.8f;
            controller.center = new Vector3(0, 0.37f, 0);
        // controller.height = originalHeight;
        // controller.center = new Vector3(0, originalHeight / 2f, 0); // Reset center to original height
    }

    void FixedUpdate()
    {
        // Calculate target horizontal velocity based on input
        float targetHorizontalVelocity = horizontalInput * runSpeed * (isCrouching ? .5f : 1);

        // Apply damping to current horizontal velocity
        float dampingFactor = controller.isGrounded ? groundDamping : airDamping;
        currentHorizontalVelocity = Mathf.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, dampingFactor * Time.fixedDeltaTime);

        // Apply rotation based on movement direction
        if (currentHorizontalVelocity > 0.1f) // Use a small threshold to avoid rotation jitter when nearly stopped
        {
            myMesh.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (currentHorizontalVelocity < -0.1f) // Use a small threshold
        {
            myMesh.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        // Pergerakan Horizontal (using currentHorizontalVelocity)
        Vector3 move = transform.right * currentHorizontalVelocity * Time.fixedDeltaTime;

        // Gravitasi
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Sedikit ke bawah untuk memastikan tetap grounded
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // Jump
        if (isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = false;
        }

        // Aplikasikan pergerakan
        controller.Move(move + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("crouchArea")) // Use CompareTag for efficiency
        {
            isOnCrouchArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("crouchArea")) // Use CompareTag for efficiency
        {
            isOnCrouchArea = false;
            DoneCrouch();
        }
    }
}