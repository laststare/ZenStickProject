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
        public void Init(PlayerViewModel playerViewModel)
        {
            _playerViewModel = playerViewModel;
            _playerViewModel.startLevel.Subscribe(() =>
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