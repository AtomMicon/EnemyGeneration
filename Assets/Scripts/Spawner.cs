using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyCount = 8;
    [SerializeField] private int _enemyMaxCount = 10;
    [SerializeField] private SpawnPoint _spawnPointPrefab;
    [SerializeField] private int _spawnPointCount = 4;
    [SerializeField] private int _spawnMaxPointCount = 6;
    [SerializeField] private float _spawnInterval = 6f;
    [SerializeField] private bool _isSpawning = true;
    [SerializeField] private float _planeSize = 2f;

    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<SpawnPoint> _spawnPointPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemyPrefab).GetComponent<Enemy>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => ReleaseEnemy(enemy),
            actionOnDestroy: enemy => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: _spawnPointCount,
            maxSize: _enemyMaxCount
        );

        _spawnPointPool = new ObjectPool<SpawnPoint>(
            createFunc: () => Instantiate(_spawnPointPrefab),
            actionOnGet: spawnPoint => spawnPoint.gameObject.SetActive(true),
            actionOnRelease: spawnPoint => spawnPoint.gameObject.SetActive(false),
            actionOnDestroy: spawnPoint => Destroy(spawnPoint.gameObject),
            collectionCheck: true,
            defaultCapacity: _spawnPointCount,
            maxSize: _spawnMaxPointCount
        );
    }

    private void Start()
    {
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        while (_isSpawning == true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            GetEnemy();
        }
    }

    private void CreateSpawnPoint()
    {
        SpawnPoint spawnPoint = Instantiate(_spawnPointPrefab);
        spawnPoint.InitializeSpawnPoint();
    }

    private void GetEnemy()
    {
        Enemy enemy = _enemyPool.Get();
        SpawnPoint spawnPoint = _spawnPointPool.Get();

        enemy.Died += ReleaseEnemy;
        enemy.gameObject.SetActive(true);
        enemy.StandOnPosition(spawnPoint.gameObject.transform.position);

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
