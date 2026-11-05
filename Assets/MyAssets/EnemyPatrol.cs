using UnityEngine;

public enum PatrolMode { Loop, PingPong, Random }

// Walks its waypoints at a constant speed, turning to face where it walks.
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] points;   // set in the Inspector: C shows them
    [SerializeField]
    PatrolMode mode = PatrolMode.
Loop
;
    [SerializeField] float speed = 2.6f;
    [SerializeField] float waitTime = 0f;
    [SerializeField] float turnSpeed = 360f;
    int index;
    int step = 1;
    float waitUntil;

    void Update()
    {
        if (Time.time < waitUntil) return;
        Vector3 target = points[index].position;
        Vector3 toTarget = target - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (toTarget != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeed * Time.deltaTime);
        }
        if (transform.position == target)
        {
            index = NextIndex();
            waitUntil = Time.time + waitTime;
        }
    }

    int NextIndex()
    {
        if (mode == PatrolMode.Loop) return (index + 1) % points.Length;
        if (mode == PatrolMode.PingPong)
        {
            if (index + step < 0 || index + step >= points.Length) step = -step;
            return index + step;
        }
        int next = Random.Range(0, points.Length);
        return next == index ? (next + 1) % points.Length : next;
    }
}
