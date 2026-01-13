using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnInterval = 1.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-8f, 8f),
            Random.Range(-5f, 5f),
            15f
        );

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}
