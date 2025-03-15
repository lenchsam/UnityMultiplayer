using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : NetworkBehaviour
{
    Camera _camera;
    [SerializeField] float _shootDistance = 50;
    [SerializeField] GameObject _bullet;

    PlayerInputAction _playerActions;
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
        _playerActions.Player.Attack.performed += Shoot;
        _camera = GetComponentInChildren<Camera>();
    }
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        //raycast to get mouse world position
        Vector3 mousePosition = new Vector3(0,0,0);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, ~LayerMask.GetMask("FogOfWar")))
        {
            mousePosition = hitData.point;
        }
        
        //get mouse direction
        Vector3 directionToMouse = (mousePosition - transform.position).normalized;
        directionToMouse.y = 0;
        directionToMouse.Normalize();

        //instantiate bullet over network
        GameObject bullet = Instantiate(_bullet, new Vector3(transform.position.x, 0.5f,transform.position.z), Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Direction = directionToMouse;

        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
