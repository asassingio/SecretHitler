using System;
using System.Collections.Generic;
using Enums;
using Managers.CanvasManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameViewElectionMenu : Menu
{
	[Header("Election Menu GUI")]
	[SerializeField] private Button yesButton;
	[SerializeField] private Button noButton;
	[SerializeField] private TextMeshProUGUI voteLog;
	[SerializeField] private TextMeshProUGUI voteResult;

	private readonly List<string> _playersVoteLog = new List<string>();
	private bool _isFirstVote = true;
	private int _voteCount;
	private int _yesCount;
	private int _noCount;
	
	
	public override void Initialize()
	{
		// Add the vote to the log
		yesButton.onClick.AddListener(() => SetVote(true));
		
		// Add the vote to the log
		noButton.onClick.AddListener(() => SetVote(false));
		
		base.Initialize();
	}
	
	/// <summary>
	/// Add the new vote in the voteLog for all clients
	/// and show the election result if all had voted.
	/// Return if the client it's trying to vote more than once
	/// </summary>
	/// <param name="vote">The vote. True for "Yes", False for "No".</param>
	private void SetVote(bool vote)
	{
		if (!_isFirstVote) return;

		if (vote)
		{
			_yesCount++;
		}
		else
		{
			_noCount++;
		}
		
		switch (GameManager.Instance.ServerGetGameMode())
		{
			case GameModeEnum.VotoPubblico:
			{
				string playerVoteLog = $"{OwnerPlayerController.ServerGetName()} ha votato: {(vote ? "SI" : "NO")}";
				_isFirstVote = false;
				Debug.LogWarning($"SetVoteLocal: playerVoteLog={playerVoteLog} _yesCount={_yesCount} _noCount={_noCount}");
				MenuManager.Instance.ServerAddVoteLog(playerVoteLog, _yesCount, _noCount);
				break;
			}
			case GameModeEnum.VotoSegreto:
			{
				string playerVoteLog = $"{OwnerPlayerController.ServerGetName()} ha votato";
				_isFirstVote = false;
				Debug.LogWarning($"SetVoteLocal: playerVoteLog={playerVoteLog} _yesCount={_yesCount} _noCount={_noCount}");
				MenuManager.Instance.ServerAddVoteLog(playerVoteLog, _yesCount, _noCount);
				break;
			}
		}
	}
	
	public void AddVoteLog(string playerVoteLog, int yesCount, int noCount)
	{
		_playersVoteLog.Add(playerVoteLog);
		_voteCount++;
		_yesCount = yesCount;
		_noCount = noCount;
		
		// stampa a schermo i voti di tutti i giocatori
		string fullVoteLog = string.Join(Environment.NewLine, _playersVoteLog);
		voteLog.text = fullVoteLog;
		
		Debug.LogWarning($"RpcAddVoteLog: {playerVoteLog} \n \n{fullVoteLog}");
		
		// stampa a schermo il risultato della votazione
		if (GameManager.Instance.GetInGamePlayersCount() == _voteCount)
		{
			if (_yesCount > _noCount)
			{
				voteResult.text = "SI";
			}
			else if (_yesCount < _noCount)
			{
				voteResult.text = "NO";
			}
			else if (_yesCount == _noCount)
			{
				voteResult.text = "PAREGGIO";
			}
		}
	}

	protected override void ResetAllVar()
	{
		_playersVoteLog.Clear();
		_isFirstVote = true;
		_voteCount = 0;
		_yesCount = 0;
		_noCount = 0;
		voteLog.text = "";
		voteResult.text = "";
		
		base.ResetAllVar();
	}
	
}
