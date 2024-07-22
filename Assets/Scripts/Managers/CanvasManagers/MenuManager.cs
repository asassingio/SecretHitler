using System;
using System.Linq;
using Enums;
using FishNet.Connection;
using FishNet.Object;

namespace Managers.CanvasManagers
{
	public sealed class MenuManager : CanvasManagers.Manager<MenuManager>
	{
		[ServerRpc(RequireOwnership = false)]
		public void ShowToAll(AllMenuListEnum gameMenu)
		{
			RpcShowToAll(gameMenu);
		}
		
		[ObserversRpc]
		private void RpcShowToAll(AllMenuListEnum gameMenu)
		{
			switch (gameMenu)
			{
				case AllMenuListEnum.VotoCancelliere:
					Show<GameViewElectionMenu>();
					break;
				case AllMenuListEnum.GuardaTesseraDiPartito:
					Show<GameViewShowCardMenu>();
					break;
				case AllMenuListEnum.Eliminazione:
					Show<GameViewEliminationMenu>();
					break;
				case AllMenuListEnum.VotoTerzoPotereBanchiere:
					Show<GameViewThirdPowerMenu>();
					break;
				case AllMenuListEnum.PrimoGiocatore:
					Show<GameViewFirstPlayerMenu>();
					break;
				case AllMenuListEnum.Team:
					Show<GameViewTeamMenu>();
					break;
				case AllMenuListEnum.NuovoComputoCarte:
					Show<GameViewDeckMenu>();
					break;
				case AllMenuListEnum.Meme:
					Show<GameViewMemeMenu>();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(gameMenu), gameMenu, null);
			}
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerUpdateCardsGui(CardTypeEnum card1, CardTypeEnum card2, CardTypeEnum card3)
		{
			RpcUpdateCardsGui(card1, card2, card3);
		}

		[ObserversRpc]
		private void RpcUpdateCardsGui(CardTypeEnum card1, CardTypeEnum card2, CardTypeEnum card3)
		{
			foreach (GameViewDeckMenu view in views.OfType<GameViewDeckMenu>())
			{
				view.UpdateCardsGui(card1, card2, card3);
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerUpdateCardsWithSkipCardGui(CardTypeEnum cardToSkip)
		{
			RpcUpdateCardsWithSkipCardGui(cardToSkip);
		}
		
		[ObserversRpc]
		private void RpcUpdateCardsWithSkipCardGui(CardTypeEnum cardToSkip)
		{
			foreach (GameViewDeckMenu view in views.OfType<GameViewDeckMenu>())
			{
				view.UpdateCardsWithSkipCardGui(cardToSkip);
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerAddVoteLog(string playerVoteLog, int yesCount, int noCount)
		{
			RpcAddVoteLog(playerVoteLog, yesCount, noCount);
		}

		[ObserversRpc]
		private void RpcAddVoteLog(string playerVoteLog, int yesCount, int noCount)
		{
			foreach (GameViewElectionMenu view in views.OfType<GameViewElectionMenu>())
			{
				view.AddVoteLog(playerVoteLog, yesCount, noCount);
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerShowFirstPlayer(string randomPlayerName)
		{
			RpcShowFirstPlayer(randomPlayerName);
		}
    
		[ObserversRpc]
		private void RpcShowFirstPlayer(string randomPlayerName)
		{
			foreach (GameViewFirstPlayerMenu view in views.OfType<GameViewFirstPlayerMenu>())
			{
				view.ShowFirstPlayer(randomPlayerName);
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerShowCardToTarget(NetworkConnection conn, string pName, SecretRoleEnum sRole)
		{
			RpcShowCardToTarget(conn, pName, sRole);
			RpcShowCardToTargetLog();
		}
	
		[TargetRpc]
		private void RpcShowCardToTarget(NetworkConnection conn, string pName, SecretRoleEnum sRole)
		{
			foreach (GameViewShowCardMenu view in views.OfType<GameViewShowCardMenu>())
			{
				view.ShowCardToTarget(pName, sRole);
			}
		}
	
		[ObserversRpc]
		private void RpcShowCardToTargetLog()
		{
			foreach (GameViewShowCardMenu view in views.OfType<GameViewShowCardMenu>())
			{
				view.ShowCardToTargetLog();
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerKillTargetLog(string playerThatWillDieName)
		{
			RpcServerKillTargetLog(playerThatWillDieName);
		}
	
		[ObserversRpc]
		private void RpcServerKillTargetLog(string playerThatWillDieName)
		{
			foreach (GameViewEliminationMenu view in views.OfType<GameViewEliminationMenu>())
			{
				view.KillTargetLog(playerThatWillDieName);
			}
		}
		
		[ServerRpc(RequireOwnership = false)]
		public void ServerVoteToKillTarget(string playerName)
		{
			RpcVoteToKillTarget(playerName);
		}
	
		[ObserversRpc]
		private void RpcVoteToKillTarget(string playerName)
		{
			foreach (GameViewThirdPowerMenu view in views.OfType<GameViewThirdPowerMenu>())
			{
				view.VoteToKillTarget(playerName);
			}
			
		}
	}
}