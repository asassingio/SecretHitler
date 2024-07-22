using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Transporting;
using PlayerComponents;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers.PersistentManagers
{
    public sealed class InactivityHandler : Manager<InactivityHandler>
    {
        private MyPlayerController _ownerPlayerController;
        private bool _isGamePaused;
        
        public void SetActiveSelf()
        {
            while (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        /*
        private void Start()
        {
            InitializeOnce();
        }

        private void OnDestroy()
        {
            if (NetworkManager != null)
            {
                ClientManager.OnClientConnectionState -= OnClientConnectionState;
                // ServerManager.OnServerConnectionState -= OnServerConnectionState;
            }
        }

        private void InitializeOnce()
        {
            if (NetworkManager == null)
            {
                Debug.LogWarning(
                    $"Initializer on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
                return;
            }

            _ownerPlayerController = FindObjectOfType<MyPlayerController>();
            if (_ownerPlayerController == null)
            {
                Debug.LogWarning($"no MyPlayerController found");
                return;
            }

            ClientManager.OnClientConnectionState += OnClientConnectionState;
            // ServerManager.OnServerConnectionState += OnServerConnectionState;

            GameManager.Instance.Initialize(_ownerPlayerController);
            ViewManager.Instance.Initialize(_ownerPlayerController);
            MenuManager.Instance.Initialize(_ownerPlayerController);

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
            clientManagerConnection.Kick(KickReason.Unset, LoggingType.Common,
                $"{ClientManager.Connection.ClientId.ToString()}");
        }
        */
    }
}