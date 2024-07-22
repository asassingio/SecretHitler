using System;
using System.Collections;
using Enums;
using FishNet.Object;
using PlayerComponents;
using UnityEngine;

namespace Managers.CanvasManagers
{
	public sealed class ViewManager : Manager<ViewManager>
	{
		[SerializeField] private View defaultView;

		public override void ManualInitialize(MyPlayerController playerController = null)
		{
			base.ManualInitialize(playerController);
			StartCoroutine(InitializeSecondCoroutine());
		}


		/// <summary>
		/// if there is a defaultView, shows it. needs a Coroutine because should do that after the Manager Coroutine
		/// </summary>
		private IEnumerator InitializeSecondCoroutine()
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
