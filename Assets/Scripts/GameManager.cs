using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 8, lives = 3;
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] TMP_Text scoreText, livesText, hintText;
    [SerializeField] GameObject winPanel, losePanel;
    [SerializeField] Image damageFlash;

    int score;

    void Awake() => Instance = this;

    void Start()
    {
        hintText.text = "";
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = $"Coins: {score} / {targetScore}";
        livesText.text = $"Lives: {lives}   Hearts: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }

    // the goal only counts once enough coins are in
    public void ReachGoal()
    {
        if (score >= targetScore) Win();
        else StartCoroutine(Hint($"Collect {targetScore - score} more coin(s) first"));
    }

    IEnumerator Hint(string message)
    {
        hintText.text = message;
        yield return new WaitForSeconds(2f);
        hintText.text = "";
    }

    // a quick red wash over the screen when a hit lands
    public void FlashDamage()
    {
        if (damageFlash == null) return;
        StopCoroutine(nameof(Flash));
        StartCoroutine(nameof(Flash));
    }

    IEnumerator Flash()
    {
        Color c = damageFlash.color;
        for (float t = 0f; t < 0.35f; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0.4f, 0f, t / 0.35f);
            damageFlash.color = c;
            yield return null;
        }
        c.a = 0f;
        damageFlash.color = c;
    }

    public void SetCheckpoint(Transform point) => respawnPoint = point;

    public void PlayerDied()
    {
        lives--;
        UpdateUI();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;
        player.Restore(spawnProtection);
    }

    void Win() { winPanel.SetActive(true); Time.timeScale = 0f; }
    void Lose() { losePanel.SetActive(true); Time.timeScale = 0f; }

    public void Restart()
    {
        Time.timeScale = 1f;    // it survives a scene load
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
