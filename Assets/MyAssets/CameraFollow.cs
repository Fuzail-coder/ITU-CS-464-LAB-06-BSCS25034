using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -8);
    public float smoothSpeed = 5f;
    public Vector3 lookOffset = new Vector3(0, 1, 0);

    void LateUpdate()
    {
        Vector3 wanted = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, wanted,
                                          smoothSpeed * Time.deltaTime);

        transform.LookAt(target.position + lookOffset);
    }
}