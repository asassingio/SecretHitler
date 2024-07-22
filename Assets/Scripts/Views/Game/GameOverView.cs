using Managers.CanvasManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameOverView : View
{
	[Header("Main GUI")]
	[SerializeField] private TextMeshProUGUI deathJokeText;
	[SerializeField] private Button menuGameButton;
	
	[Header("Audio Clip")] 
	[SerializeField] private AudioClip audioClip;
	
	public override void Initialize()
	{
		// go back to the lobby
		menuGameButton.onClick.AddListener(() => ViewManager.Instance.Show<StartGameView>());
		
		base.Initialize();
	}

	public override void Show()
	{
		OwnerPlayerController.SetIsInGame(false);
		
		AudioSource audioSource = FindObjectOfType<AudioSource>();
		Utils.PlaySound(audioClip, audioSource);
		
		base.Show();
	}
}