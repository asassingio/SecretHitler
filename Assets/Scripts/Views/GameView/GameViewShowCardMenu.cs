using System.Collections.Generic;
using Enums;
using Managers.CanvasManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class GameViewShowCardMenu : Menu
{
	[Header("Show PlayerController Card Menu GUI")]
	[SerializeField] private TextMeshProUGUI waitAdminText;
	[SerializeField] private TextMeshProUGUI playerCardText;
	
	[Header("Admin GUI")]
	[SerializeField] private GameObject adminSettings;
	[SerializeField] private TMP_Dropdown playerThatWillSeeDropdown;
	[SerializeField] private TMP_Dropdown playerToSeeDropdown;
	[SerializeField] private Button confirmButton;
	
	
	public override void Initialize()
	{
		// Confirm who will see who
		confirmButton.onClick.AddListener(() => ShowCardToTarget());
		
		base.Initialize();
	}


	/// <summary>
	/// Update the Dropdowns with the current players
	/// </summary>
	public override void Show()
	{
		Utils.ActiveIfAdmin(adminSettings, OwnerPlayerController, name);
		
		// get a list of all the alive clients
		List<string> playersName = GameManager.Instance.GetPlayersName();
		
		// Set all Names to the playerThatWillSeeDropdown;
		playerThatWillSeeDropdown.ClearOptions();
		foreach (string pName in playersName) { playerThatWillSeeDropdown.options.Add(new TMP_Dropdown.OptionData(pName)); }
		playerThatWillSeeDropdown.RefreshShownValue();
		
		// Set all Names to the playerToSeeDropdown
		playerToSeeDropdown.ClearOptions();
		foreach (string pName in playersName) { playerToSeeDropdown.options.Add(new TMP_Dropdown.OptionData(pName)); }
		playerToSeeDropdown.RefreshShownValue();

		waitAdminText.text = "Aspettando L'admin";
		
		base.Show();
	}

	/// <summary>
	/// Shows the role card to a target player. if the role is Hitler or Banchiere change it to their faction 
	/// </summary>
	private void ShowCardToTarget()
	{
		PlayerController playerControllerThatWillSee = GameManager.Instance.GetPlayerByName(playerThatWillSeeDropdown.captionText.text);
		PlayerController playerControllerToSee = GameManager.Instance.GetPlayerByName(playerToSeeDropdown.captionText.text);
		string playerToSeeName = playerControllerToSee.ServerGetName();

		SecretRoleEnum cardToShow;
		switch (playerControllerToSee.ServerGetRole())
		{
			case SecretRoleEnum.Hitler:
				cardToShow = SecretRoleEnum.Fascista;	
				break;
			case SecretRoleEnum.Banchiere:
				cardToShow = SecretRoleEnum.Liberale;
				break;
			default:
				cardToShow = playerControllerToSee.ServerGetRole();
				break;
		}
		MenuManager.Instance.ServerShowCardToTarget(playerControllerThatWillSee.Owner, playerToSeeName, cardToShow);
	}

	public void ShowCardToTarget(string pName, SecretRoleEnum sRole)
	{
		Debug.LogWarning("GameViewShowCardMenu: RpcShowCardToTarget (target)");
		playerCardText.text = $"{pName} e' {sRole}";
	}
	
	public void ShowCardToTargetLog()
	{
		waitAdminText.text = "Il giocatore ha visto la tessera di partito";
	}

	protected override void ResetAllVar()
	{
		waitAdminText.text = "";
		playerCardText.text = "";
		base.ResetAllVar();
	}
}