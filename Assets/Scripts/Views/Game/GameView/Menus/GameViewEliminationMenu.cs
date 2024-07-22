using System.Collections.Generic;
using Managers;
using Managers.CanvasManagers;
using PlayerComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Seen only by the admin. used to remove dead player
/// </summary>
public sealed class GameViewEliminationMenu : Menu
{
	[Header("Elimination Menu GUI")]
	[SerializeField] private TextMeshProUGUI waitAdminText;
	[SerializeField] private TextMeshProUGUI deadPlayerText;
	
	[Header("Admin GUI")]
	[SerializeField] private GameObject adminSettings;
	[SerializeField] private TMP_Dropdown playerThatDieDropdown;
	[SerializeField] private Button confirmButton;

	public override void Initialize()
	{
		// Confirm kill
		confirmButton.onClick.AddListener(() => KillTarget());
		
		base.Initialize();
	}

	/// <summary>
	/// Update the Dropdown with the current players
	/// </summary>
	public override void Show()
	{
		Utils.ActiveIfAdmin(adminSettings, OwnerPlayerController, name);
		
		// get a list of all the alive clients
		List<string> playersName = GameManager.Instance.GetInGamePlayersName();
		
		// Set all Names to the playerThatWillSeeDropdown;
		playerThatDieDropdown.ClearOptions();
		foreach (string pName in playersName) { playerThatDieDropdown.options.Add(new TMP_Dropdown.OptionData(pName)); }
		playerThatDieDropdown.RefreshShownValue();
		
		waitAdminText.text = "Aspettando L'admin";
		
		base.Show();
	}

	/// <summary>
	/// Kills the target and removes it form the gameManager and game
	/// </summary>
	private void KillTarget()
	{
		MyPlayerController playerControllerThatWillDie = GameManager.Instance.GetPlayerByName(playerThatDieDropdown.captionText.text);
		string playerThatWillDieName = playerControllerThatWillDie.GetPlayerName();
		
		GameManager.Instance.ServerKillTarget(playerControllerThatWillDie.Owner);
		MenuManager.Instance.ServerKillTargetLog(playerThatWillDieName);
	}
	
	public void KillTargetLog(string playerThatWillDieName)
	{
		waitAdminText.text = "";
		deadPlayerText.text = $"Shock! {playerThatWillDieName} e' stato ucciso";
	}

	protected override void ResetAllVar()
	{
		waitAdminText.text = "";
		deadPlayerText.text = "";
		base.ResetAllVar();
	}
}