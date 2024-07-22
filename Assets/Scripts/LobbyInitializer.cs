using System;
using FishNet.Object;
using Managers;
using Managers.CanvasManagers;
using Managers.PersistentManagers;
using PlayerComponents;
using UnityEngine;

public class LobbyInitializer : NetworkBehaviour
{
    private MyPlayerController _ownerPlayerController;
    [field: SerializeField] public bool IsInitialized { get; private set; }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _ownerPlayerController = PlayerManager.Instance.OwnerMyPlayerController;
        if (_ownerPlayerController == null)
        {
            Debug.LogError("PlayerSpawnerManager doesn't have an OwnerPlayerController!!!");
            return;
        }
        
        LobbyManager.Instance.ManualInitialize(_ownerPlayerController);
        ViewManager.Instance.ManualInitialize(_ownerPlayerController);
        
        IsInitialized = true;
    }
}
