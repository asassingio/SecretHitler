using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Enums;
using FishNet;
using FishNet.Managing.Server;
using FishNet.Object;
using Managers.CanvasManager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using static Enums.GameModeEnum;
using static Enums.SecretRoleEnum;

public sealed class SetPlayerView : View
{
	[Header("Main GUI")]
	[SerializeField] private TMP_InputField playerNameInputField;
	[SerializeField] private TMP_Dropdown secretRoleDropdown;
	[SerializeField] private Button readyGameButton;
	[SerializeField] private TextMeshProUGUI waitServerIsLoadingText;

	private bool _isServerReady;

	public override void Initialize()
	{
		// salva tutte le variabili del giocatore
		readyGameButton.onClick.AddListener(SetImReady);
		
		base.Initialize();
	}

	private void SetImReady()
	{
		// se playerName e secretRole sono stati scelti il player e' pronto
		if ((!string.IsNullOrEmpty(playerNameInputField.text.Trim())) && (!string.IsNullOrEmpty(secretRoleDropdown.captionText.text.Trim())))
		{
			OwnerPlayerController.ServerSetName(playerNameInputField.text);
			OwnerPlayerController.ServerSetRole(Enum.Parse<SecretRoleEnum>(secretRoleDropdown.captionText.text));
			OwnerPlayerController.ServerSetIsReady(true);
			
			ViewManager.Instance.Show<StartGameView>();
		}
	}

	private void Update()
	{
		if (_isServerReady) return;
		
		// aspetta che il game manager sia pronto
		if (!GameManager.Instance.isInitialized)
		{
			waitServerIsLoadingText.text = "In attesa che il server ti carichi";
		}
		else
		{
			waitServerIsLoadingText.text = "Il server ti ha caricato";
			_isServerReady = true;
		}
	}
	
	public override void Show()
	{
		OwnerPlayerController.ServerSetIsInGame(false);
		
		base.Show();
	}
}