using UnityEngine;

// one wide trigger under the level: falling off costs a life
public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
            health.Kill();
    }
}
