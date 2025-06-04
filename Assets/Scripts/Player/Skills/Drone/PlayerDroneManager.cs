using UnityEngine;

public class PlayerDroneManager : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab;
    private GameObject[] activeDrones;

    private void Start()
    {
        int droneCount = Mathf.RoundToInt(StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_COUNT));

        if (droneCount <= 0) return;

        activeDrones = new GameObject[droneCount];
        SpawnDrones(droneCount);
    }


    private void Update()
    {
        UpdateDronePositions();
    }

    private void SpawnDrones(int count)
    {
        float radius = 1.5f;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject drone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
            activeDrones[i] = drone;
        }
    }

    private void UpdateDronePositions()
    {
        if (activeDrones == null) return;

        float radius = 1.5f;
        float angleStep = 360f / activeDrones.Length;

        for (int i = 0; i < activeDrones.Length; i++)
        {
            if (activeDrones[i] == null) continue;

            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            activeDrones[i].transform.position = transform.position + offset;
        }
    }
}
