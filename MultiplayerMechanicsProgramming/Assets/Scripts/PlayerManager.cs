using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> _PlayerCount = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );
    private TextMeshProUGUI _PlayerCounterDisplay;
    private void Awake()
    {
        _PlayerCounterDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                _PlayerCount.Value++;
            };
            NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                _PlayerCount.Value--;
            };
            _PlayerCount.OnValueChanged += (int previousValue, int newValue) =>
            {
                Debug.Log($"The current amount of players connected is {_PlayerCount.Value}");

                _PlayerCounterDisplay.text = $"Current playre count is {_PlayerCount.Value}";
            };
        }
        else if (IsClient) 
        {
            _PlayerCount.OnValueChanged += (int previousValue, int newValue) =>
            {
                _PlayerCounterDisplay.text = $" Current player count is {_PlayerCount.Value}";
            };
        }
    }
}
