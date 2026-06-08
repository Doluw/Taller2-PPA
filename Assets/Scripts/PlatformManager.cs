using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField, Min(10)] private int initialPlatformCount = 15;
    [SerializeField] private float platformLength = 20f;
    [SerializeField] private float spawnZ = 0f;
    [SerializeField] private float destroyDistanceBehindPlayer = 25f;

    private readonly Queue<GameObject> activePlatforms = new Queue<GameObject>();

    private void Start()
    {
        initialPlatformCount = Mathf.Clamp(initialPlatformCount, 10, 30);

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        if (player == null || activePlatforms.Count == 0)
        {
            return;
        }

        GameObject oldestPlatform = activePlatforms.Peek();
        float platformEndZ = oldestPlatform.transform.position.z + platformLength;

        if (platformEndZ < player.position.z - destroyDistanceBehindPlayer)
        {
            Destroy(activePlatforms.Dequeue());
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        if (platformPrefabs == null || platformPrefabs.Length == 0)
        {
            Debug.LogWarning("PlatformManager necesita al menos un prefab de plataforma.");
            return;
        }

        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(0f, 0f, spawnZ);
        GameObject platform = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);

        activePlatforms.Enqueue(platform);
        spawnZ += platformLength;
    }
}
