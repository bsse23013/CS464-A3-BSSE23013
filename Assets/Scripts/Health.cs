using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int max = 3;
    [SerializeField] float iFrames = 1f;
    int current;
    float invulnerableUntil;
    Renderer[] visuals;

    public int Current => current;
    public int Max => max;

    void Awake()
    {
        current = max;
        visuals = GetComponentsInChildren<Renderer>();
    }

    // blink while the i-frames run, so you can see you are protected
    void Update()
    {
        bool blinking = Time.time < invulnerableUntil && current > 0;
        bool visible = !blinking || Mathf.FloorToInt(Time.time * 12f) % 2 == 0;
        foreach (Renderer r in visuals) r.enabled = visible;
    }

    public bool TakeDamage(int amount)
    {
        if (current <= 0 || Time.time < invulnerableUntil) return false;
        invulnerableUntil = Time.time + iFrames;
        current -= amount;
        GameManager.Instance.UpdateUI();
        GameManager.Instance.FlashDamage();
        if (current <= 0) GameManager.Instance.PlayerDied();
        return true;
    }

    // falling out of the level, whatever the hearts say
    public void Kill()
    {
        if (current <= 0) return;
        current = 0;
        GameManager.Instance.UpdateUI();
        GameManager.Instance.PlayerDied();
    }

    public void Restore(float protection)
    {
        current = max;
        invulnerableUntil = Time.time + protection;
        GameManager.Instance.UpdateUI();
    }
}
