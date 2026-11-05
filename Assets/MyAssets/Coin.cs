using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 90f;

    bool collected;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;
        collected = true;

        GameManager.Instance.AddScore(1);
        Destroy(gameObject);
    }
}