using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using FishNet.Managing;
using FishNet.Transporting.Tugboat.Client;
using Managers.CanvasManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class StartGameView : View
{
	[Header("Main GUI")]
	[SerializeField] private TextMeshProUGUI readyPlayerlistText;
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button goBackButton;

	[Header("Admin GUI")]
	[SerializeField] private GameObject adminSettings;
	[SerializeField] private Button playIntroMusicButton;
	[SerializeField] private Button playIntroMacimoButton;
	[SerializeField] private TMP_Dropdown voteTypeDropdown;
	
	[Header("Audio Clip")] 
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip song;
	[SerializeField] private AudioClip introMacimo;
	
	public override void Initialize()
	{
		// by default the GameMode will be VotoPubblico
		GameManager.Instance.ServerSetGameMode(GameModeEnum.VotoPubblico);
		voteTypeDropdown.onValueChanged.AddListener(arg0 =>
		{
			GameModeEnum gameMode = (GameModeEnum)Enum.Parse(typeof(GameModeEnum), voteTypeDropdown.captionText.text);
			GameManager.Instance.ServerSetGameMode(gameMode);
		});
		
		// passa alla schermata successiva
		startGameButton.onClick.AddListener(() =>
		{
			Debug.LogError(GameManager.Instance.GetClientsCount());
			if (GameManager.Instance.GetInGamePlayersCount() == 0)
			{
				if (GameManager.Instance.GetIsReadyPlayersCount() == GameManager.Instance.GetClientsCount())
				{
					// stop the music
					audioSource.Stop();
					
					GameManager.Instance.ServerSetShouldGenerateNewDeck(true);
					ViewManager.Instance.ShowToAll(AllViewListEnum.GameView);
				}
			}
			else
			{
				Debug.LogWarning("ci sono ancora giocatori in partita!");
			}
		});
		
		// passa alla schermata precendente
		goBackButton.onClick.AddListener(() =>
		{
			OwnerPlayerController.ServerSetName("");
			OwnerPlayerController.ServerSetIsReady(false);
			
			// stop the music
			audioSource.Stop();
			
			ViewManager.Instance.Show<SetPlayerView>();
		});
		
		// play the intro music
		playIntroMusicButton.onClick.AddListener(() =>
		{
			Utils.PlaySound(song, audioSource);
		});
		
		// play the intro Macimo music
		playIntroMacimoButton.onClick.AddListener(() =>
		{
			Utils.PlaySound(introMacimo, audioSource);
		});
		
		base.Initialize();
	}
	
	private void Update()
	{
		if (!IsInitialized) return; // if the view it's not initialized return avoiding errors
		
		// Show on the screen a list with all players
		List<string> names = GameManager.Instance.GetPlayersName();
		readyPlayerlistText.text = string.Join("\n", names.Select((item, index) => $"{index + 1}. {item}"));
	}

	public override void Show()
	{
		Utils.ActiveIfAdmin(adminSettings, OwnerPlayerController, name);
		
		OwnerPlayerController.ServerSetIsInGame(false);
		
		base.Show();
	}
}