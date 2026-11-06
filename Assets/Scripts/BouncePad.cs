using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float power = 10f;   // m/s straight up

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Rigidbody body = other.attachedRigidbody;

        // zero the fall first so the bounce is the same every time
        Vector3 v = body.linearVelocity;
        body.linearVelocity = new Vector3(v.x, 0f, v.z);

        // one kick, mass ignored
        body.AddForce(Vector3.up * power, ForceMode.VelocityChange);
    }
}
