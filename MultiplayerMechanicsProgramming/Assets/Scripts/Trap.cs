using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Trap : MonoBehaviour
{
    [SerializeField] float _damage = 10;
    [SerializeField] float _tickDamageSpeed = 0.5f;
    bool isInCollider = false;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("entered");
        if (other.tag != "player") return;
        Health healthScript = other.GetComponent<Health>();
        if (healthScript != null) isInCollider = true;
        healthScript.TakeDamage(_damage);
        StartCoroutine(tickDamage(healthScript));
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "player") return;
        //Debug.Log("exited");
        isInCollider = false;
    }
    IEnumerator tickDamage(Health healthScript)
    {
        while (isInCollider) {
            //Debug.Log("Ticking");
            healthScript.TakeDamage(_damage);
            yield return new WaitForSeconds(_tickDamageSpeed);
        }
    }
}
