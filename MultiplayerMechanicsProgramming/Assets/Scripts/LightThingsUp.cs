using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;
using System;

public class LightThingsUp : NetworkBehaviour
{
    [SerializeField] TMP_Text _lightText;
    int _numLitLights = 0;
    PlayerInputAction _playerActions;
    [SerializeField] Camera _camera;
    [SerializeField] LayerMask _layerMask;
    private void OnEnable()
    {

        _playerActions = new PlayerInputAction();
        _playerActions.Enable();
    }
    private void OnDisable()
    {
        _playerActions.Disable();
    }
    private void Start()
    {
        _lightText = GameObject.Find("NumLightText").GetComponent<TMP_Text>();
        _playerActions.Player.Attack.performed += Handle_LightUp;
    }
    void Handle_LightUp(InputAction.CallbackContext ctx)
    {
        if (!NetworkObject.IsOwner) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Collide))
        {
           if (hit.transform.tag == "Light")
           {
               LightUpEveryoneRpc(hit.transform.GetChild(0).gameObject);
           }
        }
    }
    [Rpc(SendTo.Everyone)]
    private void LightUpEveryoneRpc(NetworkObjectReference gameObjectRefToLightUp)
    {
        NetworkObject gameObjectToLightUp;
        if(gameObjectRefToLightUp.TryGet(out gameObjectToLightUp))
        {
            _numLitLights++;
            _lightText.text = _numLitLights + "/5 Lights illuminated";
            gameObjectToLightUp.transform.parent.GetComponent<Lights>().IsLit = true;
            gameObjectToLightUp.GetComponent<Renderer>().material.color = Color.yellow;
            gameObjectToLightUp.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
