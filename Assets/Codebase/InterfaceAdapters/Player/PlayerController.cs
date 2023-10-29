using Codebase.Data;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerController : DisposableBase
    {
        private readonly PlayerViewModel _playerViewModel;
        private readonly ContentProvider _contentProvider;
        private PlayerView _view;
        
        public PlayerController(ContentProvider contentProvider, PlayerViewModel playerViewModel)
        {
            _contentProvider = contentProvider;
            _playerViewModel = playerViewModel;
            _playerViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }).AddTo(_disposables);
            _playerViewModel.playerFinishMoving.Subscribe(PlayerOnNextColumn).AddTo(_disposables);
            CreateView();
        }
        
        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.Views.PlayerView);
            _view.Init(_playerViewModel);
        }
        
        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _playerViewModel.actualColumnXPosition.Value + 1 + _playerViewModel.stickLength.Value;
            _playerViewModel.columnIsReachable.SetValueAndForceNotify(moveDistance >= _playerViewModel.nextColumnXPosition.Value - 1.25f &&
                                                                      moveDistance <= _playerViewModel.nextColumnXPosition.Value + 1.25f);
            var playerDestination = _playerViewModel.columnIsReachable.Value
                ? _playerViewModel.nextColumnXPosition.Value + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _playerViewModel.movePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_playerViewModel.columnIsReachable.Value)
                _playerViewModel.changeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else 
                _playerViewModel.finishLevel.Notify();
        }
    }
}