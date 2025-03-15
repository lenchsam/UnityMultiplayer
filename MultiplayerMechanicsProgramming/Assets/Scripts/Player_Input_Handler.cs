using System.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[SelectionBase]
public class Player_Input_Handler : NetworkBehaviour
{
    [SerializeField] private float _gravityValue = -35f;
    [SerializeField] private float _playerSpeed = 4.0f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private Camera _camera;
    private float _jumpBufferCounter;

    CharacterController _controller;
    PlayerInputAction _playerActions;
    Vector3 _playerVelocity;
    bool _groundedPlayer;

    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
    }
    private void OnEnable()
    {
        _playerActions = new PlayerInputAction();
        _playerActions.Enable();
    }
    private void OnDisable()
    {
        _playerActions.Disable();
    }
    private void Update()
    {
        if (!NetworkObject.IsOwner) return;
        // Gravity
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
        // Is player Grounded
        _groundedPlayer = _controller.isGrounded;
        if (_playerVelocity.y < 0 && _groundedPlayer)
        {
            _playerVelocity.y = 0f;
        }
        //Basic Moving
        Vector2 input = _playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Sprinting
        if (_playerActions.Player.Sprint.IsPressed() && _groundedPlayer)
        {
            move *= _sprintMultiplier;
        }

        if (move != new Vector3(0, 0, 0))
        {
            _controller.Move(move * Time.deltaTime * _playerSpeed);

            // Smooth rotation towards moving direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // Jump Buffer
        if (_playerActions.Player.Jump.triggered)
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        // Makes the player jump
        if (_jumpBufferCounter > 0 && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravityValue);
        }
    }
    public override void OnNetworkSpawn()
    {
        //Debug.Log("Ran");
        _camera.enabled = IsOwner;
        
    }
}
