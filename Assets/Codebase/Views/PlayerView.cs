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
        private PlayerViewModel _playerViewModel;
        private GameFlowViewModel _gameFlowViewModel;
        public void Init(PlayerViewModel playerViewModel, GameFlowViewModel gameFlowViewModel)
        {
            _playerViewModel = playerViewModel;
            _gameFlowViewModel = gameFlowViewModel;
            _gameFlowViewModel.startLevel.Subscribe(() =>
            {
                transform.position = new Vector2( Constant.PlayerOnColumnXOffset, Constant.PlayerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);

            _playerViewModel.movePlayerTo.SubscribeWithSkip(x => transform.DOMoveX(x, 2).OnComplete(() =>
            {
                _playerViewModel.playerFinishMoving.Notify();
            })).AddTo(this);
        }
        
    }
}