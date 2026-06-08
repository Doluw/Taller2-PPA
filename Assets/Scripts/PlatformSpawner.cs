using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Spawnable Objects")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.8f;
    [SerializeField] private float[] lanes = { -3f, 0f, 3f };
    [SerializeField] private float minLocalZ = -4f;
    [SerializeField] private float maxLocalZ = 4f;
    [SerializeField] private float coinSpawnHeight = 1f;
    [SerializeField] private float zombieSpawnHeight = 1f;
    [SerializeField] private float obstacleSpawnHeight = 0.3f;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomObject();
        }
    }

    public void SpawnRandomObject()
    {
        if (Random.value > spawnChance)
        {
            return;
        }

        GameObject prefab = ChooseRandomPrefab();

        if (prefab == null || lanes == null || lanes.Length == 0)
        {
            return;
        }

        float laneX = lanes[Random.Range(0, lanes.Length)];
        float localZ = Random.Range(minLocalZ, maxLocalZ);
        Vector3 localPosition = new Vector3(laneX, GetSpawnHeight(prefab), localZ);

        GameObject spawnedObject = Instantiate(prefab, transform);
        spawnedObject.transform.localPosition = localPosition;
        spawnedObject.transform.localRotation = Quaternion.identity;
    }

    private GameObject ChooseRandomPrefab()
    {
        GameObject[] options = { coinPrefab, zombiePrefab, obstaclePrefab };
        return options[Random.Range(0, options.Length)];
    }

    private float GetSpawnHeight(GameObject prefab)
    {
        if (prefab == coinPrefab)
        {
            return coinSpawnHeight;
        }

        if (prefab == zombiePrefab)
        {
            return zombieSpawnHeight;
        }

        return obstacleSpawnHeight;
    }
}
