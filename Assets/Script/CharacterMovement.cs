using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float runSpeed = 40f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float crouchScale = 0.5f;

    private float horizontalMove;
    private Vector3 velocity;
    private bool isJumping;
    private bool isCrouching;
    private float originalHeight;

    [Space]

    [Header("Animasi Duniawi")]
    [SerializeField] GameObject myMesh;
    [SerializeField] Animator myAnimator;

    [Space]
    [Header("crouching")]

    bool isOnCrouchArea;

    void Start()
    {
        originalHeight = controller.height;
    }

    void Update()
    {
        // Input horizontal

        if (horizontalMove != 0)
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
            isCrouching = true;
            controller.height = originalHeight * crouchScale;
            controller.center = new Vector3(0, 0.37f - originalHeight * (1 - crouchScale) / 2f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!isOnCrouchArea)
            {
                DoneCrouch();
            }
        }
    }

    void DoneCrouch()
    {
        myAnimator.SetBool("isCrouch", false);
        isCrouching = false;
        controller.height = originalHeight;
        controller.center = new Vector3(0, 0.37f, 0);
    }

    void FixedUpdate()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (horizontalMove > 0)
        {
            myMesh.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (horizontalMove < 0)
        {
            myMesh.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        // Pergerakan Horizontal
        Vector3 move = transform.right * horizontalMove * Time.fixedDeltaTime;

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
        if (other.gameObject.tag == "crouchArea")
        {
            isOnCrouchArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "crouchArea")
        {
            isOnCrouchArea = false;
            DoneCrouch();
        }
    }
}