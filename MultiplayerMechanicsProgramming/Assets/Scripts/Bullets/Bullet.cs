using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _destroyTime, _damage = 10, _movementSpeed = 1;
    Vector3 _startPosition;
    public Vector3 Direction;
    void Start(){
        Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(Direction * _movementSpeed, ForceMode.Impulse);
        _startPosition = transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        if(other.gameObject.tag != "Enemy") {
            Destroy(gameObject);
            return;
        }
        Health healthScript = other.GetComponentInParent<Health>();
        healthScript.TakeDamage(_damage);
        
        gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
