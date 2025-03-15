using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
[SelectionBase]
public class Spawner : NetworkBehaviour
{
    List<GameObject> _allEnemies = new List<GameObject>();
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] float _spawnTime;
    [SerializeField] float _spawnRadius;
    
    bool _isSpawning = true;
    SphereCollider _sphereCollider;
    private void Start()
    {
        _sphereCollider = gameObject.GetComponent<SphereCollider>();
        _sphereCollider.radius = _spawnRadius;
        //StartCoroutine(SpawnEnemies());
    }
    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        if (!IsServer) return;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_spawnTime);
            GameObject enemy = Instantiate(_enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
            _allEnemies.Add(enemy);

        }
    }
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = gameObject.transform.position;
        randomPosition += Random.insideUnitSphere * _spawnRadius;
        randomPosition.y = transform.position.y;

        return randomPosition;
    }
    //used when the game ends
    public void KillAllEnemies(){
        //no more enemies spawning
        StopCoroutine(SpawnEnemies());
        _isSpawning = false;

        //destroys all enemies spawned from this spawner
        foreach(GameObject enemy in _allEnemies){
            enemy.GetComponent<NetworkObject>().Despawn();
            Destroy(enemy);
        }
        _allEnemies.Clear();
    }
}
