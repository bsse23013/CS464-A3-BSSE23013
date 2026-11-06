using System.Collections;
using UnityEngine;

// spikes (restTime 0), the guard (restTime 0) and the crusher (restTime > 0)
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 8f;
    [SerializeField] float knockTime = 0.3f;
    [SerializeField] float restTime = 0f, warnTime = 0.8f, strikeTime = 0.5f;
    [SerializeField] GameObject warning, slab;

    IEnumerator Start()
    {
        if (warning != null) warning.SetActive(false);
        if (slab != null) slab.SetActive(false);
        if (restTime <= 0f) yield break;

        Collider zone = GetComponent<Collider>();
        while (true)
        {
            zone.enabled = false;
            yield return new WaitForSeconds(restTime);
            warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);
            warning.SetActive(false);
            zone.enabled = true;
            slab.SetActive(true);
            yield return new WaitForSeconds(strikeTime);
            slab.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Health health) || !health.TakeDamage(damage)) return;
        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;

        Vector3 away = body.position - transform.position;
        away.y = 0f;
        if (away.sqrMagnitude < 0.01f) away = -body.transform.forward;
        Vector3 push = away.normalized * knockback;
        if (other.TryGetComponent(out PlayerController p))
            p.Knockback(push, knockTime);
    }
}
