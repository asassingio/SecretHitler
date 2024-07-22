using System;
using Enums;
using Managers.CanvasManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameView : View
{
	
	[Header("Main GUI")]
	[SerializeField] private Button teamButton;
	[SerializeField] private Button deckCardsCountButton;
	[SerializeField] private TextMeshProUGUI cardsCountNamesText;
	[SerializeField] private TextMeshProUGUI cardsCountText;
	[SerializeField] private Button closeMenuButton;
	
	[Header("Admin GUI")]
	[SerializeField] private GameObject adminSettings;
	[SerializeField] private TMP_Dropdown newMenuDropdown;
	[SerializeField] private Button confirmNewMenuButton;
	[SerializeField] private Button endGameButton;

	/// <summary>
	/// Called ONE time when the gameManager is fully functional
	/// Sets all the buttons and show the admin menu to the admin
	/// </summary>
	public override void Initialize()
	{
		// Show memeMenu only to the client
		closeMenuButton.onClick.AddListener(() =>
		{
			Debug.Log("closeMenuButton: pressed");
			MenuManager.Instance.Show<GameViewMemeMenu>();
		});
		
		// Show deckMenu only to the client
		deckCardsCountButton.onClick.AddListener(() => MenuManager.Instance.Show<GameViewDeckMenu>());
		
		// Show teamMenu only to the client
		teamButton.onClick.AddListener(() => MenuManager.Instance.Show<GameViewTeamMenu>());
		
		// End the game for all clients
		endGameButton.onClick.AddListener(() => ViewManager.Instance.ShowToAll(AllViewListEnum.StartGameView));
		
		// Admin show a new menu for himself, one client or all clients
		confirmNewMenuButton.onClick.AddListener(ShowMenuByAdminChoice);
		
		base.Initialize();
	}

	/// <summary>
	/// Shows a menu based on the admin's choice.
	/// </summary>
	/// <param name="newMenuDropdown">The dropdown menu to select the desired menu.</param>
	private void ShowMenuByAdminChoice()
	{
		AdminMenuListEnum selectedMenu = (AdminMenuListEnum)Enum.Parse(typeof(AdminMenuListEnum), newMenuDropdown.captionText.text);
		
		try
		{
			MenuManager.Instance.ShowToAll((AllMenuListEnum)selectedMenu);
		}
		catch (Exception e)
		{
			Debug.LogError($"Enum cast failed: {e}");
			throw;
		}
	}
	
	/// <summary>
	/// Show the updated Card count names
	/// </summary>
	public void UpdateCardCountNames(string newCardNames)
	{
		Debug.Log($"updating card names with: {newCardNames}");
		cardsCountNamesText.text = newCardNames;
	}
	
	/// <summary>
	/// Show the updated Card count
	/// </summary>
	public void UpdateCardCount(string newCardCount)
	{
		Debug.Log($"updating card count with: {newCardCount}");
		cardsCountText.text = newCardCount;
	}

	public override void Show()
	{
		Utils.ActiveIfAdmin(adminSettings, OwnerPlayerController, name);
		
		OwnerPlayerController.SetIsInGame(true);
		
		MenuManager.Instance.ShowToAll(AllMenuListEnum.NuovoComputoCarte);
		MenuManager.Instance.ShowToAll(AllMenuListEnum.PrimoGiocatore);
		
		base.Show();
	}
}
