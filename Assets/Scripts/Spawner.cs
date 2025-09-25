using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _planeSize = 1;
    [SerializeField] private GameObject _spawnPoint_1;
    [SerializeField] private GameObject _spawnPoint_2;
    [SerializeField] private GameObject _spawnPoint_3;
    [SerializeField] private GameObject _spawnPoint_4;

    private ObjectPool<Enemy> _enemyPool;
    private List<GameObject> _spawnPoints = new List<GameObject>();

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

        _spawnPoints.Add(_spawnPoint_1);
        _spawnPoints.Add(_spawnPoint_2);
        _spawnPoints.Add(_spawnPoint_3);
        _spawnPoints.Add(_spawnPoint_4);
    }

    private void Update()
    {
        GenerateEnemy();
    }

    private void GenerateEnemy()
    {
        while (true)
        {
            StartCoroutine(WaitRoutine());
        }
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(_spawnInterval);
        GetEnemy();
    }

    private void GetEnemy()
    {
        Enemy enemy = _enemyPool.Get();
        enemy.Died += ReleaseEnemy;
        enemy.gameObject.SetActive(true);

        
        GameObject spawnPoint = ChooseSpawnPoint();
        enemy.transform.position = spawnPoint.transform.position;

        Vector3 direction = GetRandomVector();
        enemy.Go(direction);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        enemy.ResetEnemy();
        enemy.gameObject.SetActive(false);
        enemy.Died -= ReleaseEnemy;
        _enemyPool.Release(enemy);
    }

    private GameObject ChooseSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }

    private Vector3 GetRandomVector()
    {
        float randomIndex_1 = Random.Range(0, _planeSize);
        float randomIndex_2 = Random.Range(0, _planeSize);
        
        return new Vector3(randomIndex_1, randomIndex_2);
    }
}
