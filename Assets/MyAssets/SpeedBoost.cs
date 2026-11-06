using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] float multiplier = 1.5f;
    [SerializeField] float duration = 5f;
    [SerializeField] float spinSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        player.BoostSpeed(multiplier, duration);
        Destroy(gameObject);
    }
}