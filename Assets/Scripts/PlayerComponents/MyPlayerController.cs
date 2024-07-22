using System;
using Enums;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Managers.PersistentManagers;
using UnityEngine;

namespace PlayerComponents
{
	public sealed class MyPlayerController : NetworkBehaviour
	{
		private Camera _playerCamera;
		
		[field: SerializeField] private readonly SyncVar<string> _playerName = 
			new SyncVar<string>(new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner));
	
		[field: SerializeField] private readonly SyncVar<SecretRoleEnum> _secretRole = 
			new SyncVar<SecretRoleEnum>(new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner));
	
		[field: SerializeField] private readonly SyncVar<bool> _isReady = 
			new SyncVar<bool>(new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner));
	
		[field: SerializeField] private readonly SyncVar<bool> _isAdmin = 
			new SyncVar<bool>(new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner));
	
		[field: SerializeField] private readonly SyncVar<bool> _isInGame = 
			new SyncVar<bool>(new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner));
		
		public string GetPlayerName() => _playerName.Value;
		[ServerRpc(RunLocally = true)] public void SetPlayerName(string value) => _playerName.Value = value;
		
		public SecretRoleEnum GetSecretRole() => _secretRole.Value;
		[ServerRpc(RunLocally = true)] public void SetSecretRole(SecretRoleEnum role) => _secretRole.Value = role;
		
		public bool GetIsReady() => _isReady.Value;
		[ServerRpc(RunLocally = true)] public void SetIsReady(bool status) => _isReady.Value = status;
		
		public bool GetIsAdmin() => _isAdmin.Value;
		[ServerRpc(RunLocally = true)] public void SetIsAdmin(bool status) => _isAdmin.Value = status;
		
		public bool GetIsInGame() => _isInGame.Value;
		[ServerRpc(RunLocally = true)] public void SetIsInGame(bool status) => _isInGame.Value = status;
		
		
		public override void OnStartClient()
		{
			// base.OnStartClient e base.OnStartServer vanno SEMPRE chiamati per primi, o il mondo finira'
			base.OnStartClient();

			if (!IsOwner)
			{
				gameObject.GetComponent<MyPlayerController>().enabled = false;
				return;
			}
			
			_playerCamera = Camera.main;
		
			if (NetworkManager.ClientManager.Clients.Count > 1)
			{
				SetIsAdmin(true);
			
				// change it few frames later
				// ServerSetIsAdmin(true);
			}

			if (FindObjectOfType<PlayerManager>() == null)
				throw new Exception($"PlayerManager found? {FindObjectOfType<PlayerManager>() != null}");
			
			FindObjectOfType<PlayerManager>().SetOwnerMyPlayerController(this);
		}
	}
}