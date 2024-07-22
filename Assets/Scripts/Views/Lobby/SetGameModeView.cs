using Managers.CanvasManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Lobby
{
	public sealed class SetGameModeView : View
	{
		[Header("Main GUI")] 
		[SerializeField] private Button appOnlyGameModeButton;
		[SerializeField] private Button appAidGameModeButton;
		[SerializeField] private TextMeshProUGUI workInProgressText;

		private string _gameMode;
	
		public override void Initialize()
		{
			appOnlyGameModeButton.onClick.AddListener(() => workInProgressText.text = $"(\u309cД\u309c)");
			appAidGameModeButton.onClick.AddListener(() => ViewManager.Instance.Show<SetPlayerView>());
		
			base.Initialize();
		}

		protected override void ResetAllVar()
		{
			workInProgressText.text = "";
			base.ResetAllVar();
		}
	}
}