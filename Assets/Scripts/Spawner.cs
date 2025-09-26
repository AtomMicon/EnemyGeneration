using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _spawnPointCount = 4;
    [SerializeField] private float _spawnInterval = 6f;
    [SerializeField] private float _planeSize = 1;
    [SerializeField] private bool _isSpawning = true;

    private ObjectPool<Enemy> _enemyPool;
    private List<Vector3> _spawnPoints;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemyPrefab).GetComponent<Enemy>(),
            actionOnGet: enemy => GetEnemy(),
            actionOnRelease: enemy => ReleaseEnemy(enemy),
            actionOnDestroy: enemy => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: 80,
            maxSize: 10
        );
    }

    private void Start()
    {
        StartCoroutine(WaitRoutine());
        InitializeSpawnPoints();
    }

    private IEnumerator WaitRoutine()
    {
        while (_isSpawning == true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            GetEnemy();
        }
    }

    private void InitializeSpawnPoints()
    {
        for(int i = 0; i < _spawnPointCount; i++)
        {
            Vector3 spawnPoint = CreateSpawnPoint();
            _spawnPoints.Add(spawnPoint);
        }
    }

    private Vector3 CreateSpawnPoint()
    {
        float xArea = Random.Range(-_planeSize, _planeSize);
        float zArea = Random.Range(-_planeSize, _planeSize);

        return new Vector3(xArea, 0, zArea);
    }

    private Vector3 GetSpawnPoint()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomIndex];
    }

    private void GetEnemy()
    {
        Enemy enemy = _enemyPool.Get();

        enemy.Died += ReleaseEnemy;
        enemy.gameObject.SetActive(true);
        enemy.StandOnPosition(GetSpawnPoint());

        Vector3 direction = GetRandomVector();
        enemy.Go(direction);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.Died -= ReleaseEnemy;

        _enemyPool.Release(enemy);
    }

    private Vector3 GetRandomVector()
    {
        float randomIndex_1 = Random.Range(-_planeSize, _planeSize);
        float randomIndex_2 = Random.Range(-_planeSize, _planeSize);

        return new Vector3(randomIndex_1, randomIndex_2);
    }
}
