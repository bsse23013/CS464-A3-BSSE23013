using UnityEngine;

public enum PatrolMode { Loop, PingPong }

public class EnemyPatrol : MonoBehaviour
{
    enum State { Patrol, Chase, Return }

    [SerializeField] Transform[] points;
    [SerializeField] PatrolMode mode = PatrolMode.Loop;
    [SerializeField] float patrolSpeed = 2f, chaseSpeed = 3.5f, turnSpeed = 360f;
    [SerializeField] float waitTime = 0f, arriveDistance = 0.1f;
    [SerializeField] float chaseRange = 6f, giveUpRange = 10f;

    State state = State.Patrol;
    Transform player;
    int index, step = 1;
    float waitUntil;

    void Start() => player = FindAnyObjectByType<PlayerController>().transform;

    void Update()
    {
        switch (state)
        {
            case State.Patrol:
                if (InRange(chaseRange)) { state = State.Chase; break; }
                Patrol();
                break;

            case State.Chase:
                // seen inside one range, lost outside a longer one
                if (!InRange(giveUpRange)) { state = State.Return; break; }
                MoveTo(new Vector3(player.position.x, transform.position.y, player.position.z), chaseSpeed);
                break;

            case State.Return:
                if (InRange(chaseRange)) { state = State.Chase; break; }
                Vector3 back = points[index].position;
                MoveTo(back, patrolSpeed);
                if (Arrived(back)) state = State.Patrol;
                break;
        }
    }

    void Patrol()
    {
        if (Time.time < waitUntil) return;
        Vector3 target = points[index].position;
        MoveTo(target, patrolSpeed);
        if (Arrived(target))
        {
            index = NextIndex();
            waitUntil = Time.time + waitTime;
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        Vector3 toTarget = target - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (toTarget != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(toTarget), turnSpeed * Time.deltaTime);
    }

    // arrival by distance, not ==
    bool Arrived(Vector3 target) =>
        Vector3.Distance(transform.position, target) < arriveDistance;

    bool InRange(float range) =>
        Vector3.Distance(transform.position, player.position) < range;

    int NextIndex()
    {
        if (mode == PatrolMode.Loop) return (index + 1) % points.Length;
        if (index + step < 0 || index + step >= points.Length) step = -step;
        return index + step;
    }
}
