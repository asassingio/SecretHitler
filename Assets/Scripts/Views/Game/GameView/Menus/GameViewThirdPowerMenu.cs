using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Enums;
using Managers;
using Managers.CanvasManagers;
using PlayerComponents;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public sealed class GameViewThirdPowerMenu : Menu
{
	[Header("Third power of the banker Menu GUI")]
	[SerializeField] private TMP_Dropdown playerListDropdown;
	[SerializeField] private Button confirmButton;
	[SerializeField] private TextMeshProUGUI voteCountText;
	[SerializeField] private TextMeshProUGUI deadPlayerText;

	private readonly Dictionary<string, int> _votedPlayer = new Dictionary<string, int>();
	private bool _hasAlreadyVoted;
	
	public override void Initialize()
	{
		// Confirm kill
		confirmButton.onClick.AddListener(() =>
		{
			bool b = IsAllowedToVote();
			Debug.LogError($"IsAllowedToVote: {b}");
			if (b)
			{
				MenuManager.Instance.ServerVoteToKillTarget(playerListDropdown.options[playerListDropdown.value].text);
			}
		});
		
		base.Initialize();
	}

	private bool IsAllowedToVote()
	{
		if (_hasAlreadyVoted) return false;
		
		SecretRoleEnum pRole = OwnerPlayerController.GetSecretRole();
		if (pRole != SecretRoleEnum.Liberale && pRole != SecretRoleEnum.Banchiere)
		{
			return false;
		}

		_hasAlreadyVoted = true;
		return true;
	}
	
	public override void Show()
	{
		// get a list of all the alive clients
		List<string> playersName = GameManager.Instance.GetInGamePlayersName();
		
		// Set all Names to the playerThatWillSeeDropdown;
		playerListDropdown.ClearOptions();
		foreach (string pName in playersName) { playerListDropdown.options.Add(new TMP_Dropdown.OptionData(pName)); }
		playerListDropdown.RefreshShownValue();

		int voteCount = 0;
		voteCount += GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Liberale).Count;
		voteCount += GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Banchiere).Count;
		voteCountText.text = $"Voti mancanti: {voteCount}";
		
		base.Show();
	}

	public void VoteToKillTarget(string votedPlayerName)
	{
		// if it's a new name (new field for the dictionary) add it and initialize it to 0
		if (!_votedPlayer.ContainsKey(votedPlayerName))
		{
			_votedPlayer.Add(votedPlayerName, 0);
		}
		
		_votedPlayer[votedPlayerName] += 1;
		
		int voteCount = 0;
		voteCount += GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Liberale).Count;
		voteCount += GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Banchiere).Count;
		voteCountText.text = $"Voti mancanti: {voteCount-_votedPlayer.Values.Sum()}";
		
		Debug.LogError($"{voteCount} {_votedPlayer.Values.Sum()}");
		
		if (voteCount == _votedPlayer.Values.Sum())
		{
			int maxVotes = _votedPlayer.Values.Max();
			
			List<string> maxVotedPlayers = 
				_votedPlayer.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key).ToList();
			
			string maxVotedPlayersString = string.Join(", ", maxVotedPlayers);
			deadPlayerText.text = $"{maxVotedPlayersString} E' stato eliminato";

			foreach (string playerName in maxVotedPlayers)
			{
				MyPlayerController player = GameManager.Instance.GetPlayerByName(playerName);
				GameManager.Instance.ServerKillTarget(player.Owner);
			}
		}
	}
	
	protected override void ResetAllVar()
	{
		_hasAlreadyVoted = false;
		_votedPlayer.Clear();
		voteCountText.text = "";
		deadPlayerText.text = "";
		base.ResetAllVar();
	}
}