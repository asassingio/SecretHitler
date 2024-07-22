using Managers;
using Managers.CanvasManagers;
using TMPro;
using UnityEngine;


public sealed class GameViewFirstPlayerMenu : Menu
{
	[Header("First PlayerController Menu GUI")]
	[SerializeField] private TextMeshProUGUI firstPlayerText;
	
	public override void Show()
	{
		GenerateFirstPlayer();
		
		base.Show();
	}
	
	/// <summary>
	/// Generates the first player play the game and shows it to all clients.
	/// </summary>
	private void GenerateFirstPlayer()
	{
		if (GameManager.Instance.GameManagerServerGetIsAdmin(OwnerPlayerController))
		{
			string randomPlayerName = GameManager.Instance.ServerGetRandomPlayer();
			MenuManager.Instance.ServerShowFirstPlayer(randomPlayerName);
		}
	}
	
	public void ShowFirstPlayer(string randomPlayerName)
	{
		firstPlayerText.text = $"Inizia il giocatore: \n{randomPlayerName}";
	}

	protected override void ResetAllVar()
	{
		firstPlayerText.text = "";
		base.ResetAllVar();
	}
}
