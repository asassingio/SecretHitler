using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Managing.Logging;
using FishNet.Managing.Server;
using Managers;
using Managers.CanvasManagers;
using PlayerComponents;
using UnityEngine;

public class GameInitializer : NetworkBehaviour
{
    /*
    private MyPlayerController _ownerPlayerController;
    private NetworkManager _networkManager;
    private bool _isInitialized;
    private bool _isGamePaused;
    
    private void Start()
    {
        if (GameManager.Instance.isEditorTesting)
        {
            if (Application.isEditor)
            {
                return;
            }
        }
        InitializeOnce();
    }
    
    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            ClientManager.OnClientConnectionState -= OnClientConnectionState;
            // ServerManager.OnServerConnectionState -= OnServerConnectionState;
        }
    }

    private void InitializeOnce()
    {
        // network stuff
        _networkManager = InstanceFinder.NetworkManager;
        if (_networkManager == null)
        {
            Debug.LogWarning(
                $"Initializer on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }

        _ownerPlayerController = FindObjectOfType<PlayerController>();
        if (_ownerPlayerController == null)
        {
            Debug.LogWarning($"no PlayerController found");
            return;
        }
        
        ClientManager.OnClientConnectionState += OnClientConnectionState;
        // ServerManager.OnServerConnectionState += OnServerConnectionState;
        
        GameManager.Instance.Initialize(_ownerPlayerController);
        ViewManager.Instance.ManualInitialize(_ownerPlayerController);
        MenuManager.Instance.ManualInitialize(_ownerPlayerController);

        _isInitialized = true;
    }
    
    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        Debug.Log($"Connection State of the client: {args.ConnectionState}");
        GameManager.Instance.GameManagerRemovePlayersFromList(_ownerPlayerController);
    }
        
    private void OnServerConnectionState(ServerConnectionStateArgs args)
    {
        Debug.Log($"Connection State of the SERVER: {args.ConnectionState}");
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (Application.isEditor) return;
        if (!_isInitialized || _networkManager == null || !_networkManager.Initialized) return;

        // is game in backgound?        
        _isGamePaused = !hasFocus;
        if (!_isGamePaused) return;
        
        Debug.Log($"Should remove player {_ownerPlayerController.OwnerId}?");
        if (!_ownerPlayerController.ServerGetIsInGame())
        {
            ServerKickPlayer(ClientManager.Connection);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerKickPlayer(NetworkConnection clientManagerConnection)
    {
        clientManagerConnection.Kick(KickReason.Unset, LoggingType.Common, $"{ClientManager.Connection.ClientId.ToString()}");
    }
    */
}
