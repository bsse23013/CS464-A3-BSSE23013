using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;
    [SerializeField] GameObject burst;      // a little particle pop

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Coin touched by {other.name}");
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(value);
            if (burst != null) Instantiate(burst, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
