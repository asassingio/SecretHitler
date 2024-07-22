using System;
using System.Collections.Generic;
using Enums;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 run order:
 Awake()
 OnEnable()
 Start()
 OnStartClient()
 */


namespace Managers
{
    public sealed class LobbyManager : Manager<LobbyManager>
    {
        [Header("Debugging")] 
        [SerializeField] private MyPlayerController _ownerPlayerController;

        public void ManualInitialize(MyPlayerController ownerPlayerController)
        {
            _ownerPlayerController = ownerPlayerController;
        }
        
        /*
        /// <summary>
        /// Initialize the GameManager and sync its variables for all clients
        /// Gets a list of all the players and the ownerPlayerController
        /// At the end of it isInitialized will be true
        /// </summary>
        /// <param name="playerController">The owner player controller</param>
        public void Initialize(PlayerController playerController)
        {
            _ownerPlayerController = playerController;
            GameManagerSetPlayersList();
        }

        public void GameManagerRemovePlayersFromList(PlayerController ownerPlayerController)
        {
            _players.AddRange(FindObjectsOfType<PlayerController>());
            _players.Remove(ownerPlayerController);
            ServerSetPlayersList(_players.ToArray());
        }
    
        /// <summary>
        /// Set Player list and Admin for all
        /// </summary>
        private void GameManagerSetPlayersList()
        {
            // Debug.LogError("GameManagerSetPlayersList");
            ServerSetPlayersList(FindObjectsOfType<PlayerController>());
        }
    
        [ServerRpc(RequireOwnership = false)]
        private void ServerSetPlayersList(PlayerController[] players)
        {
            RpcSetPlayersList(players);
        }
    
        [ObserversRpc]
        private void RpcSetPlayersList(PlayerController[] players)
        {
            // Debug.LogError("RpcSetPlayersList");
            _players.Clear();
            _players.AddRange(players);

            foreach (PlayerController player in players)
            {
                if (player.ServerGetIsAdmin())
                {
                    isInitialized = true;
                    return;
                }
            }
            _players[0].ServerSetIsAdmin(true);
        
            isInitialized = true;
        }
    
        /// <summary>
        /// Determines if the player of the given <paramref name="playerController"/> is an admin.
        /// If it's not and isInitialized and there is no admin in the game. it will generate one
        /// This method MUST be used anywhere instead of playerController.ServerGetIsAdmin()
        /// </summary>
        public bool GameManagerServerGetIsAdmin(PlayerController playerController)
        {
            if (playerController.ServerGetIsAdmin())
            {
                return true;
            }

            if (isInitialized)
            {
                foreach (PlayerController player in _players)
                {
                    if (player.ServerGetIsAdmin())
                    {
                        return false;
                    }
                }
                _players[0].ServerSetIsAdmin(true);
            
                if (playerController == _players[0])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return a list of all players' names.
        /// </summary>
        /// <returns>A list of strings representing the names of all players.</returns>
        public List<string> GetPlayersName()
        {
            List<string> names = new List<string>();
            foreach (PlayerController player in _players)
            {
                names.Add(player.ServerGetName());
            }
            return names;
        }
    
        public List<string> GetInGamePlayersName()
        {
            List<string> names = new List<string>();
            foreach (PlayerController player in _players)
            {
                if (player.ServerGetIsInGame())
                {
                    names.Add(player.ServerGetName());
                }
            }
            return names;
        }

        /// <summary>
        /// Get a random player name from the list of players.
        /// </summary>
        /// <returns>A random player name.</returns>
        public string ServerGetRandomPlayer()
        {
            int randomIndex = Random.Range(0, _players.Count); // from 0 to numPlayers-1
            return _players[randomIndex].ServerGetName();
        }

        /// <summary>
        /// Gets the count of players who are currently alive.
        /// </summary>
        /// <returns>The count of alive players.</returns>
        public int GetInGamePlayersCount()
        {
            int playerInGameCount=0;
            foreach (PlayerController player in _players)
            {
                if (player.ServerGetIsInGame())
                {
                    playerInGameCount++;
                }
            }
            return playerInGameCount;
        }
    
        /// <summary>
        /// Gets the count of players who are currently ready.
        /// </summary>
        /// <returns>The count of alive players.</returns>
        public int GetIsReadyPlayersCount()
        {
            int playersIsReadyCount=0;
            foreach (PlayerController player in _players)
            {
                if (player.ServerGetIsReady())
                {
                    playersIsReadyCount++;
                }
            }
            return playersIsReadyCount;
        }

        /// <summary>
        /// Return a list of all the players that have the same targetRole.
        /// </summary>
        /// <param name="targetRole">The target role to filter the players by</param>
        /// <returns>A list of player names with the same targetRole</returns>
        public List<string> GetPlayersNameWithSameRole(SecretRoleEnum targetRole)
        {
            List<string> names = new List<string>();
            foreach (PlayerController player in _players)
            {
                Debug.LogWarning($"{player.ServerGetRole()} {targetRole} {player.ServerGetRole() == targetRole}: isInGame = {player.ServerGetIsInGame()}");
                if ((SecretRoleEnum) player.ServerGetRole() == targetRole)
                {
                    names.Add(player.ServerGetName());
                    // if (player.ServerGetIsInGame())
                    // {
                    //     names.Add(player.ServerGetName());
                    // }
                }
            }
            return names;
        }

        /// <summary>
        /// Retrieves a PlayerComponent by their name.
        /// </summary>
        /// <param name="pName">String of name of the PlayerComponent to retrieve.</param>
        /// <returns>The PlayerController with the given name. Throws an ArgumentException if no player is found.</returns>
        public PlayerController GetPlayerByName(string pName)
        {
            foreach (PlayerController player in _players)
            {
                if (player.ServerGetName() == pName)
                {
                    return player;
                }
            }
            throw new ArgumentException($"No player found with the provided name: {nameof(pName)}");
        }

        /// <summary>
        /// Retrieves the current game mode.
        /// </summary>
        /// <returns>The current game mode.</returns>
        public GameModeEnum ServerGetGameMode()
        {
            return _gameMode;
        }
    
        /// <summary>
        /// Set the current game mode.
        /// </summary>
        /// <returns>The current game mode.</returns>
        [ServerRpc(RequireOwnership = false)]
        public void ServerSetGameMode(GameModeEnum gameMode)
        {
            _gameMode = gameMode;
        }

        /// <summary>
        /// load the target player to GameOverView
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ServerKillTarget(NetworkConnection conn)
        {
            RpcKillTarget(conn);
        }
    
        [TargetRpc]
        private void RpcKillTarget(NetworkConnection conn)
        {
            ViewManager.Instance.Show<GameOverView>();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerSetShouldGenerateNewDeck(bool status)
        {
            shouldGenerateNewDeck = status;
        }

        /// <summary>
        /// Gets the value of shouldGenerateNewDeck and sets it to false.
        /// This method will ONLY be called by the GameVIewDeckMenu
        /// </summary>
        /// <returns>The value of shouldGenerateNewDeck before being set to false.</returns>
        public bool GetShouldGenerateNewDeck()
        {
            if (shouldGenerateNewDeck)
            {
                ServerSetShouldGenerateNewDeck(false);
                return true;
            }
        
            return false;
        }

        public int GetClientsCount()
        {
            if (isEditorTesting)
            {
                return NetworkManager.ClientManager.Clients.Count - 1;
            }
            return NetworkManager.ClientManager.Clients.Count;
        }

        public void Update()
        {
            if (!isInitialized) return;
            if (!IsClientInitialized) return;
            if (!ViewManager.Instance.IsClientInitialized) return;
        
            foreach(PlayerController player in _players)
            {
                if (player == null)
                {
                    GameManagerSetPlayersList();
                }
            }
        }
        */
    }
}