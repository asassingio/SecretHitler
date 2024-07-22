using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

/*
namespace Managers.PersistentManagers
{
    public class PlayersHandler : MonoBehaviour
    {
        private NetworkManager _networkManager;

        private void Awake()
        {
            InitializeServer();
        }

        private void OnDestroy()
        {
            _networkManager = GetComponent<NetworkManager>();
            if (_networkManager == null)
            {
                Debug.LogError($"PlayersHandler's _networkManager not found!");
                return;
            }
            
            _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
            _networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }

        private void InitializeServer()
        {
            _networkManager = GetComponent<NetworkManager>();
            if (_networkManager == null)
            {
                Debug.LogError($"PlayersHandler's _networkManager not found!");
                return;
            }
            
            _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
            _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            Debug.LogWarning($"PlayerHandler (OnSERVERConnectionState) {args.ConnectionState}");
        }
        
        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            // starting, started
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                Debug.LogWarning($"PlayerHandler (OnCLIENTConnectionState) {args.ConnectionState}");
                // spawn the player for all client
                PlayerSpawnerManager.Instance.SpawnForAll();
            }
        }
    }
}
*/


namespace Managers.PersistentManagers
{
    public class MyPlayerSpawner : MonoBehaviour
    {

        [Header("Player Prefab")] 
        [SerializeField] private NetworkObject playerPrefab;
        
        /// <summary>
        /// True to add player to the active scene when no global scenes are specified through the SceneManager.
        /// </summary>
        [Tooltip("True to add player to the active scene when no global scenes are specified through the SceneManager.")]
        [SerializeField] private bool addToDefaultScene = true;
        
        /// <summary>
        /// Called on the server when a player is spawned.
        /// </summary>
        public event Action<NetworkObject> OnSpawned;
        
        private NetworkManager _networkManager;

        private void Start()
        {
            InitializeOnce();
        }

        private void OnDestroy()
        {
            if (_networkManager != null)
            {
                _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
                _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
            }
        }
        
        private void InitializeOnce()
        {
            _networkManager = InstanceFinder.NetworkManager;
            if (_networkManager == null)
            {
                Debug.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
                return;
            }
            
            _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
            _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        }

        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
        {
            if (obj.ConnectionState == LocalConnectionState.Started)
            {
                Debug.LogWarning("ClientManager_OnClientConnectionState Started");
                // PlayerSpawnerManager.Instance.RefreshPlayers();
            }
            if (obj.ConnectionState == LocalConnectionState.Stopped)
            {
                Debug.LogWarning("ClientManager_OnClientConnectionState Stopped");
                // PlayerSpawnerManager.Instance.RefreshPlayers();
            }
        }

        /// <summary>
        /// Called when a client loads initial scenes after connecting.
        /// </summary>
        private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
        {
            Debug.LogWarning("SceneManager_OnClientLoadedStartScenes");
            
            // only the server will spawn the player
            if (asServer)
            {
                if (playerPrefab == null)
                {
                    Debug.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
                    return;
                }

                Vector3 position = new Vector3(0, 0, 0);
                Quaternion rotation = Quaternion.identity;

                NetworkObject nob = _networkManager.GetPooledInstantiated(playerPrefab, position, rotation, true);
                _networkManager.ServerManager.Spawn(nob, conn);

                //If there are no global scenes 
                if (addToDefaultScene)
                    _networkManager.SceneManager.AddOwnerToDefaultScene(nob);

                OnSpawned?.Invoke(nob);
            }
        }
    }
}