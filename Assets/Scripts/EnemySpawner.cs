using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;

    [Header("Player")]
    public Transform player;

    [Header("Spawn Settings")]
    public float minDistance = 8f;
    public float maxDistance = 15f;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    [Header("Angle Control")]
    public float forwardAngle = 60f; // how wide in front enemies can spawn

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();

            float wait = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || player == null)
            return;

        Vector3 spawnPos = GetSpawnPosition();

        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        GameObject enemy = Instantiate(
            enemyPrefabs[enemyIndex],
            spawnPos,
            Quaternion.identity
        );

        // 🔥 Hook AI to player
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.player = player;
        }
    }

    Vector3 GetSpawnPosition()
    {
        // Player forward direction
        Vector3 forward = player.forward;

        // Random angle in front cone
        float angle = Random.Range(-forwardAngle, forwardAngle);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 direction = rotation * forward;

        // Random distance
        float distance = Random.Range(minDistance, maxDistance);

       Vector3 spawnPos = player.position + direction.normalized * distance;

        // 🔥 Raycast down to ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + Vector3.up * 50f, Vector3.down, out hit, 100f))
        {
            spawnPos = hit.point;
        }

        UnityEngine.AI.NavMeshHit navHit;
if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out navHit, 2f, UnityEngine.AI.NavMesh.AllAreas))
{
    spawnPos = navHit.position;
}
        return spawnPos;
    }
}