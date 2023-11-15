using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.Player;
using Codebase.Utilities;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Codebase.Views
{
    public class PlayerView : ViewBase
    {
        public void Init(PlayerViewModel playerViewModel, IGameFlow iGameFlow)
        {
            iGameFlow.StartLevel.Subscribe(() =>
            {
                transform.position = new Vector2( Constant.PlayerOnColumnXOffset, Constant.PlayerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);

            playerViewModel.MovePlayerTo.SubscribeWithSkip(x => transform.DOMoveX(x, 2).OnComplete(() =>
            {
                playerViewModel.PlayerFinishMoving.Notify();
            })).AddTo(this);
        }
        
    }
}