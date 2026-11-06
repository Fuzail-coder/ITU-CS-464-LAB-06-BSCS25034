using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 6f;

    [Header("A crusher: give it a rest time and it strikes on a cycle")]
    [SerializeField] float restTime = 2.4f;
    [SerializeField] float warnTime = 0.7f;
    [SerializeField] float strikeTime = 0.4f;
    [SerializeField] GameObject warning;
    [SerializeField] Transform block;
    [SerializeField] float dropDistance = 3f;

    /*IEnumerator Start()
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

            if (warning != null) warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);
            if (warning != null) warning.SetActive(false);

            for (float t = 0f; t < 0.1f; t += Time.deltaTime)
            {
                block.localPosition = Vector3.Lerp(up, down, t / 0.1f);
                yield return null;
            }
            block.localPosition = down;
            zone.enabled = true;
            yield return new WaitForSeconds(strikeTime);
        }
    }*/
    IEnumerator Start()
    {
        Debug.Log("Crusher Start. restTime = " + restTime, this);
        if (restTime <= 0f) { Debug.Log("restTime is 0, crusher disabled", this); yield break; }

        Collider zone = GetComponent<Collider>();
        if (zone == null) { Debug.LogError("No Collider on the Crusher object", this); yield break; }
        if (block == null) { Debug.LogError("Block slot is empty", this); yield break; }

        Vector3 up = block.localPosition;
        Vector3 down = up - Vector3.up * dropDistance;

        while (true)
        {
            zone.enabled = false;
            block.localPosition = up;
            yield return new WaitForSeconds(restTime);

            Debug.Log("Warning phase", this);
            if (warning != null) warning.SetActive(true);
            yield return new WaitForSeconds(warnTime);
            if (warning != null) warning.SetActive(false);

            Debug.Log("Slam", this);
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
    void OnTriggerEnter(Collider other) => Hit(other);
    void OnTriggerStay(Collider other) => Hit(other);

    void Hit(Collider other)
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