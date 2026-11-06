using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.ReachGoal();
    }
}
