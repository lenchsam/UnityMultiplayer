using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PathFinding : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    GameObject _player;

    bool _isUpdating = true;
    void Start()
    {
        _player = GetRandomPlayer();
        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(_player.transform.position);

        if( GetRandomPlayer() != null)
            StartCoroutine(UpdateDestination());
    }
    GameObject GetRandomPlayer() {
        GameObject player;
        if (GameObject.FindAnyObjectByType<LightThingsUp>() != null)
        {
            player = GameObject.FindAnyObjectByType<LightThingsUp>().gameObject;
        }
        else
        {
            player = GameObject.FindAnyObjectByType<Player_Input_Handler>().gameObject;
        }
        return player;
    }
    IEnumerator UpdateDestination()
    {
        while (_isUpdating)
        {
            yield return new WaitForSeconds(1);

            _navMeshAgent.SetDestination(_player.transform.position);
        }
    }
}
