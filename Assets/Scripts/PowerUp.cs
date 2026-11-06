using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float multiplier = 1.6f;
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject burst;
    [SerializeField] float respawnAfter = 4f;   // so a fall can't leave a level without its pickup

    Collider zone;
    Renderer[] visuals;

    void Awake()
    {
        zone = GetComponent<Collider>();
        visuals = GetComponentsInChildren<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // the timer lives on the player, so it keeps running after this pickup is hidden
        other.GetComponent<PlayerController>().BoostSpeed(multiplier, duration);
        if (burst != null) Instantiate(burst, transform.position, Quaternion.identity);
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        Show(false);
        yield return new WaitForSeconds(respawnAfter);
        Show(true);
    }

    void Show(bool on)
    {
        zone.enabled = on;
        foreach (Renderer r in visuals) r.enabled = on;
    }
}
