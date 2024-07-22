using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using PlayerComponents;
using UnityEngine;

namespace Managers.PersistentManagers
{
    public sealed class PlayerManager : Manager<PlayerManager>
    {
        [Header("Debug Info")] 
        [field: SerializeField] private List<MyPlayerController> _players = new List<MyPlayerController>();
        [field: SerializeField] public MyPlayerController OwnerMyPlayerController { get; private set;}
        private bool _shouldRefreshPlayers;
        
        public void SetOwnerMyPlayerController(MyPlayerController myPlayerController)
        {
            Debug.Log($"new Client in the scene, ID: {myPlayerController.OwnerId}");
            OwnerMyPlayerController = myPlayerController;
            
            if (!_players.Contains(myPlayerController))
            {
                _players.Add(myPlayerController);
            }
        }
        
        private void RefreshPlayers()
        {
            if (OwnerMyPlayerController==null)
                return;
            
            // update the list
            _players.Clear();
            _players.AddRange(FindObjectsOfType<MyPlayerController>());
            
            // check for the Owner MyPlayerController
            if (!_players.Contains(OwnerMyPlayerController))
            {
                Debug.LogError("Owner MyPlayerController missing in the list!!!");
                return;
            }

            // check and if necessary set an admin
            if (!_players.Any(player => player.GetIsAdmin()))
            {
                MyPlayerController pc = _players.FirstOrDefault();
                if (pc != null)
                {
                    pc.SetIsAdmin(true);
                }
            }
        }
        
        public void SetActiveSelf()
        {
            while (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        
        public void SetShouldRefreshPlayers(bool status)
        {
            _shouldRefreshPlayers = status;
        }

        private void Update()
        {
            // RefreshPlayers();
        }
    }
}