using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;

    public event Action<Enemy> Died;

    public void StandOnPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Go(Vector3 direction)
    {
        transform.position += direction * _speed * Time.deltaTime;
        StartCoroutine(WaitRoutine());
    }

    public void ResetEnemy()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        ResetEnemy();
        Died?.Invoke(this);
    }
}
