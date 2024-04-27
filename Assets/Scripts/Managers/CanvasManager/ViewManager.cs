using System;
using System.Collections;
using Enums;
using FishNet.Object;
using UnityEngine;

namespace Managers.CanvasManager
{
	public sealed class ViewManager : Manager<ViewManager>
	{
		[SerializeField] private bool autoInitialize;
		[SerializeField] private View defaultView;

		private void Start()
		{
			if (autoInitialize) Initialize();
		}

		public override void Initialize(PlayerController playerController = null)
		{
			base.Initialize(playerController);
			StartCoroutine(InitializeCoroutine());
		}

		private IEnumerator InitializeCoroutine()
		{
			while (!IsInitialized)
			{
				yield return null;
			}

			if (defaultView != null) defaultView.Show();
		}


		[ServerRpc(RequireOwnership = false)]
		public void ShowToAll(AllViewListEnum gameView)
		{
			RpcShowToAll(gameView);
		}
		
		[ObserversRpc]
		private void RpcShowToAll(AllViewListEnum gameView)
		{
			switch (gameView)
			{
				case AllViewListEnum.SetPlayerView:
					// Show<SetPlayerView>();
					break;
				case AllViewListEnum.StartGameView:
					Show<StartGameView>();
					break;
				case AllViewListEnum.GameView:
					Show<GameView>();
					break;
				case AllViewListEnum.GameOverView:
					Show<GameOverView>();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(gameView), gameView, null);
			}
		}
	}
}
