using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.Stick;
using Codebase.Utilities;
using Codebase.Views;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerController : DisposableBase
    {
        private readonly PlayerViewModel _playerViewModel;
        private readonly IContentProvider _contentProvider;
        private readonly GameFlowViewModel _gameFlowViewModel;
        private readonly LevelBuilderViewModel _levelBuilderViewModel;
        private readonly StickViewModel _stickViewModel;
        private PlayerView _view;
        
        public PlayerController(IContentProvider contentProvider, PlayerViewModel playerViewModel, 
            GameFlowViewModel gameFlowViewMode, LevelBuilderViewModel levelBuilderViewModel, StickViewModel stickViewModel)
        {
            _contentProvider = contentProvider;
            _playerViewModel = playerViewModel;
            _gameFlowViewModel = gameFlowViewMode;
            _levelBuilderViewModel = levelBuilderViewModel;
            _stickViewModel = stickViewModel;
            _gameFlowViewModel.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }).AddTo(_disposables);
            _playerViewModel.playerFinishMoving.Subscribe(PlayerOnNextColumn).AddTo(_disposables);
            CreateView();
        }
        
        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.PlayerView());
            _view.Init(_playerViewModel, _gameFlowViewModel);
        }
        
        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _levelBuilderViewModel.actualColumnXPosition.Value + 1 + _stickViewModel.stickLength.Value;
            _playerViewModel.columnIsReachable.SetValueAndForceNotify(moveDistance >= _levelBuilderViewModel.nextColumnXPosition.Value - 1.25f &&
                                                                      moveDistance <= _levelBuilderViewModel.nextColumnXPosition.Value + 1.25f);
            var playerDestination = _playerViewModel.columnIsReachable.Value
                ? _levelBuilderViewModel.nextColumnXPosition.Value + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _playerViewModel.movePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_playerViewModel.columnIsReachable.Value)
                _gameFlowViewModel.changeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else 
                _gameFlowViewModel.finishLevel.Notify();
        }
    }
}