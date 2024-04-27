using Enums;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Managers.CanvasManager;
using UnityEngine;

public sealed class PlayerController : NetworkBehaviour
{
	private Camera _playerCamera;

	[SerializeField] [SyncVar] private string playerName;
	[SerializeField] [SyncVar] private SecretRoleEnum secretRole;
	[SerializeField] [SyncVar] private bool isReady;
	[SerializeField] [SyncVar] private bool isAdmin;
	[SerializeField] [SyncVar] private bool isInGame;

	// tutti i metodi get e set
	public string ServerGetName() { return this.playerName; }
	[ServerRpc] public void ServerSetName(string playerName) { this.playerName = playerName; }
	
	public SecretRoleEnum ServerGetRole() { return this.secretRole; }
	[ServerRpc] public void ServerSetRole(SecretRoleEnum secretRole)
	{
		this.secretRole = secretRole;
	}

	public bool ServerGetIsReady()
	{
		return this.isReady;
	}
	[ServerRpc] public void ServerSetIsReady(bool isReady)
	{
		this.isReady = isReady;
	}
	
	public bool ServerGetIsAdmin()
	{
		return this.isAdmin;
	}
	[ServerRpc] public void ServerSetIsAdmin(bool isAdmin)
	{
		this.isAdmin = isAdmin;
	}
	
	public bool ServerGetIsInGame()
	{
		return this.isInGame;
	}
	[ServerRpc] public void ServerSetIsInGame(bool isInGame)
	{
		this.isInGame = isInGame;
	}

	public override void OnStartClient()
	{
		// base.OnStartClient e base.OnStartServer vanno SEMPRE chiamati per primi, o il mondo finira'
		base.OnStartClient();

		if (!IsOwner)
		{
			gameObject.GetComponent<PlayerController>().enabled = false;
			return;
		}
		
		if (NetworkManager.ClientManager.Clients.Count == 1)
		{
			isAdmin = true;
			
			// change it few frames later
			ServerSetIsAdmin(true);
		}

		_playerCamera = Camera.main;
		GameManager.Instance.Initialize(this);
		ViewManager.Instance.Initialize(this);
		MenuManager.Instance.Initialize(this);
	}
}