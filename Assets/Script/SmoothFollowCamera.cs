using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target; // Target yang akan diikuti (karakter)
    public float smoothTime = 0.3f; // Waktu yang dibutuhkan untuk mencapai target
    public Vector3 offset; // Offset posisi kamera dari target

    private Vector3 currentVelocity;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}