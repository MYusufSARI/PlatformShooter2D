using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnTimer = 3f;

    private void Start() {
        StartCoroutine(SpawnRoutine());
    }
    
    private IEnumerator SpawnRoutine() {
        while (true)
        {
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}
