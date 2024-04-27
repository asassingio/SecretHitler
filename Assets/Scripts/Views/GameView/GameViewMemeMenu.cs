using System.Collections.Generic;
using GameKit.Utilities;
using TMPro;
using UnityEngine;


public sealed class GameViewMemeMenu : Menu
{
	[Header("Meme Menu GUI")]
	[SerializeField] private TextMeshProUGUI memeText;
	
	private List<string> memeList = new List<string> {
		"Esotico", 
		"Chiara fascista Federico Hitler", 
		"Nel dubbio spara ad Alessio", 
		"Lorem ipsum palle"
	};
	
	/// <summary>
	/// Show each time a different meme
	/// </summary>
	public override void Show()
	{
		memeList.Shuffle();
		memeText.text = memeList[0];
		base.Show();
	}

	protected override void ResetAllVar()
	{
		memeText.text = "";
		base.ResetAllVar();
	}
}