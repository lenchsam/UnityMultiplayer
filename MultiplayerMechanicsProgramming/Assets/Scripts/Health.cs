using UnityEngine;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _health;
    [SerializeField] bool _canRespawn = false;
    Transform _spawnPoint;
    private void Start()
    {
        _health = _maxHealth;
        _spawnPoint = GameObject.Find("SpawnPoint").transform;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if(!IsOwner) return;
        if (_canRespawn) {
            Debug.Log("Respawn");
            gameObject.transform.position = _spawnPoint.position;
            Physics.SyncTransforms();
            _health = _maxHealth;
        }
        else
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
