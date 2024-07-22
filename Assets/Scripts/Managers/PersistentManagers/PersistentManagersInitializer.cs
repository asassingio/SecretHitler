using System;
using FishNet.Object;
using UnityEngine;

namespace Managers.PersistentManagers
{
    public class PersistentManagersInitializer : MonoBehaviour
    {
        [Header("Managers")] 
        [SerializeField] private NetworkObject bootstrapSceneManager;
        [SerializeField] private NetworkObject playerManager;
        [SerializeField] private NetworkObject inactivityHandler;

        private void Awake()
        {
            Instantiate(bootstrapSceneManager);
            Instantiate(playerManager);
            Instantiate(inactivityHandler);

            // Debug.Log($"{BootstrapSceneManager.Instance.gameObject.activeSelf} {BootstrapSceneManager.Instance.IsInitialized}");
            // Debug.Log($"{PlayerManager.Instance.gameObject.activeSelf} {PlayerManager.Instance.IsInitialized}");
            // Debug.Log($"{InactivityHandler.Instance.gameObject.activeSelf} {InactivityHandler.Instance.IsInitialized}");
        }

        private void Update()
        {
            if (!BootstrapSceneManager.Instance.gameObject.activeSelf)
                BootstrapSceneManager.Instance.SetActiveSelf();
            
            if (!PlayerManager.Instance.gameObject.activeSelf)
                PlayerManager.Instance.SetActiveSelf();
            
            if (!InactivityHandler.Instance.gameObject.activeSelf)
                InactivityHandler.Instance.SetActiveSelf();
        }
    }
}
