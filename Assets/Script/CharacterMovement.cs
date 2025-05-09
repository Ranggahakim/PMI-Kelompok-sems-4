using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public float moveSpeed = 5f; // Kecepatan pergerakan
    public float jumpForce = 10f; // Kekuatan lompatan
    public float gravityMultiplier = 2f; // Pengali gravitasi
    public LayerMask groundLayer; // Layer yang dianggap sebagai tanah
    public float horizontalDamping = 0.1f; // Damping untuk pergerakan horizontal

    [Header("Pengaturan Crouch")]
    public float crouchSpeedMultiplier = 0.5f; // Pengali kecepatan saat crouch
    public float crouchHeightMultiplier = 0.5f; // Pengali tinggi saat crouch
    public float standingHeight = 2f;      // Tinggi karakter saat berdiri
    public float crouchCheckDistance = 0.6f; // Jarak untuk memeriksa halangan di atas saat berdiri
    private float currentHeight;
    private bool isCrouching;

    [Header("Pengecekan Tanah")]
    public Transform groundCheck; // Transform untuk posisi pengecekan tanah
    public float groundedCheckRadius = 0.2f;

    private Rigidbody rb; // Komponen Rigidbody
    private bool isGrounded;
    private float currentSpeed;

    [Header("Pengaturan Kamera")]
    public Camera mainCamera; // Referensi ke kamera utama
    public float cameraFollowSpeed = 5f; // Kecepatan kamera mengikuti

    private Vector3 cameraOffset; // Offset awal kamera dari karakter
    private float horizontalVelocity; // Kecepatan horizontal karakter

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Mencegah rotasi
        currentHeight = standingHeight;
        currentSpeed = moveSpeed;

        // Simpan offset awal kamera
        if (mainCamera != null)
        {
            cameraOffset = mainCamera.transform.position - transform.position;
        }
        else
        {
            Debug.LogError("Main Camera belum di-assign di Inspector!");
        }
    }

    void Update()
    {
        // Pengecekan Tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundedCheckRadius, groundLayer);

        // Pergerakan Horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput, 0f, 0f).normalized;
        currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed; // Ternary untuk crouch speed

        // Menggunakan damping untuk pergerakan yang lebih halus
        horizontalVelocity = Mathf.Lerp(horizontalVelocity, moveInput * currentSpeed, 1 - horizontalDamping);
        Vector3 targetVelocity = new Vector3(horizontalVelocity, rb.velocity.y, 0f);
        rb.velocity = targetVelocity;

        // Lompat
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // Menggunakan GetKeyDown
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
        }

        // Crouch
        if (Input.GetKey(KeyCode.LeftControl)) // Menggunakan GetKey
        {
            Crouch();
        }
        else
        {
            StandUp();
        }

        // Gravitasi yang lebih kuat
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
        }

        // Kamera Mengikuti dengan Delay
        if (mainCamera != null)
        {
            Vector3 targetPosition = transform.position + cameraOffset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
        }
    }

    void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            currentHeight = standingHeight * crouchHeightMultiplier;
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
        }
    }

    void StandUp()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.up, out hit, crouchCheckDistance, groundLayer))
        {
            isCrouching = false;
            currentHeight = standingHeight;
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundedCheckRadius);
    }
}