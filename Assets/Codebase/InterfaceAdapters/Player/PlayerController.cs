using Codebase.Data;
using Codebase.InterfaceAdapters.GameFlow;
using Codebase.InterfaceAdapters.LevelBuilder;
using Codebase.InterfaceAdapters.Stick;
using Codebase.Utilities;
using Codebase.Views;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace Codebase.InterfaceAdapters.Player
{
    public class PlayerController : DisposableBase, IPlayer
    {
        private readonly PlayerViewModel _playerViewModel;
        private readonly IContentProvider _contentProvider;
        private readonly IGameFlow _iGameFlow;
        private readonly ILevelBuilder _iLevelBuilder;
        private readonly IStick _iStick;
        private PlayerView _view;
        public ReactiveEvent<float> movePlayerTo { get; set; }
        public ReactiveTrigger playerFinishMoving { get; set; }
        public ReactiveProperty<bool> columnIsReachable { get; set; }
        
        public PlayerController(IContentProvider contentProvider, 
            IGameFlow iGameFlow, 
            IStick iStick, 
            ILevelBuilder iLevelBuilder, 
            PlayerViewModel playerViewModel)
        {
            movePlayerTo = new ReactiveEvent<float>();
            playerFinishMoving = new ReactiveTrigger();
            columnIsReachable = new ReactiveProperty<bool>();
            
            _contentProvider = contentProvider;
            _playerViewModel = playerViewModel;
            _iLevelBuilder = iLevelBuilder;
            _iStick = iStick;
            _iGameFlow = iGameFlow;
            
            _playerViewModel.startLevel = _iGameFlow.startLevel;
            
            _iGameFlow.levelFlowState.Subscribe(x =>
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
            _view.Init(_playerViewModel);
        }
        
        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _iLevelBuilder.actualColumnXPosition + 1 +  _iStick.stickLength.Value;
            _playerViewModel.columnIsReachable.SetValueAndForceNotify(moveDistance >= _iLevelBuilder.nextColumnXPosition - 1.25f &&
                                                                      moveDistance <= _iLevelBuilder.nextColumnXPosition + 1.25f);
            var playerDestination = _playerViewModel.columnIsReachable.Value
                ? _iLevelBuilder.nextColumnXPosition + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _playerViewModel.movePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_playerViewModel.columnIsReachable.Value)
                _iGameFlow.changeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else 
                _iGameFlow.finishLevel.Notify();
        }
        
    }
}