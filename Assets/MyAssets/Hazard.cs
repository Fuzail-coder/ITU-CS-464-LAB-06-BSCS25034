using System.Collections;
using UnityEngine;

// Hurts whatever has a Health and stays in its trigger. Spikes, guards and the crusher all use it.
public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;   // the crusher's Inspector says 2
    [SerializeField] float knockback = 6f;

    [Header("A crusher: give it a rest time and it strikes on a cycle")]
    [SerializeField] float restTime = 2.4f;   // the crusher's Inspector values
    [SerializeField] float warnTime = 0.7f;
    [SerializeField] float strikeTime = 0.4f;
    [SerializeField] GameObject warning;
    [SerializeField] Transform block;        // the red block that slams down
    [SerializeField] float dropDistance = 3f;
    IEnumerator Start()
    {
        if (restTime <= 0f) yield break;
        Collider zone = GetComponent<Collider>();

        Vector3 up = block.localPosition;
        Vector3 down = up - Vector3.up * dropDistance;

        while (true)
        {
            zone.enabled = false;
            block.localPosition = up;
            yield return new WaitForSeconds(restTime);

            warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);
            warning.SetActive(false);

            // slam down fast
            for (float t = 0f; t < 0.1f; t += Time.deltaTime)
            {
                block.localPosition = Vector3.Lerp(up, down, t / 0.1f);
                yield return null;
            }
            block.localPosition = down;
            zone.enabled = true;
            yield return new WaitForSeconds(strikeTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Health health)) return;
        if (!health.TakeDamage(damage)) return;

        Rigidbody body = other.attachedRigidbody;
        if (body == null) return;

        Vector3 away = body.position - transform.position;
        away.y = 0f;
        body.AddForce(away.normalized * knockback, ForceMode.Impulse);
    }
}