using UnityEngine;

// Waits at home, chases the player when close, gives up when the player gets far away.
public class EnemyChaser : MonoBehaviour
{
    [SerializeField] Transform target;          // leave empty: finds the object tagged Player
    [SerializeField] float speed = 3f;          // keep it a bit below the player's speed
    [SerializeField] float detectRange = 8f;    // starts chasing inside this distance
    [SerializeField] float loseRange = 12f;     // gives up beyond this distance
    [SerializeField] float turnSpeed = 360f;

    Vector3 home;
    bool chasing;

    void Start()
    {
        home = transform.position;

        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        float dist = FlatDistance(target.position);

        if (!chasing && dist <= detectRange) chasing = true;
        else if (chasing && dist > loseRange) chasing = false;

        Vector3 goal = chasing ? target.position : home;
        MoveToward(goal);
    }

    void MoveToward(Vector3 goal)
    {
        goal.y = transform.position.y;   // stay on the floor
        Vector3 dir = goal - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeed * Time.deltaTime);
        }
    }

    float FlatDistance(Vector3 p)
    {
        Vector3 d = p - transform.position;
        d.y = 0f;
        return d.magnitude;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}