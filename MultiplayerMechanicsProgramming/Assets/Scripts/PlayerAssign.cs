using Unity.Netcode;
using UnityEngine;

public class PlayerAssign : MonoBehaviour
{
    NetworkObject _networkObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _networkObject = GetComponent<NetworkObject>();
        if(_networkObject.OwnerClientId == 0)
        {
            //gameObject.AddComponent<ViewLight>();
            Destroy(gameObject.GetComponent<LightThingsUp>());
            Destroy(this);
        }
        else
        {
            //player that can light things up. should have lightThingUp script. 
            //Should not have audio listener, shooting script or viewLightScript
            gameObject.GetComponent<LightThingsUp>().enabled = true;
            gameObject.GetComponentInChildren<Camera>().cullingMask &= ~LayerMask.GetMask("Light"); // ~ to invert the bitmask
            Destroy(gameObject.GetComponentInChildren<AudioListener>());
            Destroy(gameObject.GetComponent<Shooting>());
            Destroy(gameObject.GetComponent<ViewLight>());
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
