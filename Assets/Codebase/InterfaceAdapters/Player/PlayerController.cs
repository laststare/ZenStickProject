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
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly IStick _iStick;
        private PlayerView _view;
        
        public PlayerController(IContentProvider contentProvider, IGameFlow iGameFlow, 
            IStick iStick, ILevelBuilder iLevelBuilder, PlayerViewModel playerViewModel)
        {
            _contentProvider = contentProvider;
            _playerViewModel = playerViewModel;
            _iLevelBuilder = iLevelBuilder;
            _iStick = iStick;
            _iGameFlow = iGameFlow;
            
            _iGameFlow.LevelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }).AddTo(_disposables);
            
            _playerViewModel.PlayerFinishMoving.Subscribe(PlayerOnNextColumn).AddTo(_disposables);
            
            CreateView();
        }
        
        private void CreateView()
        {
            _view = Object.Instantiate(_contentProvider.PlayerView());
            _view.Init(_playerViewModel, _iGameFlow);
        }
        
        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _iLevelBuilder.ActualColumnXPosition + 1 +  _iStick.StickLength;
            _iGameFlow.ColumnIsReachable.SetValueAndForceNotify(moveDistance >= _iLevelBuilder.NextColumnXPosition - 1.25f &&
                                                                moveDistance <= _iLevelBuilder.NextColumnXPosition + 1.25f);
            var playerDestination = _iGameFlow.ColumnIsReachable.Value
                ? _iLevelBuilder.NextColumnXPosition + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _playerViewModel.MovePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_iGameFlow.ColumnIsReachable.Value)
                _iGameFlow.ChangeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else 
                _iGameFlow.FinishLevel.Notify();
        }
        
    }
}