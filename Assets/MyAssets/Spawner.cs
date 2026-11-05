using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Coin")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int maxCoins = 3;
    [SerializeField] float coinHeight = 0.5f;

    [Header("Spawn area (yellow box in the Scene view)")]
    [SerializeField] Vector3 areaSize = new Vector3(9f, 0.1f, 9f);

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] float minDistanceFromPlayer = 1.5f;

    readonly List<GameObject> coins = new List<GameObject>();

    void Update()
    {
        coins.RemoveAll(c => c == null);   // forget coins that were destroyed

        while (coins.Count < maxCoins)
            coins.Add(Instantiate(coinPrefab, RandomPoint(), Quaternion.identity));
    }

    Vector3 RandomPoint()
    {
        Vector3 c = transform.position;
        Vector3 pos = c;

        for (int i = 0; i < 20; i++)
        {
            pos = new Vector3(
                c.x + Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                c.y + coinHeight,
                c.z + Random.Range(-areaSize.z / 2f, areaSize.z / 2f));

            if (player == null) break;

            Vector3 flat = pos - player.position;
            flat.y = 0f;
            if (flat.magnitude >= minDistanceFromPlayer) break;
        }
        return pos;
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.up * coinHeight;

        Gizmos.color = new Color(1f, 0.92f, 0.1f, 0.15f);
        Gizmos.DrawCube(center, areaSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, areaSize);
    }
}