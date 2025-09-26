using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float _planeSize = 1;

    public void InitializeSpawnPoint()
    {
        Vector3 spawnPoint = CreateSpawnPoint();
        gameObject.transform.position = spawnPoint;
    }

    private Vector3 CreateSpawnPoint()
    {
        float xArea = Random.Range(-_planeSize, _planeSize);
        float zArea = Random.Range(-_planeSize, _planeSize);

        return new Vector3(xArea, 0, zArea);
    }
}
