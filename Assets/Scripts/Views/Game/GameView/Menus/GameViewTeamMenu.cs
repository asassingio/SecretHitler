using System;
using System.Collections.Generic;
using Enums;
using Managers;
using TMPro;
using UnityEngine;

public sealed class GameViewTeamMenu : Menu
{
	[Header("Team Menu GUI")]
	[SerializeField] private TextMeshProUGUI teamMembersText;
	
	public override void Show()
	{
		SecretRoleEnum ownerRole = OwnerPlayerController.GetSecretRole();
		switch (ownerRole)
		{
			case SecretRoleEnum.Fascista:
				List<string> names = GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Fascista);
				List<string> names2 = GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Hitler);
				
				string finalList = string.Join(Environment.NewLine, names);
				finalList += string.Join(Environment.NewLine, names2);
				teamMembersText.text = finalList;
				break;
			default:
				teamMembersText.text = "La tua squadra e': sicuramente non Massimo. \nNon sei fascista non vedrai nulla";
				break;
		}
		base.Show();
	}

	protected override void ResetAllVar()
	{
		teamMembersText.text = "";
		base.ResetAllVar();
	}
}