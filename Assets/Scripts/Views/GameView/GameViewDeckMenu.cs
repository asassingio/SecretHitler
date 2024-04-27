using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers.CanvasManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameViewDeckMenu : Menu
{
	[Header("Deck Menu GUI")]
	[SerializeField] private TMP_Dropdown card1Dropdown;
	[SerializeField] private TMP_Dropdown card2Dropdown;
	[SerializeField] private TMP_Dropdown card3Dropdown;
	[SerializeField] private Button confirmButton;
	[SerializeField] private TMP_Dropdown cardSkipDropdown;
	[SerializeField] private Button confirmSkipButton;
	
	[Header("GameView reference")]
	[SerializeField] private GameView gameView;
	
	// holds all the CardTypeEnum and their count
	private readonly Dictionary<CardTypeEnum, int> _cardsInDeck = new Dictionary<CardTypeEnum, int>();
	private readonly Dictionary<CardTypeEnum, int> _cardsInGame = new Dictionary<CardTypeEnum, int>();
	private readonly Dictionary<CardTypeEnum, int> _cardsInDiscardPile = new Dictionary<CardTypeEnum, int>();
	
	// strings that will be printed
	private string _cardNamesString;
	private string _cardValuesString;
	
	public override void Initialize()
	{
		// Add the vote to the log
		confirmButton.onClick.AddListener(AddNewCards);
		
		// Add the skip to the log
		confirmSkipButton.onClick.AddListener(SkipCard);
		
		base.Initialize();
	}

	private void SkipCard()
	{
		CardTypeEnum cardToSkip = (CardTypeEnum) Enum.Parse(typeof(CardTypeEnum), cardSkipDropdown.captionText.text);
		MenuManager.Instance.ServerUpdateCardsWithSkipCardGui(cardToSkip);
	}

	/// <summary>
	/// Generate a deck of cards and send them to the clients
	/// </summary>
	private void GenerateDeck()
	{
		_cardsInDeck[CardTypeEnum.Fascista] = 11;
		_cardsInGame[CardTypeEnum.Fascista] = 0;
		_cardsInDiscardPile[CardTypeEnum.Fascista] = 0;
		
		_cardsInDeck[CardTypeEnum.Liberale] = 6;
		_cardsInGame[CardTypeEnum.Liberale] = 0;
		_cardsInDiscardPile[CardTypeEnum.Liberale] = 0;
		
		// add or remove economic cards
		SetValueInDropdown dropdownScript1 = card1Dropdown.gameObject.GetComponent<SetValueInDropdown>();
		SetValueInDropdown dropdownScript2 = card2Dropdown.gameObject.GetComponent<SetValueInDropdown>();
		SetValueInDropdown dropdownScript3 = card3Dropdown.gameObject.GetComponent<SetValueInDropdown>();
		SetValueInDropdown cardSkiKDropdownScript = cardSkipDropdown.gameObject.GetComponent<SetValueInDropdown>();
		string bankerCardType = SecretRoleEnum.Banchiere.ToString();
		
		int bankerPlayerCount = GameManager.Instance.GetPlayersNameWithSameRole(SecretRoleEnum.Banchiere).Count;
		Debug.LogError($"i banchieri sono: {bankerPlayerCount}");
		switch (bankerPlayerCount)
		{
			case 0: // remove the Banker cards form the dropdowns and from the dictionary
				if (dropdownScript1.IsValueInDropdown(bankerCardType))
					dropdownScript1.RemoveDropdownValue(bankerCardType);
				
				if (dropdownScript2.IsValueInDropdown(bankerCardType))
					dropdownScript2.RemoveDropdownValue(bankerCardType);
				
				if (dropdownScript3.IsValueInDropdown(bankerCardType))
					dropdownScript3.RemoveDropdownValue(bankerCardType);
				
				if (cardSkiKDropdownScript.IsValueInDropdown(bankerCardType))
					cardSkiKDropdownScript.RemoveDropdownValue(bankerCardType);
				
				if (_cardsInDeck.ContainsKey(CardTypeEnum.Economica))
					_cardsInDeck.Remove(CardTypeEnum.Economica);
				
				if (_cardsInGame.ContainsKey(CardTypeEnum.Economica))
					_cardsInGame.Remove(CardTypeEnum.Economica);
				
				if (_cardsInDiscardPile.ContainsKey(CardTypeEnum.Economica))
					_cardsInDiscardPile.Remove(CardTypeEnum.Economica);
				break;
			
			case 1: // add the Banker cards in the dropdowns and in the dictionary
				if (!dropdownScript1.IsValueInDropdown(bankerCardType))
					dropdownScript1.AddDropdownValue(bankerCardType);
				
				if (!dropdownScript2.IsValueInDropdown(bankerCardType))
					dropdownScript2.AddDropdownValue(bankerCardType);
				
				if (!dropdownScript3.IsValueInDropdown(bankerCardType))
					dropdownScript3.AddDropdownValue(bankerCardType);
				
				if (!cardSkiKDropdownScript.IsValueInDropdown(bankerCardType))
					cardSkiKDropdownScript.AddDropdownValue(bankerCardType);
				
				_cardsInDeck[CardTypeEnum.Economica] = 3;
				_cardsInGame[CardTypeEnum.Economica] = 0;
				_cardsInDiscardPile[CardTypeEnum.Economica] = 0;
				break;
			
			case > 1:
				Debug.LogError("cazzo fai");
				throw new Exception("Unexpected number of bankers.");
		}
		
		FormatCards();
		UpdateCardsGui();
	}
	
	
	
	/// <summary>
	/// Adds new cards to the game and send them to all clients
	/// </summary>
	private void AddNewCards()
	{
		CardTypeEnum card1 = (CardTypeEnum) Enum.Parse(typeof(CardTypeEnum), card1Dropdown.captionText.text);
		CardTypeEnum card2 = (CardTypeEnum) Enum.Parse(typeof(CardTypeEnum), card2Dropdown.captionText.text);
		CardTypeEnum card3 = (CardTypeEnum) Enum.Parse(typeof(CardTypeEnum), card3Dropdown.captionText.text);
		MenuManager.Instance.ServerUpdateCardsGui(card1, card2, card3);
	}
	
	public void UpdateCardsGui(CardTypeEnum card1, CardTypeEnum card2, CardTypeEnum card3)
	{
		AddNewCardsToDictionary(card1, card2, card3);
		if (_cardsInDeck.Values.Sum() < 3)
		{
			ShuffleDeck();
		}

		FormatCards();
		UpdateCardsGui();
	}
	
	/// <summary>
	/// Add 3 new cards to the dictionaries
	/// </summary>
	private void AddNewCardsToDictionary(CardTypeEnum card1, CardTypeEnum card2, CardTypeEnum card3)
	{
		// add the new cards to the dictionaries
		if(_cardsInDeck.ContainsKey(card1) && _cardsInGame.ContainsKey(card1) && _cardsInDiscardPile.ContainsKey(card1))
		{
			_cardsInDeck[card1]--;
			_cardsInGame[card1]++;
		}
		
		if(_cardsInDeck.ContainsKey(card2) && _cardsInGame.ContainsKey(card2) && _cardsInDiscardPile.ContainsKey(card2))
		{
			_cardsInDeck[card2]--;
			_cardsInDiscardPile[card2]++;
		}
		
		if(_cardsInDeck.ContainsKey(card3) && _cardsInGame.ContainsKey(card3) && _cardsInDiscardPile.ContainsKey(card3))
		{
			_cardsInDeck[card3]--;
			_cardsInDiscardPile[card3]++;
		}
	}
	
	/// <summary>
	/// If the are less than 3 cards, Shuffle the Draw Deck
	/// </summary>
	private void ShuffleDeck()
	{
		foreach (CardTypeEnum key in _cardsInDeck.Keys.ToList())
		{
			_cardsInDeck[key] += _cardsInDiscardPile[key];
			_cardsInDiscardPile[key] = 0;
		}
	}
	
	
	
	
	/// <summary>
	/// Format two strings with the Deck info
	/// </summary>
	private void FormatCards()
	{
		// generate the two strings that will be printed
		_cardNamesString = "Deck:\n";
		_cardValuesString = $"{_cardsInDeck.Values.Sum()}\n";
		// string dictionaryContent = "";
		foreach (KeyValuePair<CardTypeEnum, int> entry in _cardsInDeck)
		{
			_cardNamesString += $"{entry.Key}\n";
			_cardValuesString += $"{entry.Value}\n";
			// dictionaryContent += $"{entry.Key}:\t{entry.Value}\n";
		}
		Debug.Log(string.Join(", ", _cardsInDeck.Select(kv => kv.Key + ": " + kv.Value.ToString()).ToArray()));
		// Debug.Log($"{_cardNamesString} \n{_cardValuesString}");
	}
	
	/// <summary>
	/// Updates the GUI with the current formatted strings: cardNames and cardValues
	/// </summary>
	private void UpdateCardsGui()
	{
		gameView.UpdateCardCountNames(_cardNamesString);
		gameView.UpdateCardCount(_cardValuesString);
	}

	protected override void ResetAllVar()
	{
		_cardNamesString = "";
		_cardValuesString = "";
		base.ResetAllVar();
	}

	public override void Show()
	{
		if (GameManager.Instance.GetShouldGenerateNewDeck())
		{
			GenerateDeck();
		}
		
		base.Show();
	}

	public void UpdateCardsWithSkipCardGui(CardTypeEnum cardToSkip)
	{
		if(_cardsInDeck.ContainsKey(cardToSkip) && _cardsInGame.ContainsKey(cardToSkip) && _cardsInDiscardPile.ContainsKey(cardToSkip))
		{
			_cardsInDeck[cardToSkip]--;
			_cardsInGame[cardToSkip]++;
			
			if (_cardsInDeck.Values.Sum() < 3)
			{
				ShuffleDeck();
			}
			
			FormatCards();
			UpdateCardsGui();
		}
	}
}