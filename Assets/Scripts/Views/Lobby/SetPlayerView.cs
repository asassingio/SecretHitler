using System;
using Enums;
using Managers;
using Managers.CanvasManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Lobby
{
	public sealed class SetPlayerView : View
	{
		[Header("Main GUI")]
		[SerializeField] private TMP_InputField playerNameInputField;
		[SerializeField] private TextMeshProUGUI secretRoleText;
		[SerializeField] private RoleDrag roleDrag;
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
			if ((!string.IsNullOrEmpty(playerNameInputField.text.Trim())) && (!string.IsNullOrEmpty(secretRoleText.text.Trim())))
			{
				OwnerPlayerController.SetPlayerName(playerNameInputField.text);
				OwnerPlayerController.SetSecretRole(Enum.Parse<SecretRoleEnum>(secretRoleText.text));
				OwnerPlayerController.SetIsReady(true);
			
				ViewManager.Instance.Show<StartGameView>();
			}
		}

		private void Update()
		{
			if (!string.IsNullOrEmpty(roleDrag.SelectedRole.ToString().Trim()))
				secretRoleText.text = roleDrag.SelectedRole.ToString().Trim();
		
			if (_isServerReady) return;
		
			// aspetta che il game manager sia pronto
			if (!LobbyManager.Instance.IsInitialized)
			{
				waitServerIsLoadingText.text = "In attesa che il server ti carichi";
			}
			else
			{
				waitServerIsLoadingText.text = "";
				_isServerReady = true;
			}
		}
	
		public override void Show()
		{
			OwnerPlayerController.SetIsReady(false);
		
			base.Show();
		}
	
		protected override void ResetAllVar()
		{
			// playerNameInputField.text = "";
			waitServerIsLoadingText.text = "";
			_isServerReady = false;
			base.ResetAllVar();
		}
	}
}